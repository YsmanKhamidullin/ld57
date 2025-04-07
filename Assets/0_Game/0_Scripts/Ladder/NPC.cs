using Cysharp.Threading.Tasks;
using Game.Core.VisualNovel;
using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
    public UnityEvent OnInteractComplete;
    
    [SerializeField]
    private DialogueSequence _dialogue;
    
    [SerializeField]
    private bool _isTrueEnd;

    private bool _isInteracted;

    public async UniTask Interact()
    {
        if (_isInteracted)
        {
            return;
        }

        _isInteracted = true;
        Root.Instance.PlayerWill.Hide();
        Root.Instance.ServiceUi.HideGamePlay();
        await Root.Instance.DialogueSequenceWrapper.StartSequence(_dialogue, this);
        await UniTask.WaitForSeconds(0.15f);
        if (_isTrueEnd)
        {
            Root.Instance.ServiceUi.ShowTrueEnding();
            return;
        }
        Root.Instance.ServiceUi.ShowGamePlay();
        Root.Instance.PlayerWill.Show();
        OnInteractComplete?.Invoke();
    }

    public void HideNpc()
    {
        gameObject.SetActive(false);
    }
}