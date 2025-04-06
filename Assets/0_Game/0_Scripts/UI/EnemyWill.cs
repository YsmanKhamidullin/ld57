using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWill : MonoBehaviour
{
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private TextMeshProUGUI _label;

    private Sequence _curSeq;
    private Enemy _enemy;

    public void SetUp(Enemy enemy)
    {
        _enemy = enemy;
        fillImage.fillAmount = 1f;
        fillImage.color = enemy.Color;
        _label.color = enemy.Color;
    }

    public void UpdateCurrent()
    {
        _curSeq.Kill();
        fillImage.DOKill();
        fillImage.transform.DOKill();
        fillImage.transform.localScale = Vector3.one;
        _curSeq = DOTween.Sequence().Append(fillImage.transform.DOPunchScale(Vector3.one * 0.4f, 0.25f))
            .Append(fillImage.transform.DOScale(Vector3.one, 0.15f));
        fillImage.DOFillAmount(_enemy.CurrentWill / (float)_enemy.MaxWill, 0.15f)
            .SetEase(Ease.OutSine);
    }
}