using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystemVisual;

    private Enemy _enemy;

    public void SetUp()
    {
        gameObject.SetActive(true);
        var mainModule = _particleSystemVisual.main;

        Color startColor = mainModule.startColor.color;
        startColor.a = 0f;
        mainModule.startColor = startColor;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        DOTween.To(
                () => mainModule.startColor.color,
                x => mainModule.startColor = x,
                targetColor,
                1.2f)
            .SetEase(Ease.OutSine);
    }

    public async UniTask Vanish()
    {
        var mainModule = _particleSystemVisual.main;

        Color startColor = mainModule.startColor.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        mainModule.startColor = Color.white;
        await DOTween.Sequence()
            .Append(_particleSystemVisual.transform.DOPunchScale(Vector3.one * 0.7f, 0.3f).SetEase(Ease.OutQuart))
            .ToUniTask();
        _particleSystemVisual.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InSine);
        mainModule.startColor = startColor;
        await DOTween.To(
                () => mainModule.startColor.color,
                x => mainModule.startColor = x,
                targetColor,
                0.5f)
            .SetEase(Ease.InOutSine)
            .ToUniTask();
        await UniTask.WaitForSeconds(1f);
    }

    public async UniTask StartDialogue()
    {
        await UniTask.WaitForSeconds(0.1f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        if (_enemy)
        {
            Destroy(_enemy.gameObject);
        }
    }

    public Enemy SpawnEnemy(Enemy prefab)
    {
        _enemy = Instantiate(prefab, transform);
        _particleSystemVisual = _enemy.GetComponentInChildren<ParticleSystem>();
        return _enemy;
    }
}