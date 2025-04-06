using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ServiceUi : MonoBehaviour
{
    public Image Fader;

    [field: SerializeField]
    public Transform VisualNovelParent { get; private set; }

    [field: SerializeField]
    public GraphicRaycaster FullScreenRayCaster { get; private set; }

    [SerializeField]
    public PauseWindow _pauseWindow;

    [SerializeField]
    public GamePlayWindow _gamePlayWindow;

    [SerializeField]
    public FightWindow _fightWindow;

    [SerializeField]
    public GameStartWindow _gameStartWindow;

    public void ShowGameStart()
    {
    }

    public void HideGameStart()
    {
    }

    public void ShowPause()
    {
    }

    public void HidePause()
    {
    }

    public void ShowGamePlay()
    {
        _gamePlayWindow.Show();
    }

    public void HideGamePlay()
    {
        _gamePlayWindow.Hide();
    }

    public FightWindow ShowFightWindow()
    {
        Root.Instance.ServiceLadder.DisableLadderVisual();
        Root.Instance.BattleCardsContainer.gameObject.SetActive(true);
        _fightWindow.Show();
        return _fightWindow;
    }

    public void HideFightWindow()
    {
        Root.Instance.ServiceLadder.EnableLadderVisual();
        Root.Instance.BattleCardsContainer.gameObject.SetActive(false);
        _fightWindow.Hide();
    }

    public async UniTask FadeIn(float time)
    {
        Fader.raycastTarget = true;
        await AnimationsUtil.FadeIn(Fader, time);
    }

    public async UniTask FadeOut(float time)
    {
        await AnimationsUtil.FadeOut(Fader, time);
        await UniTask.WaitForSeconds(0.2f);
        Fader.raycastTarget = false;
    }
}