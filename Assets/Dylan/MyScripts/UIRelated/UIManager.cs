using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : DefaultUIManager
{
    [Header("SHOP WINDOW")]
    [SerializeField] private CanvasGroup shopWindow;
    [SerializeField] private CanvasGroup elementsToDisplayOnShopClosure;
    [SerializeField] private GameObject shopButtonsGroup;
    public bool shopWindowIsDisplayed = true;

    [Header("PLAYER POINTS TEXT")]
    [SerializeField] private GameObject playerPoints;
    [SerializeField] private TextMeshProUGUI playerPointsValueText;
    [SerializeField] private int valueToSubstractPerTicks;

    [Header("PURCHASE VALIDATION POPUP PARAMETERS")]
    public CanvasGroup purchaseValidationPopupWindow;
    public GameObject purchaseValidationPopupButtonLayout;
    //[HideInInspector]
    public bool purchaseValidationPopupIsDisplayed = false;

    [Header("BEGIN EXFILTRATION VALIDATION POPUP PARAMETERS")]
    public CanvasGroup beginExfiltrationValidationPopupWindow;
    public GameObject beginExfiltrationValidationPopupButtonLayout;
    //[HideInInspector]
    public bool beginExfiltrationValidationPopupIsDisplayed = false;

    [Header("ACTION IS IMPOSSIBLE FEEDBACK")]
    public CanvasGroup cantUseAnObjectFeedback;
    public CanvasGroup cantPickAnObjectFeedback;
    public CanvasGroup cantUseASpellFeedback;
    public CanvasGroup inventoryIsFullFeedBack;
    [SerializeField] private float timeBetweenFades = 0.25f;

    [Header("INPUT LANDMARK OBJECT COMPARTMENT 00")]
    public TextMeshProUGUI objectCompartmentInputActionText;
    public Image objectCompartmentInputIcon;
    public Sprite[] objectCompartmentInputSprites;
    public string objectCompartmentIsActive;
    public string objectCompartmentIsNotActive;

    [Header("INPUT LANDMARK SPELL COMPARTMENT 00")]
    public TextMeshProUGUI spellCompartmentInputActionText;
    public Image spellCompartmentInputIcon;
    public Sprite[] spellCompartmentInputSprites;
    public string spellCompartmentIsActive;
    public string spellCompartmentIsNotActive;

    [Header("DUPPLICATION PARAMETERS")]
    public CanvasGroup duplicationWindow;
    public GameObject duplicationButtonLayout;
    public bool duplicationValidationPopupIsDisplayed = false;

    public static UIManager s_Singleton;

    #region Singleton
    private void Awake()
    {
        if (s_Singleton != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            s_Singleton = this;
        }

        if (elementsToDisplayOnShopClosure.alpha == 1)
            elementsToDisplayOnShopClosure.alpha = 0;
    }
    #endregion

    private void Start()
    {
        SetOSTStateAndPostEvent("state_Exfiltration");

        //Initialisation des points du joueur au start
        SetPlayerPointsCountValue();
        DisplayShopWindow();
    }

    public override void Update()
    {
        base.Update();

        if(GameManager.s_Singleton.gameState == GameState.ConsultingShop)
        {
            #region Circle/B - Purchase Validation Popup
            if (purchaseValidationPopupIsDisplayed && !beginExfiltrationValidationPopupIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
            {
                Debug.Log("Circle/B pressed");
                CancelPurchase();
            }
            #endregion

            #region Circle/B 2 - Begin Exfiltration Validation Popup
            if (beginExfiltrationValidationPopupIsDisplayed &&  (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
            {
                Debug.Log("Circle/B pressed");
                HideBeginExfiltrationPopup();
            }
            #endregion

            #region Square/X
            if (!ShopManager.isBuying && !beginExfiltrationValidationPopupIsDisplayed && !purchaseValidationPopupIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X")))
            {
                Debug.Log("Square/X pressed");
                DisplayBeginExfiltrationPopup();
            } 
            #endregion
        }

        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            #region Circle/B 3 - Duplication Validation Popup
            if (duplicationValidationPopupIsDisplayed && !pauseWindowHasBeenClosed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
            {
                Debug.Log("Circle/B pressed");
                HideDuplicationPopup();
                Player.s_Singleton.isUsingASpell = false;
                PlayerSpellsInventory.s_Singleton.DeactivateSpellActivationFeedback();
            }
            #endregion
        }
    }

    #region Display/Hide Shop Window
    void DisplayShopWindow()
    {
        GameManager.s_Singleton.gameState = GameState.ConsultingShop;

        if (!shopWindow.transform.gameObject.activeInHierarchy)
            shopWindow.transform.gameObject.SetActive(true);

        StartCoroutine(FadeCanvasGroup(shopWindow, shopWindow.alpha, 1, fadeDuration));
        shopWindowIsDisplayed = true;
        EnableButtonsInLayout(shopButtonsGroup, null);

        playerPoints.SetActive(true);
    }

    public void HideShopWindow()
    {
        StartCoroutine(FadeCanvasGroup(shopWindow, shopWindow.alpha, 0, fadeDuration));
        shopWindow.transform.gameObject.SetActive(false);
        shopWindowIsDisplayed = false;
        DisableButtonsInLayout(shopButtonsGroup);

        playerPoints.SetActive(false);
        StartCoroutine(FadeCanvasGroup(elementsToDisplayOnShopClosure, elementsToDisplayOnShopClosure.alpha, 1, fadeDuration));

        GameManager.s_Singleton.gameState = GameState.PlayMode;
        GameManager.s_Singleton.exfiltrationHasBegun = true;
    }
    #endregion

    #region Display/Hide Begin Exfiltration Popup
    void DisplayBeginExfiltrationPopup()
    {
        DisplayAPopup(beginExfiltrationValidationPopupWindow);
        beginExfiltrationValidationPopupIsDisplayed = true;

        EnableButtonsInLayout(beginExfiltrationValidationPopupButtonLayout, beginExfiltrationValidationPopupButtonLayout.transform.GetChild(1).gameObject);
    }

    public void HideBeginExfiltrationPopup()
    {
        HideAPopup(beginExfiltrationValidationPopupWindow);
        beginExfiltrationValidationPopupIsDisplayed = false;

        DisableButtonsInLayout(beginExfiltrationValidationPopupButtonLayout);

        lastSelectedButton.GetComponent<ButtonSoundEffects>().isSelected = true;
        ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
    }
    #endregion

    #region Display/Hide Duplication Popup
    public void DisplayDuplicationPopup()
    {
        DisplayAPopup(duplicationWindow);
        duplicationValidationPopupIsDisplayed = true;

        //Activation des boutons et attribution du bouton sélectionné par l'Event system
        for (int i = 0; i < duplicationButtonLayout.transform.childCount; i++)
        {
            duplicationButtonLayout.transform.GetChild(i).gameObject.SetActive(true);
        }
        
        EnableButtonsInLayout(duplicationButtonLayout, duplicationButtonLayout.transform.GetChild(0).gameObject);
    }

    public void HideDuplicationPopup()
    {
        HideAPopup(duplicationWindow);
        duplicationValidationPopupIsDisplayed = false;
        DisableButtonsInLayout(duplicationButtonLayout);
    }
    #endregion

    public IEnumerator FadeInAndOutObjectFeedBack(CanvasGroup _canvas)
    {
        if (GameManager.s_Singleton.gameState != GameState.PlayMode)
            yield return new WaitUntil(() => GameManager.s_Singleton.gameState == GameState.PlayMode);

        StartCoroutine(FadeCanvasGroup(_canvas, _canvas.alpha, 1, fadeDuration));

        yield return new WaitForSeconds(timeBetweenFades);

        if (GameManager.s_Singleton.gameState != GameState.PlayMode)
            yield return new WaitUntil(() => GameManager.s_Singleton.gameState == GameState.PlayMode);

        StartCoroutine(FadeCanvasGroup(_canvas, _canvas.alpha, 0, fadeDuration));
    }

    #region Player Score Points
    //Summary : Permet d'attribuer un nombre de points au joueur. Dès que le joueur reçoit de nouveaux points, on vérifie si de nouveaux sort sont maintenant achetables.
    public void AddPointsToPlayerScore(int valueToAdd)
    {
        GameManager.s_Singleton.playerPointsValue += valueToAdd;
        SetPlayerPointsCountValue();

        foreach (Transform obj in ShopManager.s_Singleton.spellsAvailableInShop)
        {
            obj.GetComponent<ShopButton>().CheckIfPlayerCanPurchaseASpell(GameManager.s_Singleton.playerPointsValue);
        }
    }

    //Summary : Permet de set up la valeur du sort qui est en train d'être acheté + Opération de soustraction de celle-ci.
    public void SetValueToSubstract(int valueToSubstract)
    {
        int tempPlayerPointsValue = GameManager.s_Singleton.playerPointsValue;
        tempPlayerPointsValue -= valueToSubstract;

        foreach (Transform obj in ShopManager.s_Singleton.spellsAvailableInShop)
        {
            obj.GetComponent<ShopButton>().CheckIfPlayerCanPurchaseASpell(tempPlayerPointsValue);

            if (ShopManager.s_Singleton.amntOfSpellBought == 3)
            {
                obj.GetComponent<ShopButton>().SetButtonColor(obj.GetComponent<ShopButton>().inventoryIsFullColor);
                obj.transform.GetChild(2).gameObject.SetActive(true);
            }
            else if (ShopManager.s_Singleton.amntOfSpellBought < 3)
            {
                obj.transform.GetChild(2).gameObject.SetActive(false);
            }
        }

        StartCoroutine(SubstractionCoroutine(valueToSubstract, valueToSubstractPerTicks));
        SetPlayerPointsCountValue();
    }

    //Summary : Permet de mettre à jour la valeur des points possédés par le joueur.
    void SetPlayerPointsCountValue()
    {
        playerPointsValueText.text = GameManager.s_Singleton.playerPointsValue.ToString();
    }

    public void HideValidationPopupWindow()
    {
        HideAPopup(purchaseValidationPopupWindow);
        DisableButtonsInLayout(purchaseValidationPopupButtonLayout);
        purchaseValidationPopupIsDisplayed = false;
        ResetEventSystemFirstSelectedGameObjet(purchaseValidationPopupButtonLayout.transform.GetChild(0).GetComponent<PurchaseASpell>().selectedButton.gameObject);
    }

   public void CancelPurchase()
    {
        HideValidationPopupWindow();
    }

    //Summary : Utiliser pour faire défiler les points lorsque le joueur achète quelque chose (effet de décrémentation dynamique) jusqu'à l'atteinte de la valeur totale à soustraite.
    IEnumerator SubstractionCoroutine(int substractValueToReach, int valueToSubtractPerTicks)
    {
        int startValue = 0;
        do
        {
            startValue += valueToSubtractPerTicks;
            GameManager.s_Singleton.playerPointsValue -= valueToSubtractPerTicks;
            SetPlayerPointsCountValue();
            yield return new WaitForEndOfFrame();
            if (GameManager.s_Singleton.gameState == GameState.Pause)
            {
                yield return new WaitUntil(() => GameManager.s_Singleton.gameState == GameState.PlayMode);
            }

        } while (startValue != substractValueToReach);
    }
    #endregion

    
}
