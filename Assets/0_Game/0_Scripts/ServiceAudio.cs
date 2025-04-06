using UnityEngine;

public class ServiceAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource _startGame;

    [SerializeField]
    private AudioSource _battleStart;

    [SerializeField]
    private AudioSource _battleEnd;

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
}