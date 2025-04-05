using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ServiceFight : MonoBehaviour
{
    private Player _player;
    private Enemy _enemy;
    private FightWindow _fightWindow;

    public async UniTask ForceStartFight(LadderStep step)
    {
        _player = Root.Instance.Player;
        _enemy = step.GetEnemyAndRemove();
        Root.Instance.ServiceUi.HideGamePlay();
        await Root.Instance.ServiceUi.FadeIn(0.25f);
        _fightWindow = Root.Instance.ServiceUi.ShowFightWindow();
        _fightWindow.SetUp(this, _player, _enemy);
        await Root.Instance.ServiceUi.FadeOut(0.15f);
        _ = StartBattle();
    }

    private async UniTask StartBattle()
    {
        await _fightWindow.SetUpActionPhase();
        await UniTask.WaitUntil(() => _player.CurrentWill <= 0 || _enemy.CurrentWill <= 0);
        await FinishBattle();
    }

    private async UniTask FinishBattle()
    {
        CheckWhoWin();
        await Root.Instance.ServiceUi.FadeIn(0.25f);
        Root.Instance.ServiceUi.HideFightWindow();
        Root.Instance.ServiceUi.ShowGamePlay();
        await Root.Instance.ServiceUi.FadeOut(0.15f);
    }

    private async UniTask PhaseBattle()
    {
        if (_enemy.CurrentWill <= 0)
        {
            return;
        }

        await _fightWindow.SetUpBattlePhase();
        await _enemy.Attack(_fightWindow.BattleField.Cells);
        await _fightWindow.SetUpActionPhase();
    }

    public async UniTask UseTalk()
    {
        await Root.Instance.Mind.IncrementTalk();
        _enemy.TakeDamage(_player.TalkDamage);
        _fightWindow.UpdateWill();
        await PhaseBattle();
    }

    public async UniTask UseListen()
    {
        await Root.Instance.Mind.IncrementListen();
        _enemy.TakeListenDamage();
        await PhaseBattle();
    }

    public async UniTask UseFlee()
    {
        Debug.Log("Trying Flee");
    }

    private void CheckWhoWin()
    {
        bool isPlayerDead = _player.CurrentWill <= 0;
        bool isEnemyDead = _enemy.CurrentWill <= 0;
        if (isEnemyDead)
        {
            PlayerWin();
            return;
        }

        EnemyWin();
    }

    private void PlayerWin()
    {
    }

    private void EnemyWin()
    {
    }

    public async UniTask MovePlayerTo(int index)
    {
        await _fightWindow.MovePlayerTo(index);
    }

    public void TryMovePlayerTo(BattleCell battleCell)
    {
        _fightWindow.TryMovePlayerTo(battleCell);
    }
}