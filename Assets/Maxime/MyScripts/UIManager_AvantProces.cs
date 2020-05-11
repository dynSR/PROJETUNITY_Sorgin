using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager_AvantProces : MonoBehaviour
{
    public static UIManager_AvantProces singleton;

    public TextMeshProUGUI titreProcesTxt;
    public int numeroProces;
    public string chefAccusation;

    public TextMeshProUGUI docActuelTxt;

    [Header("PURCHASE VALIDATION POPUP PARAMETERS")]
    [SerializeField] private CanvasGroup validationPopupWindow;
    [SerializeField] private GameObject validationPopupButtonLayout;
    [HideInInspector]
    public bool validationPopupIsDisplayed = false;

    [Header("FADE DURATION")]
    public float fadeDuration = 0.25f;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        titreProcesTxt.text = chefAccusation + " - Proces n°" + numeroProces;
    }

    // Update is called once per frame
    void Update()
    {
        if (!validationPopupIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X")))
        {
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

    public void GoToTrial()
    {
        SceneManager.LoadScene("SceneProces001");
    }

    //Summary : Utiliser pour réaliser des effets de Fade-In / Fade-Out. Utilisé notamment pour faire apparaître ou disparaître des fenêtres d'UI.
    #region Canvas Fade Coroutine
    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {
        float _timerStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timerStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timerStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
