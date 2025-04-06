using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BaseWindow : MonoBehaviour
{
    protected CanvasGroup CanvasGroup;

    public virtual void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        if (CanvasGroup == null)
        {
            CanvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        CanvasGroup.alpha = 1f;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }

    public virtual void Hide()
    {
        CanvasGroup.alpha = 0f;
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
    }
}