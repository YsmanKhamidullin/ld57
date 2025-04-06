using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.VisualNovel;
using TMPro;
using UnityEngine;

public class FirstEndingWindow : BaseWindow
{
    [SerializeField]
    private SlowRevealText label;

    public async UniTask Animate()
    {
        CanvasGroup.alpha = 0f;
        label.SetText("Ending: 1/2");
        await CanvasGroup.DOFade(1f, 1f).SetEase(Ease.OutSine).ToUniTask();
        await UniTask.WaitForSeconds(0.2f);
        await label.Show();
        await UniTask.WaitForSeconds(1f);
    }
}