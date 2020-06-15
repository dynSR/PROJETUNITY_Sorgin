using Fungus;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefaultUIManager : MonoBehaviour
{
    [Header("FADE DURATION")]
    public float fadeDuration = 0.25f;

    [Header("PARAMETERS TO SET THE GAME IN PAUSE")]
    public GameObject pauseMenuWindow;
    public GameObject pauseMenuButtonLayout;
    public GameObject firstButtonOfPauseMenu;
    public GameObject pauseMenuInputsDisplayerWindow;
    public GameObject pauseMenuInputsDisplayerButton;
    public GameObject pauseMenuOptionsButton;
    public GameObject pauseMenuOptionsWindow;
    public GameObject optionsFirstSelectedButton;
    
    [Header("LAST SELECTED BUTTON")]
    public static GameObject lastSelectedButton;

    [Header("INPUTS LAYOUT PARAMETERS")]
    [SerializeField] private Image inputsLayoutImage;
    [SerializeField] private Sprite[] inputsLayoutImageArray;
    [SerializeField] private TextMeshProUGUI inputLayoutDisplayedIdx;
    public bool inputsDisplayerIsDisplayed = false;
    private int imageToDisplayIdx = 0;

    [Header("WWISE EVENT ")]
    public AK.Wwise.Event ostSwitchWwiseEvent;

    public bool pauseWindowIsDisplayed = false;
    public bool pauseWindowHasBeenClosed = false;
    [HideInInspector] public bool pauseWindowOptionsAreDisplayed = false;
    [HideInInspector] public bool pauseWindowInputsDisplayerIsDisplayed = false;
    public static bool playerIsBackToMainMenu = false;
   

    public virtual void Update()
    {
        if ((GameManager.s_Singleton.gameState == GameState.PlayMode || GameManager.s_Singleton.gameState == GameState.Pause) && pauseMenuWindow != null)
        {
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Options") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_Start"))
            {
                Debug.Log("Pressed Options or Start and trying to set the game in pause....");
                if (!pauseWindowIsDisplayed)
                {
                    Pause();
                }
                else if (pauseWindowIsDisplayed && !pauseWindowOptionsAreDisplayed && !pauseWindowInputsDisplayerIsDisplayed)
                {
                    Resume();
                }
            }
        }

        if (pauseWindowIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
        {
            BackInPauseMenu();
        }

        SwitchInputLayoutDisplayed();
    }

    private void Pause()
    {
        Debug.Log("Set Game To Pause Mode");
        StartCoroutine(DisplayPauseWindowAndSetFirstButton());
    }

    IEnumerator DisplayPauseWindowAndSetFirstButton()
    {
        pauseMenuWindow.SetActive(true);
        pauseWindowIsDisplayed = true;
        pauseWindowHasBeenClosed = false;

        yield return new WaitForEndOfFrame();

        EnableButtonsInLayout(pauseMenuButtonLayout, firstButtonOfPauseMenu);
        GameManager.s_Singleton.gameState = GameState.Pause;
    }

    private void Resume()
    {
        pauseMenuWindow.gameObject.SetActive(false);

        DisableButtonsInLayout(pauseMenuButtonLayout);

        pauseWindowIsDisplayed = false;
        pauseWindowHasBeenClosed = true;

        GameManager.s_Singleton.gameState = GameState.PlayMode;

        //if(lastSelectedButton != null)
        //    ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
        //EventSystem.current.firstSelectedGameObject = lastSelectedButton;
    }

    public void BackInPauseMenu()
    {
        if(pauseWindowOptionsAreDisplayed)
        {
            UIWindowsHide(pauseMenuOptionsWindow);
            pauseWindowOptionsAreDisplayed = false;
            EnableButtonsInLayout(pauseMenuButtonLayout, pauseMenuOptionsButton);
            //EventSystem.current.SetSelectedGameObject(pauseMenuOptionsButton);
        }
        else if(pauseWindowInputsDisplayerIsDisplayed)
        {
            UIWindowsHide(pauseMenuInputsDisplayerWindow);
            pauseWindowInputsDisplayerIsDisplayed = false;
            EnableButtonsInLayout(pauseMenuButtonLayout, pauseMenuInputsDisplayerButton);
            //ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
        }
        else
        {
            Resume();
        }
    }

    #region Buttons Behaviour
    public void OnClickResumeButton()
    {
        Resume();
    }

    public void OnClickDisplayInputsButton()
    {
        UIWindowsDisplay(pauseMenuInputsDisplayerWindow);
        pauseWindowInputsDisplayerIsDisplayed = true;
        DisableButtonsInLayout(pauseMenuButtonLayout);
    }

    public void OnClickDisplayOptionsButton()
    {
        UIWindowsDisplay(pauseMenuOptionsWindow);
        pauseWindowOptionsAreDisplayed = true;

        DisableButtonsInLayout(pauseMenuButtonLayout);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstSelectedButton);
    }

    public void OnClickBackToMainMenuButton()
    {
        pauseMenuWindow.SetActive(false);
        pauseWindowIsDisplayed = false;
        GameManager.s_Singleton.gameState = GameState.InMainMenu;
        playerIsBackToMainMenu = true;
        Debug.Log("Player is back to main menu " + playerIsBackToMainMenu);

        SceneManager.LoadScene("Scene_MainMenu");
    }

    public void OnClickQuitButton()
    {
        Debug.Log("Quitte le jeu...");
        Application.Quit();
    }
    #endregion

    public void SwitchInputLayoutDisplayed()
    {
        if (inputsLayoutImage != null && (inputsDisplayerIsDisplayed || pauseWindowInputsDisplayerIsDisplayed))
        {
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_L1") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_LB"))
            {
                if (imageToDisplayIdx == 0)
                {
                    imageToDisplayIdx = inputsLayoutImageArray.Length - 1;
                }
                else
                {
                    imageToDisplayIdx--;
                }
            }
            else if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_R1") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_RB"))
            {
                if (imageToDisplayIdx == inputsLayoutImageArray.Length - 1)
                {
                    imageToDisplayIdx = 0;
                }
                else
                {
                    imageToDisplayIdx++;
                }
            }

            if (inputsLayoutImage.sprite == inputsLayoutImageArray[0])
            {
                Debug.Log("Inputs Avant-Procès");
                inputLayoutDisplayedIdx.text = "1 / 3";
            }
            else if (inputsLayoutImage.sprite == inputsLayoutImageArray[1])
            {
                Debug.Log("Inputs Procès");
                inputLayoutDisplayedIdx.text = "2 / 3";
            }
            else if (inputsLayoutImage.sprite == inputsLayoutImageArray[2])
            {
                Debug.Log("Inputs Exfiltration");
                inputLayoutDisplayedIdx.text = "3 / 3";
            }
        }

        if(inputsLayoutImage != null)
            inputsLayoutImage.sprite = inputsLayoutImageArray[imageToDisplayIdx];
    }

    //

    #region Tools For UI
    //Summary : Affiche une fenêtre popup.
    public void DisplayAPopup(CanvasGroup popupToDisplay)
    {
        //Affichage de la fenêtre de confirmation d'achat en Fade-In
        if (popupToDisplay.alpha == 0)
        {
            StartCoroutine(FadeCanvasGroup(popupToDisplay, popupToDisplay.alpha, 1, fadeDuration));
            popupToDisplay.blocksRaycasts = true;
        }
    }

    //Summary : Permet de désafficher une fenêtre popup. 
    public void HideAPopup(CanvasGroup popupToHide)
    {
        if (popupToHide.alpha == 1)
        {
            StartCoroutine(FadeCanvasGroup(popupToHide, popupToHide.alpha, 0, fadeDuration));
            popupToHide.blocksRaycasts = false;
        }
    }


    //Utilisé dans les button component pour afficher / désafficher un élément d'UI
    public void UIWindowsDisplayToggle(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void UIWindowsDisplay(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void UIWindowsHide(GameObject obj)
    {
        obj.SetActive(false);
    }

    //Summary : Permet de reset et de déterminer le premier objet sélectionné dans l'Event System (obligatoire à cause de l'utilisation de la manette)
    public void ResetEventSystemFirstSelectedGameObjet(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);

        Debug.Log(obj.name);
    }
    #endregion

    #region Enable or Disable Buttons in a Layout
    public void EnableButtonsInLayout(GameObject buttonLayout, GameObject firstSelectedObject)
    {
        //Réactivation des boutons contenus dans cette fenêtre (prévient les problèmes liés à la navigation de l'Event System)
        foreach (Button _buttons in buttonLayout.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = true;
        }

        //Reset du premier objet sélectionné par l'Event System et initialisation du nouveau premier objet sélectionné sur "Non" (prévient l'appuie "SPAM")
        if(firstSelectedObject != null)
            ResetEventSystemFirstSelectedGameObjet(firstSelectedObject);
    }

    public void DisableButtonsInLayout(GameObject buttonLayout)
    {
        foreach (Button _buttons in buttonLayout.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = false;
        }

        PurchaseASpell purchasedSpell = buttonLayout.GetComponentInChildren<PurchaseASpell>();

        //if (purchasedSpell != null && purchasedSpell.gameObject.activeInHierarchy)
        //    ResetEventSystemFirstSelectedGameObjet(purchasedSpell.selectedButton.gameObject);
        //else
        //    ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
    }
    #endregion

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

    public void SetOSTStateAndPostEvent(string state)
    {
        AkSoundEngine.SetState("inGamePhases", state);
        //ostSwitchWwiseEvent.Post(this.gameObject);
    }
}
