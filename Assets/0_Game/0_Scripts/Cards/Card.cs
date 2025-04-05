using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum CardTypes
{
    None,
    Forward,
    Backward,
    Dream,
    Struggle,
    Talk,
    Listen,
    Flee,
}

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler,
    IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    public CardTypes CardType;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Image imageComponent;

    [SerializeField]
    private CardVisual _visual;

    public Vector3 _offset;
    public bool _isDragging;
    public bool _isDragBlocked;
    public bool _isHovering;
    public bool wasDragged;

    [Header("Events")]
    [HideInInspector]
    public UnityEvent<Card> PointerEnterEvent;

    [HideInInspector]
    public UnityEvent<Card> PointerExitEvent;

    [HideInInspector]
    public UnityEvent<Card, bool> PointerUpEvent;

    [HideInInspector]
    public UnityEvent<Card> PointerDownEvent;

    [HideInInspector]
    public UnityEvent<Card> BeginDragEvent;

    [HideInInspector]
    public UnityEvent<Card> EndDragEvent;

    public float selectionOffset = 50;
    private float pointerDownTime;
    private float pointerUpTime;
    private Vector2 _defaultPos;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        imageComponent = GetComponent<Image>();
        _visual.Initialize(this);
        _defaultPos = transform.position;
    }

    void Update()
    {
        if (_isDragging)
        {
            Vector2 targetPosition = Input.mousePosition - _offset;
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            var distance = Vector2.Distance(transform.position, targetPosition);
            var moveSpeed = Mathf.Min(4500, distance / Time.deltaTime);
            Vector2 velocity = direction * moveSpeed;
            transform.Translate(velocity * Time.deltaTime);
        }

        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -Screen.width, Screen.width);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -Screen.height, Screen.height);
        transform.position = clampedPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CheckAvailableInput(eventData) == false)
        {
            return;
        }

        Vector2 mousePosition = Input.mousePosition;
        _offset = mousePosition - (Vector2)transform.position;
        _isDragging = true;
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        imageComponent.raycastTarget = false;

        wasDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        // EndDragEvent.Invoke(this);
        _isDragging = false;
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
        imageComponent.raycastTarget = true;

        StartCoroutine(FrameWait());

        IEnumerator FrameWait()
        {
            yield return new WaitForEndOfFrame();
            wasDragged = false;
        }
    }

    private bool CheckAvailableInput(PointerEventData eventData)
    {
        var notUsingCard = Root.Instance.ServiceCards.IsUsingCard == false;
        return eventData.button == PointerEventData.InputButton.Left && notUsingCard;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        _isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent.Invoke(this);
        _isHovering = false;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (CheckAvailableInput(eventData) == false)
        {
            return;
        }

        PointerDownEvent.Invoke(this);
        pointerDownTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (CheckAvailableInput(eventData) == false)
        {
            return;
        }

        pointerUpTime = Time.time;
        PointerUpEvent.Invoke(this, pointerUpTime - pointerDownTime > .2f);
        if (wasDragged)
        {
            _visual.transform.localScale = Vector3.one;
            Root.Instance.ServiceCards.TryUseCard(this);
        }

        if (pointerUpTime - pointerDownTime > .2f)
            return;
    }

    public void ToDefaultPos()
    {
        transform.DOMove(_defaultPos, 0.15f).SetEase(Ease.OutQuad);
    }

    public void ReAppear(Action onHide)
    {
        var hide = transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InQuad);
        var show = transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutQuad);
        DOTween.Sequence()
            .Append(hide)
            .AppendCallback(() =>
            {
                onHide?.Invoke();
                transform.position = _defaultPos;
            })
            .Append(show);
    }

    public int SiblingAmount()
    {
        return 3;
    }

    public int ParentIndex()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
    }

    public float NormalizedPosition()
    {
        return transform.parent.CompareTag("Slot")
            ? E.Remap((float)ParentIndex(), 0, (float)(transform.parent.parent.childCount - 1), 0, 1)
            : 0;
    }

    private void OnDestroy()
    {
        if (_visual != null)
            Destroy(_visual.gameObject);
    }
}