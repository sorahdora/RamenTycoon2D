using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotDropZone : MonoBehaviour, IDropHandler
{
    public GameManager gameManager;

    public AudioSource soundEffects;
    public AudioClip eggCrackSound;
    public AudioClip knifeSound;
    public AudioClip waterBoilingSound;
    public AudioClip serveBellSound;
    public AudioClip coinEarnSound;
    public AudioClip errorSound;

    public GameObject waterLayer;
    public GameObject powderLayer;
    public GameObject noodleLayer;
    public GameObject eggLayer;
    public GameObject greenOnionLayer;
    public GameObject readyText;

    public float readyDelay = 3f;
    public float burnDelay = 5f;

    private bool hasWater;
    private bool hasPowder;
    private bool hasNoodle;
    private bool hasEgg;
    private bool hasGreenOnion;

    private bool isCookingFinalStep;
    private bool isReady;
    private bool isBurned;

    private float readyTimer;
    private float burnTimer;

    private Image potImage;

    private DraggablePot draggablePot;

    void Awake()
    {
        potImage = GetComponent<Image>();
        draggablePot = GetComponent<DraggablePot>();
    }

    void Update()
    {
        if (isBurned)
        {
            return;
        }

        
        if (draggablePot == null || !draggablePot.IsOnBurner)
        {
            return;
        }

        if (isCookingFinalStep)
        {
            readyTimer += Time.deltaTime;

            if (readyTimer >= readyDelay)
            {
                isCookingFinalStep = false;
                isReady = true;
                burnTimer = 0f;

                readyText.SetActive(true);

                if (soundEffects != null && serveBellSound != null)
                {
                    soundEffects.PlayOneShot(serveBellSound);
                }

                Debug.Log(gameObject.name + " is READY!");
            }
        }

        if (isReady)
        {
            burnTimer += Time.deltaTime;

            if (burnTimer >= burnDelay)
            {
                BurnRamen();
            }
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (isBurned || isReady || isCookingFinalStep)
        {
            return;
        }

        if (draggablePot == null || !draggablePot.IsOnBurner)
        {
            Debug.Log("Place the pot on a burner first!");
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
            PlayErrorSound();
            return;
        }

        hasWater = true;
        waterLayer.SetActive(true);

        if (soundEffects != null && waterBoilingSound != null)
        {
            soundEffects.PlayOneShot(waterBoilingSound);
        }
    }

    private void AddPowder()
    {
        if (!hasWater)
        {
            Debug.Log("Add water first!");
            PlayErrorSound();
            return;
        }

        if (hasPowder)
        {
            Debug.Log("This pot already has powder.");
            PlayErrorSound();
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
            PlayErrorSound();
            return;
        }

        if (hasNoodle)
        {
            Debug.Log("This pot already has noodles.");
            PlayErrorSound();
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
            PlayErrorSound();
            return;
        }

        if (hasEgg)
        {
            Debug.Log("This pot already has an egg.");
            PlayErrorSound();
            return;
        }

        hasEgg = true;
        eggLayer.SetActive(true);

        if (soundEffects != null && eggCrackSound != null)
        {
            soundEffects.PlayOneShot(eggCrackSound);
        }
    }

    private void AddGreenOnion()
    {
        if (!hasEgg)
        {
            Debug.Log("Add an egg first!");
            PlayErrorSound();
            return;
        }

        if (hasGreenOnion)
        {
            Debug.Log("This pot already has green onion.");
            PlayErrorSound();
            return;
        }

        hasGreenOnion = true;
        greenOnionLayer.SetActive(true);

        if (soundEffects != null && knifeSound != null)
        {
            soundEffects.PlayOneShot(knifeSound);
        }

        isCookingFinalStep = true;
        readyTimer = 0f;
    }

    public void TryServeRamen()
    {
        if (isBurned)
        {
            Debug.Log("Burned ramen cannot be served.");
            return;
        }

        if (!isReady)
        {
            Debug.Log("The ramen is not ready yet!");
            return;
        }

        isReady = false;
        isCookingFinalStep = false;

        readyText.SetActive(false);

        if (gameManager != null)
        {
            gameManager.AddMoney(1);
        }

        if (soundEffects != null && coinEarnSound != null)
        {
            soundEffects.PlayOneShot(coinEarnSound);
        }

        Debug.Log(gameObject.name + " was served! +$1");

        ResetPot();
    }

    private void BurnRamen()
    {
        isBurned = true;
        isReady = false;
        isCookingFinalStep = false;

        readyText.SetActive(false);

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

        isCookingFinalStep = false;
        isReady = false;
        isBurned = false;

        readyTimer = 0f;
        burnTimer = 0f;

        waterLayer.SetActive(false);
        powderLayer.SetActive(false);
        noodleLayer.SetActive(false);
        eggLayer.SetActive(false);
        greenOnionLayer.SetActive(false);
        readyText.SetActive(false);

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
    private void PlayErrorSound()
    {
        if (soundEffects != null && errorSound != null)
        {
            soundEffects.PlayOneShot(errorSound);
        }
    }
}