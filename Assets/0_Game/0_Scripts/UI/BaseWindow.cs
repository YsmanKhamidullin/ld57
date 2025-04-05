using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BaseWindow : MonoBehaviour
{
    protected CanvasGroup CanvasGroup;
    public virtual void Start()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        if (CanvasGroup == null)
        {
            CanvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void Show()
    {
        CanvasGroup.alpha = 1f;
        CanvasGroup.interactable = true;
    }

    public void Hide()
    {
        CanvasGroup.alpha = 0f;
        CanvasGroup.interactable = false;
    }
}