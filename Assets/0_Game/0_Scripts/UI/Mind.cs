using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class Mind : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private Image arrowImage;

    [SerializeField]
    private Transform clockImage;

    public static int MindValue;


    public async UniTask SetToZero()
    {
        await _canvasGroup.Fade(1f, 0.4f);
        MindValue = 0;
        await RotateToCurrent();
        await _canvasGroup.Fade(0f, 0.2f);
        await UniTask.WaitForSeconds(0.15f);
    }

    public async UniTask Increment()
    {
        await _canvasGroup.Fade(1f, 0.4f);
        MindValue = Mathf.Clamp(MindValue + 1, -5, 5);
        await RotateToCurrent();
        await _canvasGroup.Fade(0f, 0.2f);
        await UniTask.WaitForSeconds(0.15f);
    }

    [Button]
    public async UniTask Decrement()
    {
        await _canvasGroup.Fade(1f, 0.4f);
        MindValue = Mathf.Clamp(MindValue - 1, -5, 5);
        await RotateToCurrent();
        await _canvasGroup.Fade(0f, 0.2f);
        await UniTask.WaitForSeconds(0.15f);
    }

    public async UniTask UpdateByCurrentValue()
    {
        await _canvasGroup.Fade(1f, 0.4f);
        await RotateToCurrent();
        await _canvasGroup.Fade(0f, 0.2f);
        await UniTask.WaitForSeconds(0.15f);
    }

    private async UniTask RotateToCurrent()
    {
        var targetZRot = MindValue * -15f;
        Vector3 newRot = new Vector3(0, 0, targetZRot);
        await arrowImage.transform.DORotate(newRot, 1f, RotateMode.Fast).SetEase(Ease.InOutCirc).ToUniTask();
    }
}