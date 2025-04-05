using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.VisualNovel;
using UnityEngine;
using UnityEngine.UI;

public class TalkBubble : MonoBehaviour
{
    public bool IsTalking { get; private set; }
    
    [SerializeField]
    private Button _skipSayButton;

    [SerializeField]
    private Transform _animRoot;

    [SerializeField]
    private SlowRevealText _slowRevealText;

    private CancellationTokenSource _cancellationToken = new();
    
    private void OnDestroy()
    {
        _animRoot.DOKill();
    }

    public void Dispose()
    {
        _cancellationToken.Cancel();
        _animRoot.DOKill();
    }

    public void ClearText()
    {
        _slowRevealText.SetText("");
    }

    public async UniTask Show()
    {
        _cancellationToken = new CancellationTokenSource();
        gameObject.SetActive(true);
        await _animRoot.DOScale(1f, Timings.Get(0.4f)).From(0f).SetEase(Ease.OutBounce).ToUniTask()
            .AttachExternalCancellation(_cancellationToken.Token).SuppressCancellationThrow();
    }

    public async UniTask Say(string text)
    {
        IsTalking = true;
        text = text.Replace("\n", "");
        _slowRevealText.SetText(text);
        await _slowRevealText.Show();
        await UniTask.WaitForSeconds(Timings.Get(Mathf.Clamp(text.Length / 30, 1, 3)));
        IsTalking = false;
    }

    public async UniTask Hide()
    {
        _animRoot.DOKill();
        _animRoot.DOScale(0f, Timings.Get(0.3f)).SetEase(Ease.InBounce);
        await UniTask.WaitForSeconds(Timings.Get(.15f)).AttachExternalCancellation(_cancellationToken.Token)
            .SuppressCancellationThrow();
        gameObject.SetActive(false);
    }
}