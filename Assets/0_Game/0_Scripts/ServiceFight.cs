using Cysharp.Threading.Tasks;
using UnityEngine;

public class ServiceFight : MonoBehaviour
{
    public async UniTask ForceStartFight(LadderStep step)
    {
        var player = Root.Instance.Player;
        var enemy = step.Enemies.GetRandom();
        Root.Instance.ServiceUi.HideGamePlay();
        await Root.Instance.ServiceUi.FadeIn(0.25f);
        var f = Root.Instance.ServiceUi.ShowFightWindow();
        f.SetUp(this, player, enemy);
        await Root.Instance.ServiceUi.FadeOut(0.15f);
        await f.AnimateAppearAndFight();
        
        // wait finish fight

        await UniTask.WaitUntil(() =>
        {
            bool isAnyDead = player.CurrentWill <= 0 || enemy.CurrentWill <= 0;
            return f._isActionState == false && isAnyDead;
        });
        CheckWhoWin(player, enemy);
        await Root.Instance.ServiceUi.FadeIn(0.25f);
        Root.Instance.ServiceUi.HideFightWindow();
        Root.Instance.ServiceUi.ShowGamePlay();
        await Root.Instance.ServiceUi.FadeOut(0.15f);
    }

    public void UseTalk()
    {
        
    }

    public void UseListen()
    {
        
    }

    public void UseFlee()
    {
        
    }

    private void CheckWhoWin(Player player, Enemy enemy)
    {
        bool isPlayerDead = player.CurrentWill <= 0;
        bool isEnemyDead = enemy.CurrentWill <= 0;
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
}