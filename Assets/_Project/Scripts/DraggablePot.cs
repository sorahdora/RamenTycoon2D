using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePot : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public RectTransform leftBurnerSlot;
    public RectTransform rightBurnerSlot;

    public GameObject leftFlame;
    public GameObject rightFlame;

    public float snapDistance = 180f;

    
    public bool IsOnBurner { get; private set; }

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector2 originalPosition;

    private static DraggablePot leftPot;
    private static DraggablePot rightPot;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        originalPosition = rectTransform.anchoredPosition;

        IsOnBurner = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform.SetAsLastSibling();

        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;

        
        IsOnBurner = false;

        if (leftPot == this)
        {
            leftPot = null;
            leftFlame.SetActive(false);
        }

        if (rightPot == this)
        {
            rightPot = null;
            rightFlame.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        float leftDistance = Vector2.Distance(
            rectTransform.anchoredPosition,
            leftBurnerSlot.anchoredPosition
        );

        float rightDistance = Vector2.Distance(
            rectTransform.anchoredPosition,
            rightBurnerSlot.anchoredPosition
        );

        if (leftDistance <= snapDistance && leftPot == null)
        {
            rectTransform.anchoredPosition =
                leftBurnerSlot.anchoredPosition;

            leftPot = this;
            leftFlame.SetActive(true);

            IsOnBurner = true;
        }
        else if (rightDistance <= snapDistance && rightPot == null)
        {
            rectTransform.anchoredPosition =
                rightBurnerSlot.anchoredPosition;

            rightPot = this;
            rightFlame.SetActive(true);

            IsOnBurner = true;
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;

            IsOnBurner = false;
        }
    }
}