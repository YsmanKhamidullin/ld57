using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PlayerHeart : MonoBehaviour
{
    [SerializeField]
    private Transform _scaleRoot;

    private BattleCell _currentBattleCell;
    private bool _isImmuneToDamage;

    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public async UniTask MoveToCell(BattleCell battleCell)
    {
        _currentBattleCell = battleCell;
        _scaleRoot.DOScale(Vector3.one * 1.25f, 0.15f).SetEase(Ease.OutQuint);
        await transform.DOMove(battleCell.transform.position, 0.2f).SetEase(Ease.OutQuint).ToUniTask();
        _scaleRoot.localScale = Vector3.one;
    }

    public bool TryDealDamage(List<BattleCell> battleCells, Enemy enemy)
    {
        if (_isImmuneToDamage)
        {
            return false;
        }

        if (battleCells.Contains(_currentBattleCell))
        {
            TakeDamage(enemy.Damage);
            return true;
        }

        return false;
    }

    private void TakeDamage(int dmg)
    {
        Debug.Log("<color=red>DEAL DAMAGE</color>");
        _isImmuneToDamage = true;
        Root.Instance.Player.TakeDamage(dmg);
        _ = ReactToDamage();
    }

    private async UniTask ReactToDamage()
    {
        Root.Instance.ServiceAudio.PlayGotDamage();
        await _scaleRoot.DOPunchScale(Vector3.one * 0.7f, 0.2f).ToUniTask();
        await UniTask.WaitForSeconds(0.1f);
        _isImmuneToDamage = false;
    }
}