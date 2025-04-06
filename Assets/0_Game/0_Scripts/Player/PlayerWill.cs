using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWill : MonoBehaviour
{
    [SerializeField]
    private Image fillImage;

    public int prevValue;
    public int curValue;
    private Sequence _curSeq;

    private void Start()
    {
        curValue = Root.Instance.Player.CurrentWill;
        prevValue = curValue;
    }

    private void Update()
    {
        curValue = Root.Instance.Player.CurrentWill;
        bool isEquals = prevValue == curValue;
        prevValue = curValue;
        if (!isEquals)
        {
            UpdateImage();
        }
    }

    private void UpdateImage()
    {
        _curSeq.Kill();
        fillImage.DOKill();
        fillImage.transform.DOKill();
        fillImage.transform.localScale = Vector3.one;
        _curSeq = DOTween.Sequence().Append(fillImage.transform.DOPunchScale(Vector3.one * 0.4f, 0.25f))
            .Append(fillImage.transform.DOScale(Vector3.one, 0.15f));
        fillImage.DOFillAmount(Root.Instance.Player.CurrentWill / (float)Root.Instance.Player.MaxWill, 0.15f)
            .SetEase(Ease.OutSine);
    }
}