using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotDropZone : MonoBehaviour, IDropHandler
{
    public GameObject waterLayer;
    public GameObject powderLayer;
    public GameObject noodleLayer;
    public GameObject eggLayer;
    public GameObject greenOnionLayer;

    public float burnDelay = 5f;

    private bool hasWater;
    private bool hasPowder;
    private bool hasNoodle;
    private bool hasEgg;
    private bool hasGreenOnion;

    private bool isComplete;
    private bool isBurned;
    private float completeTimer;

    private Image potImage;


    void Awake()
    {
        potImage = GetComponent<Image>();
    }

    void Update()
    {
        if (!isComplete || isBurned)
        {
            return;
        }

        completeTimer += Time.deltaTime;

        if (completeTimer >= burnDelay)
        {
            BurnRamen();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isBurned)
        {
            return;
        }

        DraggableIngredient ingredient =
            eventData.pointerDrag?.GetComponent<DraggableIngredient>();

        if (ingredient == null)
        {
            return;
        }

        switch (ingredient.ingredientType)
        {
            case IngredientType.Water:
                AddWater();
                break;

            case IngredientType.Powder:
                AddPowder();
                break;

            case IngredientType.Noodle:
                AddNoodle();
                break;

            case IngredientType.Egg:
                AddEgg();
                break;

            case IngredientType.GreenOnion:
                AddGreenOnion();
                break;
        }
    }

    private void AddWater()
    {
        if (hasWater)
        {
            Debug.Log("This pot already has water.");
            return;
        }

        hasWater = true;
        waterLayer.SetActive(true);
    }

    private void AddPowder()
    {
        if (!hasWater)
        {
            Debug.Log("Add water first!");
            return;
        }

        if (hasPowder)
        {
            Debug.Log("This pot already has powder.");
            return;
        }

        hasPowder = true;

        waterLayer.SetActive(false);
        powderLayer.SetActive(true);
    }

    private void AddNoodle()
    {
        if (!hasPowder)
        {
            Debug.Log("Add powder first!");
            return;
        }

        if (hasNoodle)
        {
            Debug.Log("This pot already has noodles.");
            return;
        }

        hasNoodle = true;

        powderLayer.SetActive(false);
        noodleLayer.SetActive(true);
    }

    private void AddEgg()
    {
        if (!hasNoodle)
        {
            Debug.Log("Add noodles first!");
            return;
        }

        if (hasEgg)
        {
            Debug.Log("This pot already has an egg.");
            return;
        }

        hasEgg = true;
        eggLayer.SetActive(true);
    }

    private void AddGreenOnion()
    {
        if (!hasEgg)
        {
            Debug.Log("Add egg first!");
            return;
        }

        if (hasGreenOnion)
        {
            Debug.Log("This pot already has green onion.");
            return;
        }

        hasGreenOnion = true;
        greenOnionLayer.SetActive(true);

        isComplete = true;
        completeTimer = 0f;

        Debug.Log(gameObject.name + " is READY!");
    }

    private void BurnRamen()
    {
        isBurned = true;

        potImage.color = new Color(0.25f, 0.25f, 0.25f, 1f);

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        Debug.Log(gameObject.name + " is BURNED!");
        Invoke(nameof(ResetPot), 2f);
    }
    private void ResetPot()
    {
        hasWater = false;
        hasPowder = false;
        hasNoodle = false;
        hasEgg = false;
        hasGreenOnion = false;

        isComplete = false;
        isBurned = false;
        completeTimer = 0f;

        waterLayer.SetActive(false);
        powderLayer.SetActive(false);
        noodleLayer.SetActive(false);
        eggLayer.SetActive(false);
        greenOnionLayer.SetActive(false);

        potImage.color = Color.white;

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }

        Debug.Log(gameObject.name + " has been reset.");
    }
}