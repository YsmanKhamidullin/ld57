using DG.Tweening;
using TMPro;
using UnityEngine;

public class StepLabel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI label;

    public int prevValue;
    public int curValue;
    private Sequence _curSeq;

    private void Start()
    {
        curValue = Root.Instance.ServiceLadder.CurrentHeight;
        prevValue = curValue;
    }

    private void Update()
    {
        curValue = Root.Instance.ServiceLadder.CurrentHeight;
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
        label.DOKill();
        label.transform.DOKill();
        label.transform.localScale = Vector3.one;
        _curSeq = DOTween.Sequence().Append(label.transform.DOPunchScale(Vector3.one * 0.4f, 0.25f))
            .Append(label.transform.DOScale(Vector3.one, 0.15f));
        label.text = curValue.ToString();
    }
}