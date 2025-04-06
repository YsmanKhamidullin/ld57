using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardBlocker : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private Image _lockerImage;

    [SerializeField]
    private Transform _lockerImageTargetPos;

    [SerializeField]
    private Vector3 _lockerImageStartPos;

    public bool IsBlocked;

    private void Awake()
    {
        _lockerImageStartPos = _lockerImage.transform.localPosition;
    }

    public void Block()
    {
        IsBlocked = true;
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _lockerImage.transform.localPosition = _lockerImageStartPos;
    }

    public async UniTask UnBlock()
    {
        await _lockerImage.transform.DOLocalMove(_lockerImageTargetPos.localPosition, 0.15f).SetEase(Ease.OutQuad)
            .ToUniTask();
        await _canvasGroup.DOFade(0f, 0.2f).SetEase(Ease.OutSine).ToUniTask();
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        IsBlocked = false;
    }
}