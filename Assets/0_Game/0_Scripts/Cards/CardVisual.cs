using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    private bool initalize = false;

    [Header("Card")]
    public Card parentCard;

    private Transform cardTransform;
    private Vector3 rotationDelta;
    private int savedIndex;
    Vector3 movementDelta;
    private Canvas canvas;

    [Header("References")]
    public Transform visualShadow;

    [SerializeField]
    private Transform shakeParent;

    [SerializeField]
    private Transform tiltParent;

    [Header("Follow Parameters")]
    [SerializeField]
    private float followSpeed = 30;

    [Header("Rotation Parameters")]
    [SerializeField]
    private float rotationAmount = 20;

    [SerializeField]
    private float rotationSpeed = 20;

    [SerializeField]
    private float autoTiltAmount = 30;

    [SerializeField]
    private float manualTiltAmount = 20;

    [SerializeField]
    private float tiltSpeed = 20;

    [Header("Scale Parameters")]
    [SerializeField]
    private bool scaleAnimations = true;

    [SerializeField]
    private float scaleOnHover = 1.15f;

    [SerializeField]
    private float scaleOnSelect = 1.25f;

    [SerializeField]
    private float scaleTransition = .15f;

    [SerializeField]
    private Ease scaleEase = Ease.OutBack;

    [Header("Select Parameters")]
    [SerializeField]
    private float selectPunchAmount = 20;

    [Header("Hober Parameters")]
    [SerializeField]
    private float hoverPunchAngle = 5;

    [SerializeField]
    private float hoverTransition = .15f;

    [Header("Swap Parameters")]
    [SerializeField]
    private bool swapAnimations = true;

    [SerializeField]
    private float swapRotationAngle = 30;

    [SerializeField]
    private float swapTransition = .15f;

    [SerializeField]
    private int swapVibrato = 5;

    [Header("Curve")]
    [SerializeField]
    private CurveParameters curve;

    private float curveYOffset;
    private float curveRotationOffset;

    public void Initialize(Card target)
    {
        //Declarations
        parentCard = target;
        cardTransform = target.transform;
        canvas = GetComponent<Canvas>();

        //Event Listening
        parentCard.PointerEnterEvent.AddListener(PointerEnter);
        parentCard.PointerExitEvent.AddListener(PointerExit);
        parentCard.BeginDragEvent.AddListener(BeginDrag);
        parentCard.EndDragEvent.AddListener(EndDrag);
        parentCard.PointerDownEvent.AddListener(PointerDown);
        parentCard.PointerUpEvent.AddListener(PointerUp);

        //Initialization
        initalize = true;
    }

    void Update()
    {
        if (!initalize || parentCard == null) return;

        SmoothFollow();
        FollowRotation();
        CardTilt();
        visualShadow.gameObject.SetActive(parentCard._isHovering || parentCard._isDragging);
    }

    private void SmoothFollow()
    {
        Vector3 verticalOffset = (Vector3.up * (parentCard._isDragging ? 0 : curveYOffset));
        transform.position = Vector3.Lerp(transform.position, cardTransform.position + verticalOffset,
            followSpeed * Time.deltaTime);
    }

    private void FollowRotation()
    {
        Vector3 movement = (transform.position - cardTransform.position);
        movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
        Vector3 movementRotation = (parentCard._isDragging ? movementDelta : movement) * rotationAmount;
        rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
            Mathf.Clamp(rotationDelta.x, -60, 60));
    }

    private void CardTilt()
    {
        savedIndex = parentCard._isDragging ? savedIndex : parentCard.ParentIndex();
        float sine = Mathf.Sin(Time.time + savedIndex) * (parentCard._isHovering ? .2f : 1);
        float cosine = Mathf.Cos(Time.time + savedIndex) * (parentCard._isHovering ? .2f : 1);

        Vector3 offset = transform.position - Input.mousePosition;
        offset.z = 0f;
        float tiltX = parentCard._isHovering ? ((offset.y * -1) * manualTiltAmount) : 0;
        float tiltY = parentCard._isHovering ? ((offset.x) * manualTiltAmount) : 0;
        float tiltZ = parentCard._isDragging
            ? tiltParent.eulerAngles.z
            : (curveRotationOffset * (curve.rotationInfluence * parentCard.SiblingAmount()));

        float lerpX = Mathf.LerpAngle(tiltParent.eulerAngles.x, tiltX + (sine * autoTiltAmount),
            tiltSpeed * Time.deltaTime);
        float lerpY = Mathf.LerpAngle(tiltParent.eulerAngles.y, tiltY + (cosine * autoTiltAmount),
            tiltSpeed * Time.deltaTime);
        float lerpZ = Mathf.LerpAngle(tiltParent.eulerAngles.z, tiltZ, tiltSpeed / 2 * Time.deltaTime);

        tiltParent.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
    }

    private void BeginDrag(Card card)
    {
        if (scaleAnimations)
            transform.DOScale(scaleOnSelect, scaleTransition).SetEase(scaleEase);

        canvas.overrideSorting = true;
    }

    private void EndDrag(Card card)
    {
        canvas.overrideSorting = false;
        transform.DOScale(1, scaleTransition).SetEase(scaleEase);
    }

    private void PointerEnter(Card card)
    {
        if (scaleAnimations)
            transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);

        DOTween.Kill(2, true);
        shakeParent.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 1).SetId(2);
    }

    private void PointerExit(Card card)
    {
        if (!parentCard.wasDragged)
            transform.DOScale(1, scaleTransition).SetEase(scaleEase);
    }

    private void PointerUp(Card card, bool longPress)
    {
        canvas.overrideSorting = false;
    }

    private void PointerDown(Card card)
    {
    }
}