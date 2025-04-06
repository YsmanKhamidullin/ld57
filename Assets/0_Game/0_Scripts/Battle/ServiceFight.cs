using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ServiceFight : MonoBehaviour
{
    private Player _player;
    private Enemy _enemy;
    private FightWindow _fightWindow;
    private LadderStep _currentLadderStep;
    public bool InFight { get; set; }

    public async UniTask ForceStartFight(LadderStep step)
    {
        InFight = true;
        _currentLadderStep = step;
        _player = Root.Instance.Player;
        _enemy = step.GetEnemy();
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
        await HandleFinishBeforeExit();
        await Root.Instance.ServiceUi.FadeIn(0.25f);
        Root.Instance.ServiceUi.HideFightWindow();
        Root.Instance.ServiceUi.ShowGamePlay();
        InFight = false;
        await Root.Instance.ServiceUi.FadeOut(0.15f);
        await HandleFinishAfterExit();
    }

    private async UniTask HandleFinishBeforeExit()
    {
        bool isEnemyDead = _enemy.CurrentWill <= 0;
        if (isEnemyDead)
        {
            _currentLadderStep.TryRemove(_enemy);
            await _fightWindow.HandleEnemyDeath(_enemy);
        }
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
        await Root.Instance.Mind.Decrement();
        _enemy.TakeDamage(_player.TalkDamage);

        _fightWindow.UpdateWill();
        await PhaseBattle();
    }

    public async UniTask UseListen()
    {
        await Root.Instance.Mind.Increment();
        _enemy.TakeListenDamage();
        await PhaseBattle();
    }

    public async UniTask UseFlee()
    {
        await Root.Instance.ServiceUi.FadeIn(0.25f);
        Root.Instance.ServiceUi.HideFightWindow();
        Root.Instance.ServiceUi.ShowGamePlay();
        InFight = false;
        await Root.Instance.ServiceUi.FadeOut(0.15f);
        await Root.Instance.ServiceCards.UseCardByType(CardTypes.Backward);
    }

    private async UniTask HandleFinishAfterExit()
    {
        bool isPlayerDead = _player.CurrentWill <= 0;
        bool isEnemyDead = _enemy.CurrentWill <= 0;
        if (isPlayerDead)
        {
            await Root.Instance.ServiceCards.UseCardByType(CardTypes.Backward);
            await Root.Instance.Mind.SetToZero();
            Root.Instance.Player.WillToMax();
            await UniTask.WaitForSeconds(0.2f);
        }
        else if (isEnemyDead)
        {
            var n = Root.Instance.ServiceLadder.GetNextStep();
            await Root.Instance.ServiceCards.TryInteractByNextStep(n);
        }
    }

    public async UniTask MovePlayerTo(int index)
    {
        await _fightWindow.MovePlayerTo(index);
    }

    public void TryMovePlayerTo(BattleCell battleCell)
    {
        _ = _fightWindow.TryMovePlayerTo(battleCell);
    }
}