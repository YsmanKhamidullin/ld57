using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Root.Instance.ServiceAudio.PlayBattleStart();
        Root.Instance.ServiceAudio.PauseBackgroundMusic();

        await Root.Instance.ServiceUi.FadeIn(0.25f);
        _fightWindow = Root.Instance.ServiceUi.ShowFightWindow();
        _fightWindow.SetUp(this, _player, _enemy);
        await Root.Instance.ServiceUi.FadeOut(0.15f);
        await Onboarding.TryBattleSecondTutorial();
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
        Root.Instance.ServiceCards.IsBlocked = true;
        await HandleFinishBeforeExit();
        Root.Instance.ServiceAudio.PlayBattleEnd();
        if (CheckWinGame())
        {
            Main.SetFirstEndingAchieved();
            var f = Root.Instance.ServiceUi.ShowFirstEnding();
            await f.Animate();
            SceneManager.LoadScene(0);
            return;
        }

        bool isPlayerDead = _player.CurrentWill <= 0;
        if (isPlayerDead)
        {
            Root.Instance.ServiceAudio.PlayDeath();
        }

        Root.Instance.BattleCardsContainer.gameObject.SetActive(false);
        await Root.Instance.ServiceUi.FadeIn(0.25f);
        Root.Instance.ServiceUi.HideFightWindow();
        Root.Instance.ServiceUi.ShowGamePlay();
        InFight = false;
        await Root.Instance.ServiceUi.FadeOut(0.15f);
        await HandleFinishAfterExit();
        await OnBattleEnd();
        Root.Instance.ServiceCards.IsBlocked = false;
    }

    public async UniTask UseFlee()
    {
        await Root.Instance.ServiceUi.FadeIn(0.25f);
        Root.Instance.ServiceUi.HideFightWindow();
        Root.Instance.ServiceUi.ShowGamePlay();
        InFight = false;
        await Root.Instance.ServiceUi.FadeOut(0.15f);
        await Root.Instance.ServiceCards.UseCardByType(CardTypes.Backward);
        await OnBattleEnd();
    }

    private async UniTask OnBattleEnd()
    {
        Root.Instance.ServiceAudio.UnPauseBackgroundMusic();
        await Root.Instance.PlayerWill.TryUnblockDreamCard();
    }

    private bool CheckWinGame()
    {
        if (_enemy is HorizontalAndVerticalEnemy)
        {
            return true;
        }

        return false;
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

        await Root.Instance.EnemyVisual.SayBeforeAttack();
        await _fightWindow.SetUpBattlePhase();
        await _enemy.Attack(_fightWindow.BattleField.Cells);
        await _fightWindow.SetUpActionPhase();
    }

    public async UniTask UseTalk()
    {
        await Root.Instance.Mind.Decrement();
        _enemy.TakeDamage(_player.TalkDamage);

        await _fightWindow.UpdateWill();
        await PhaseBattle();
    }

    public async UniTask UseListen()
    {
        await Root.Instance.Mind.Increment();
        _enemy.TakeListenDamage();
        await _fightWindow.UpdateWill();
        await PhaseBattle();
    }

    private async UniTask HandleFinishAfterExit()
    {
        bool isEnemyDead = _enemy.CurrentWill <= 0;
        bool isPlayerDead = _player.CurrentWill <= 0;
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