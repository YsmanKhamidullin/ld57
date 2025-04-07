using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class ServiceAudio : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> _keyClips;


    [SerializeField]
    private AudioSource _key;

    [SerializeField]
    private AudioSource _keySecond;

    [SerializeField]
    private AudioSource _startGame;

    [SerializeField]
    private AudioSource _battleStart;

    [SerializeField]
    private AudioSource _battleEnd;

    [SerializeField]
    private AudioSource _bg;

    [SerializeField]
    private AudioSource _enemyAttack;

    [SerializeField]
    private AudioSource _enemyGotDamage;

    [SerializeField]
    private AudioSource _death;

    [SerializeField]
    private AudioSource _gotDamage;

    [SerializeField]
    private AudioSource _step;

    [Button]
    public void PlayKey()
    {
        if (_key.isPlaying)
        {
            return;
        }

        var v = Random.Range(0.85f, 1.15f);
        _key.pitch = v;
        _key.clip = _keyClips.GetRandom();
        _key.Play();
        DOTween.Sequence().InsertCallback(0.075f, () =>
        {
            if (_keySecond.isPlaying)
            {
                return;
            }

            var v1 = Random.Range(0.85f, 1.15f);
            _keySecond.pitch = v1;
            _keySecond.clip = _keyClips.GetRandom();
            _keySecond.Play();
        });
    }

    public void PlayStartGame()
    {
        _startGame.Play();
    }

    public void PlayBattleStart()
    {
        _battleStart.Play();
    }

    public void PlayBattleEnd()
    {
        _battleEnd.Play();
    }

    private float fadeDuration = 2.0f; // Duration of the fade effect in seconds

    private float _bgDefaultVolume;

    private void Start()
    {
        _bgDefaultVolume = _bg.volume;
    }

    [Button]
    public void UnPauseBackgroundMusic()
    {
        _bg.UnPause();
        StartCoroutine(FadeAudio(_bg, 0f, _bgDefaultVolume, fadeDuration));
    }

    [Button]
    public void PauseBackgroundMusic()
    {
        StartCoroutine(FadeAudio(_bg, _bgDefaultVolume, 0f, fadeDuration, () => { _bg.Pause(); }));
    }

    private IEnumerator FadeAudio(AudioSource audioSource, float startVolume, float targetVolume, float duration,
        Action onComplete = null)
    {
        float currentTime = 0;
        float start = startVolume;
        float end = targetVolume;

        audioSource.volume = start;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, end, currentTime / duration);
            yield return null;
        }

        audioSource.volume = end;
        onComplete?.Invoke();
    }

    [Button]
    public void PlayEnemyAttack()
    {
        var v = Random.Range(0.9f, 1.1f);
        _enemyAttack.pitch = v;
        _enemyAttack.Play();
    }

    [Button]
    public void PlayEnemyGotDamage()
    {
        _enemyGotDamage.Play();
    }

    [Button]
    public void PlayDeath()
    {
        _death.Play();
    }

    [Button]
    public void PlayGotDamage()
    {
        _gotDamage.Play();
    }

    [Button]
    public void PlayStep()
    {
        var v = Random.Range(0.9f, 1.1f);
        _step.pitch = v;
        _step.Play();
    }
}