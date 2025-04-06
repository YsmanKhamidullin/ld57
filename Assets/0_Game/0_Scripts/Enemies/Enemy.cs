using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// new EnemyAttack.AttackPattern
// {
//     patternName = "DiagonalMain",
//     targetPositions = new List<Vector2Int>
//     {
//         new Vector2Int(0, 0),
//         new Vector2Int(1, 1),
//         new Vector2Int(2, 2)
//     }
// },
// new EnemyAttack.AttackPattern
// {
//     patternName = "DiagonalSecond",
//     targetPositions = new List<Vector2Int>
//     {
//         new Vector2Int(2, 0),
//         new Vector2Int(1, 1),
//         new Vector2Int(0, 2)
//     }
// }
// new EnemyAttack.AttackPattern
// {
//     patternName = "First Row",
//     targetPositions = new List<Vector2Int>
//     {
//         new Vector2Int(0, 0),
//         new Vector2Int(1, 0),
//         new Vector2Int(2, 0)
//     }
// },

[Serializable]
public class DialogueWithEnemy
{
    public bool IsPlayerTalk;

    [TextArea]
    public string PlayerText;

    [TextArea]
    public string EnemyText;
}

public abstract class Enemy : MonoBehaviour, IWill
{
    public abstract List<EnemyAttack.AttackPattern> AttackPatterns { get; }
    public abstract Vector2Int AttacksCount { get; }
    public Color Color => GetComponent<EnemyAttack>().attackColor;
    
    public string EnemyName;

    public Sprite EnemySprite;

    private int _currentWill = 1;

    [TextArea]
    [SerializeField]
    public List<string> _beforeAttackText;
    
    [SerializeField]
    public List<DialogueWithEnemy> _noWillText;

    [SerializeField]
    private int _maxWill;

    [SerializeField]
    private float _betweenAttacksTime = 0.75f;

    [SerializeField]
    private int _listenCountToMercy;

    public int CurrentWill
    {
        get => _currentWill;
        set => _currentWill = Math.Clamp(value, 0, _maxWill);
    }

    public int MaxWill
    {
        get => _maxWill;
        set => _maxWill = value;
    }

    [field: SerializeField]
    public int Damage { get; private set; } = 1;

    private void Awake()
    {
        _currentWill = _maxWill;
    }

    public void TakeDamage(int dmg)
    {
        CurrentWill -= dmg;
    }

    public void TakeListenDamage()
    {
        _listenCountToMercy--;
        if (_listenCountToMercy == 0)
        {
            CurrentWill = 0;
        }
    }

    public async UniTask Attack(List<BattleCell> battleCells)
    {
        await UniTask.WaitForSeconds(0.5f);
        var attacksCount = Random.Range(AttacksCount.x, AttacksCount.y + 1);
        var attackTasks = new List<UniTask>();
        for (int i = 0; i < attacksCount; i++)
        {
            var a = GetComponent<EnemyAttack>().Attack(AttackPatterns, battleCells, this);
            attackTasks.Add(a);
            await UniTask.WaitForSeconds(_betweenAttacksTime);
        }

        await UniTask.WhenAll(attackTasks);
        await UniTask.WaitForSeconds(0.2f);
    }
}