using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.VisualNovel;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField]
    private SlowRevealText _slowRevealTextPlayer;

    private ParticleSystem _enemyVisualParticle;
    private SlowRevealText _slowRevealTextEnemy;

    private Enemy _enemy;
    private int _beforeAttackSayIndex;

    public void SetUp()
    {
        _beforeAttackSayIndex = 0;
        gameObject.SetActive(true);
        var mainModule = _enemyVisualParticle.main;

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
        var mainModule = _enemyVisualParticle.main;

        Color startColor = mainModule.startColor.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        mainModule.startColor = Color.white;
        await DOTween.Sequence()
            .Append(_enemyVisualParticle.transform.DOPunchScale(Vector3.one * 0.7f, 0.3f).SetEase(Ease.OutQuart))
            .ToUniTask();
        _enemyVisualParticle.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InSine);
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

    public async UniTask SayBeforeAttack()
    {
        if (_beforeAttackSayIndex >= _enemy._beforeAttackText.Count)
        {
            return;
        }

        var text = _enemy._beforeAttackText[_beforeAttackSayIndex];
        if (text == "")
        {
            return;
        }

        _slowRevealTextEnemy.SetText(text);
        await _slowRevealTextEnemy.Show();
        await UniTask.WaitForSeconds(Mathf.Clamp(text.Length / 30, 1, 3));
        _slowRevealTextEnemy.SetText("");
        _beforeAttackSayIndex++;
    }

    public async UniTask StartDialogueBeforeVanish()
    {
        for (int i = 0; i < _enemy._noWillText.Count; i++)
        {
            var dialogueWithEnemy = _enemy._noWillText[i];
            string text = "";
            if (dialogueWithEnemy.IsPlayerTalk)
            {
                text = dialogueWithEnemy.PlayerText;
                _slowRevealTextPlayer.SetText(text);
                await _slowRevealTextPlayer.Show();
                await UniTask.WaitForSeconds(Mathf.Clamp(text.Length / 30f, 0.75f, 3));
                _slowRevealTextPlayer.SetText("");
            }
            else
            {
                text = dialogueWithEnemy.EnemyText;
                _slowRevealTextEnemy.SetText(text);
                await _slowRevealTextEnemy.Show();
                await UniTask.WaitForSeconds(Mathf.Clamp(text.Length / 30f, 0.75f, 3));
                _slowRevealTextEnemy.SetText("");
            }
        }
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
        _enemyVisualParticle = _enemy.GetComponentInChildren<ParticleSystem>();
        var mainModule = _enemyVisualParticle.main;
        mainModule.startColor = _enemy.Color;
        _slowRevealTextEnemy = _enemy.GetComponentInChildren<SlowRevealText>();
        _slowRevealTextEnemy.SetText("");
        _slowRevealTextEnemy.SetColor(_enemy.Color);
        return _enemy;
    }
}