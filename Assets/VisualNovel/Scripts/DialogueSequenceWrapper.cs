using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.VisualNovel;
using UnityEngine;
using Object = UnityEngine.Object;

public class DialogueSequenceWrapper : MonoBehaviour
{
    private Transform _parent;
    private ServiceUi _gameWindows;

    private void Start()
    {
        _gameWindows = Root.Instance.ServiceUi;
        _parent = _gameWindows.VisualNovelParent;
    }

    public async UniTask StartSequence(DialogueSequence dialogueSequence, NPC npc = null, bool isSkipFade = true)
    {
        if (!isSkipFade)
        {
            await _gameWindows.FadeIn(0.3f);
        }

        var instance = InstantiateDefault(dialogueSequence);
        instance.OneAlpha();
        instance.HideAll();
        if (!isSkipFade)
        {
            await _gameWindows.FadeOut(0.2f);
        }

        await instance.Play();
        if (!isSkipFade)
        {
            await _gameWindows.FadeIn(0.3f);
        }

        if (npc != null)
        {
            npc.HideNpc();
        }

        instance.gameObject.SetActive(false);
        if (!isSkipFade)
        {
            await _gameWindows.FadeOut(0.2f);
        }

        Object.Destroy(instance.gameObject);
    }

    private async UniTask StartSequence(string path)
    {
        var prefab = Resources.Load<DialogueSequence>(path);
        var instance = InstantiateDefault(prefab);
        await instance.Play();
        Object.Destroy(instance.gameObject);
    }

    private DialogueSequence InstantiateDefault(DialogueSequence prefab)
    {
        var instance = Object.Instantiate(prefab, _parent);
        return instance;
    }
}