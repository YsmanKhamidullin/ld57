using UnityEngine;
using DG.Tweening;

public class Breath : MonoBehaviour
{
    [Header("Breath Settings")]
    [Tooltip("The duration of one complete breath cycle (in seconds)")]
    public float breathDuration = 1.5f;

    [Tooltip("The scale multiplier at the peak of the breath")]
    public float breathScale = 1.1f;

    [Tooltip("The ease type for the breathing animation")]
    public Ease easeType = Ease.InOutSine;

    [Tooltip("Start breathing automatically on awake")]
    public bool playOnAwake = true;

    private Vector3 originalScale;
    private Sequence breathSequence;

    private void Awake()
    {
        originalScale = transform.localScale;

        if (playOnAwake)
        {
            StartBreathing();
        }
    }

    public void StartBreathing()
    {
        if (breathSequence != null && breathSequence.IsPlaying())
        {
            breathSequence.Kill();
        }

        breathSequence = DOTween.Sequence();

        breathSequence.Append(transform.DOScale(originalScale * breathScale, breathDuration / 2f)
            .SetEase(easeType));

        breathSequence.Append(transform.DOScale(originalScale, breathDuration / 2f)
            .SetEase(easeType));

        breathSequence.SetLoops(-1, LoopType.Restart);
    }

    public void StopBreathing()
    {
        if (breathSequence != null)
        {
            breathSequence.Kill();
            transform.localScale = originalScale;
        }
    }

    private void OnDestroy()
    {
        if (breathSequence != null)
        {
            breathSequence.Kill();
        }
    }
}