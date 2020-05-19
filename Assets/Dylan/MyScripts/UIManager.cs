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
    public bool shopWindowIsDisplayed = true;

    [Header("PLAYER POINTS TEXT")]
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
    [SerializeField] private float timeBetweenFades = 0.25f;

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
    }
    #endregion

    private void Start()
    {
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
                HideAPopup(purchaseValidationPopupWindow);
                DisableButtonsInLayout(purchaseValidationPopupButtonLayout);
                purchaseValidationPopupIsDisplayed = false;
            }
            #endregion

            #region Circle/B 2 - Begin Exfiltration Validation Popup
            if (beginExfiltrationValidationPopupIsDisplayed &&  (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
            {
                Debug.Log("Circle/B pressed");
                HideAPopup(beginExfiltrationValidationPopupWindow);
                DisableButtonsInLayout(beginExfiltrationValidationPopupButtonLayout);
                beginExfiltrationValidationPopupIsDisplayed = false;
            }
            #endregion

            #region Square/X
            if (!beginExfiltrationValidationPopupIsDisplayed && !purchaseValidationPopupIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X")))
            {
                Debug.Log("Square/X pressed");
                DisplayAPopup(beginExfiltrationValidationPopupWindow);
                EnableButtonsInLayout(beginExfiltrationValidationPopupButtonLayout);
                beginExfiltrationValidationPopupIsDisplayed = true;
            }
            #endregion
        }
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            #region Circle/B 3 - Duplication Validation Popup
            if (duplicationValidationPopupIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
            {
                Debug.Log("Circle/B pressed");
                HideAPopup(duplicationWindow);
                //DisableButtonsInLayout(duplicationButtonLayout);
                duplicationValidationPopupIsDisplayed = false;
                PlayerSpellsInventory.s_Singleton.DeactivateSpellActivationFeedback();
            }
            #endregion
        }
    }

    #region Display/Hide Shop WIndow
    void DisplayShopWindow()
    {
        if (!shopWindow.transform.gameObject.activeInHierarchy)
            shopWindow.transform.gameObject.SetActive(true);

        playerPointsValueText.gameObject.SetActive(true);
        StartCoroutine(FadeCanvasGroup(shopWindow, shopWindow.alpha, 1, fadeDuration));
        GameManager.s_Singleton.gameState = GameState.ConsultingShop;
        shopWindowIsDisplayed = true;
    }

    public void HideShopWindow()
    {
        StartCoroutine(FadeCanvasGroup(shopWindow, shopWindow.alpha, 0, fadeDuration));
        GameManager.s_Singleton.gameState = GameState.PlayMode;
        shopWindowIsDisplayed = false;
        shopWindow.transform.gameObject.SetActive(false);
        playerPointsValueText.gameObject.SetActive(false);
    }
    #endregion

    #region Popup Bool State
    public void SetBeginExfiltrationPupopState(bool state)
    {
        beginExfiltrationValidationPopupIsDisplayed = state;
    }

    public void SetDuplicationPupopState(bool state)
    {
        duplicationValidationPopupIsDisplayed = state;
    }

    public void SetPurchasePupopState(bool state)
    {
        purchaseValidationPopupIsDisplayed = state;
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

        foreach (GameObject obj in ShopManager.s_Singleton.spellsAvailableInShop)
        {
            obj.GetComponent<ShopButton>().CheckIfPlayerCanPurchaseASpell(GameManager.s_Singleton.playerPointsValue);
        }
    }

    //Summary : Permet de set up la valeur du sort qui est en train d'être acheté + Opération de soustraction de celle-ci.
    public void SetValueToSubstract(int valueToSubstract)
    {
        int tempPlayerPointsValue = GameManager.s_Singleton.playerPointsValue;
        tempPlayerPointsValue -= valueToSubstract;

        foreach (GameObject obj in ShopManager.s_Singleton.spellsAvailableInShop)
        {
            obj.GetComponent<ShopButton>().CheckIfPlayerCanPurchaseASpell(tempPlayerPointsValue);
        }

        StartCoroutine(SubstractionCoroutine(valueToSubstract, valueToSubstractPerTicks));
        SetPlayerPointsCountValue();
    }

    //Summary : Permet de mettre à jour la valeur des points possédés par le joueur.
    void SetPlayerPointsCountValue()
    {
        playerPointsValueText.text = GameManager.s_Singleton.playerPointsValue.ToString();
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
