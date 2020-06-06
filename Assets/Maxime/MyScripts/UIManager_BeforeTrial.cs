using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager_BeforeTrial : DefaultUIManager
{
    [Header("FADE OUT PARTAMETERS")]
    [SerializeField] private TextMeshProUGUI dayNumber;
    [SerializeField] private TextMeshProUGUI trialSubject;
    public bool inspectionHasBegun = false;

    public TextMeshProUGUI titreProcesTxt;
    //public int numeroProces;
    public string chefAccusation;

    public TextMeshProUGUI docActuelTxt;

    public GameObject changeDocumentTypeLandmark;
    [SerializeField] private GameObject firstDayIndications;

    [Header("VALIDATION POPUP PARAMETERS")]
    [SerializeField] private CanvasGroup validationPopupWindow;
    [SerializeField] private GameObject validationPopupButtonLayout;
    [HideInInspector]
    public bool validationPopupIsDisplayed = false;

    #region Singleton
    public static UIManager_BeforeTrial singleton;

    private void Awake()
    {
        if (singleton)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
        }

        titreProcesTxt.text = chefAccusation;
        SetFadeOutTextsIndicator();
        LevelChanger.s_Singleton.FadeOutOnLevelLoaded("FadeOut");
    }
    #endregion

    void Start()
    {
        if (GameManager.s_Singleton.trialDayNumber == 1)
            firstDayIndications.SetActive(true);

        SetOSTStateAndPostEvent("state_BeforeTrial");
    }

    public override void Update()
    {
        base.Update();

        if (firstDayIndications.activeInHierarchy && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_X") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_A")))
        {
            Debug.Log("Croix ou A presed");
            firstDayIndications.SetActive(false);
        }

        if (inspectionHasBegun && !firstDayIndications.activeInHierarchy && !validationPopupIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X")))
        {
            Debug.Log("Square pressed");
            DisplayValidationPopup();
        }

        if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B"))
        {
            if (validationPopupIsDisplayed)
                HideValidationPopup();
        }
    }

    public void UIUpdateActualDoc(int actualDoc, int maxDoc)
    {
        docActuelTxt.text = "Preuve " + actualDoc + " sur " + maxDoc;
    }

    #region Validation Popup
    //Summary : Affiche la fenêtre de confirmation d'achat lorsque le joueur appuie sur un bouton contenant un sort qu'il peut acheter.
    public void DisplayValidationPopup()
    {
        //Affichage de la fenêtre de confirmation d'achat en Fade-In
        StartCoroutine(FadeCanvasGroup(validationPopupWindow, validationPopupWindow.alpha, 1, fadeDuration));
        validationPopupWindow.blocksRaycasts = true;

        //Réactivation des boutons contenus dans cette fenêtre (prévient les problèmes liés à la navigation de l'Event System)
        foreach (Button _buttons in validationPopupButtonLayout.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = true;
        }

        //Reset du premier objet sélectionné par l'Event System et initialisation du nouveau premier objet sélectionné sur "Non" (prévient l'appuie "SPAM")
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(validationPopupButtonLayout.transform.GetChild(1).gameObject);

        validationPopupIsDisplayed = true;
    }

    //Summary : Permet de désafficher la fenêtre de confirmation d'achat 
    public void HideValidationPopup()
    {
        StartCoroutine(FadeCanvasGroup(validationPopupWindow, validationPopupWindow.alpha, 0, fadeDuration));
        validationPopupWindow.blocksRaycasts = false;

        foreach (Button _buttons in validationPopupWindow.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = false;
        }

        EventSystem.current.SetSelectedGameObject(null);
        validationPopupIsDisplayed = false;
    }
    #endregion

    public void GoToTrial()
    {
        SceneManager.LoadScene("SceneProces001");
    }

    void SetFadeOutTextsIndicator()
    {
        dayNumber.text = "Jour " + GameManager.s_Singleton.trialDayNumber.ToString();
        trialSubject.text = titreProcesTxt.text;
    }
}
