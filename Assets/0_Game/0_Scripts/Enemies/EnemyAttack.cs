﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyAttack : MonoBehaviour
{
    [System.Serializable]
    public class AttackPattern
    {
        public string patternName;
        public List<Vector2Int> targetPositions;
    }

    [Header("Attack Settings")]
    [SerializeField]
    private float highlightDuration = 0.5f;

    [SerializeField]
    private float betweenHighlightsDelay = 0.2f;

    [SerializeField]
    private Color attackColor = Color.red;

    [Header("Projectile Settings")]
    [SerializeField]
    private Projectile projectilePrefab;

    [SerializeField]
    private float projectileSpeed = 5f;

    [SerializeField]
    private float hitEffectDuration = 0.3f;

    private Color[,] originalColors;
    private BattleCell[,] gridBattleCells;
    private List<AttackPattern> _attackPatterns;
    private List<BattleCell> _battleCells;
    private Projectile _spawnedProjectile;
    private AttackPattern _selectedAttackPattern;
    private Enemy _enemy;

    private void InitializeGridReferences()
    {
        int rows = (int)Math.Sqrt(_battleCells.Count);
        int columns = rows;

        gridBattleCells = new BattleCell[columns, rows];
        originalColors = new Color[columns, rows];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                int index = y * columns + x;
                gridBattleCells[x, y] = _battleCells[index];
                originalColors[x, y] = new Color(0.1986928f, 0.1882353f, 0.2509804f, 1f);
            }
        }
    }

    public async UniTask Attack(List<AttackPattern> attackPatterns, List<BattleCell> battleCells, Enemy enemy)
    {
        _enemy = enemy;
        _attackPatterns = attackPatterns;
        _battleCells = battleCells;
        InitializeGridReferences();

        if (attackPatterns == null || attackPatterns.Count == 0)
        {
            throw new Exception("No attack patterns defined!");
        }

        _selectedAttackPattern = attackPatterns[Random.Range(0, attackPatterns.Count)];
        Debug.Log($"Attacking with pattern: {_selectedAttackPattern.patternName}");

        await ExecuteAttackPattern(_selectedAttackPattern);
    }

    private async UniTask ExecuteAttackPattern(AttackPattern pattern)
    {
        Vector3 startPosition = transform.position;
        _spawnedProjectile = CreateProjectile(startPosition);
        //pos
        Vector3 firstCell = GetCellWorldPosition(pattern.targetPositions[0].x, pattern.targetPositions[0].y);
        Vector3 lastCell = GetCellWorldPosition(pattern.targetPositions.Last().x, pattern.targetPositions.Last().y);
        Vector3 flightDirection = (lastCell - firstCell).normalized;
        float spawnOffset = 240;
        Vector3 spawnPosition = firstCell - (flightDirection * spawnOffset);
        _spawnedProjectile.transform.position = spawnPosition;

        //rot
        Vector3 direction = (lastCell - _spawnedProjectile.transform.position);
        direction.z = 0;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        _spawnedProjectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        _spawnedProjectile.transform.DOScale(Vector3.one, 0.2f).From(Vector3.zero).SetEase(Ease.InQuad);
        await HighlightAllCells(pattern.targetPositions);
        float reactTime = 0.2f;
        await UniTask.WaitForSeconds(reactTime);
        bool isTutoring = await Onboarding.TryBattleTutorial(0);
        if (isTutoring)
        {
            await Root.Instance.ServiceFight.MovePlayerTo(5);
        }

        await AnimateProjectileThroughCells(_spawnedProjectile, pattern.targetPositions);

        Destroy(_spawnedProjectile.gameObject, 1f);
    }

    private Projectile CreateProjectile(Vector3 position)
    {
        Projectile projectile = Instantiate(projectilePrefab, Root.Instance.ServiceUi._fightWindow.transform);
        projectile.transform.SetPositionAndRotation(position, Quaternion.identity);
        return projectile;
    }

    private async UniTask HighlightAllCells(List<Vector2Int> targetPositions)
    {
        Sequence attackSequence = DOTween.Sequence();

        foreach (var pos in targetPositions)
        {
            if (IsValidGridPosition(pos.x, pos.y))
            {
                HighlightCell(pos.x, pos.y);
                attackSequence.AppendInterval(0.15f);
            }
        }

        await UniTask.WaitForSeconds(0.15f * targetPositions.Count);
    }

    private async UniTask AnimateProjectileThroughCells(Projectile projectile, List<Vector2Int> targetPositions)
    {
        if (targetPositions == null || targetPositions.Count == 0) return;

        var cellPosition = targetPositions.Last();
        var pos = GetCellWorldPosition(cellPosition.x, cellPosition.y);
        Vector3 direction = (pos - projectile.transform.position).normalized;
        await UniTask.WaitForSeconds(0.1f);
        await projectile.transform
            .DOMove(pos + direction * 250f, 1f / projectileSpeed)
            .SetEase(Ease.OutQuad)
            .OnUpdate(TryDealDamageToPlayer)
            .ToUniTask();
        await projectile.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.OutQuad).ToUniTask();
    }

    private void TryDealDamageToPlayer()
    {
        if (!_spawnedProjectile.IsTriedDealDamage && _spawnedProjectile.IsNearHeart())
        {
            _spawnedProjectile.IsTriedDealDamage = true;
            Root.Instance.PlayerHeart.TryDealDamage(GetBattleCells(_selectedAttackPattern), _enemy);
        }
    }

    private List<BattleCell> GetBattleCells(AttackPattern attackPattern)
    {
        var res = new List<BattleCell>();
        foreach (var p in attackPattern.targetPositions)
        {
            res.Add(gridBattleCells[p.x, p.y]);
        }

        return res;
    }

    private Vector3 GetCellWorldPosition(int x, int y)
    {
        return gridBattleCells[x, y].transform.position;
    }

    private void HighlightCell(int x, int y)
    {
        Image image = gridBattleCells[x, y].Image;
        image.DOColor(attackColor, highlightDuration / 2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => image.DOColor(originalColors[x, y], highlightDuration / 2f)
                .SetEase(Ease.InQuad));
    }

    private void ResetAllImages()
    {
        for (int x = 0; x < gridBattleCells.GetLength(0); x++)
        {
            for (int y = 0; y < gridBattleCells.GetLength(1); y++)
            {
                if (gridBattleCells[x, y] != null && originalColors[x, y] != default(Color))
                {
                    gridBattleCells[x, y].Image.color = originalColors[x, y];
                }
            }
        }
    }

    private bool IsValidGridPosition(int x, int y)
    {
        return x >= 0 && x < gridBattleCells.GetLength(0) &&
               y >= 0 && y < gridBattleCells.GetLength(1);
    }
}