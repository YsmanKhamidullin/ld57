using Cysharp.Threading.Tasks;
using Game.Core.VisualNovel;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private DialogueSequence _dialogue;

    private bool _isInteracted;

    public async UniTask Interact()
    {
        if (_isInteracted)
        {
            return;
        }

        _isInteracted = true;
        Root.Instance.ServiceUi.HideGamePlay();
        await Root.Instance.DialogueSequenceWrapper.StartSequence(_dialogue, this);
        await UniTask.WaitForSeconds(0.15f);
        Root.Instance.ServiceUi.ShowGamePlay();
    }

    public void HideNpc()
    {
        gameObject.SetActive(false);
    }
}