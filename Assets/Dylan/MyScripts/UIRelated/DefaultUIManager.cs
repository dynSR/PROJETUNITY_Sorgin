using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefaultUIManager : MonoBehaviour
{
    [Header("FADE DURATION")]
    public float fadeDuration = 0.25f;

    [Header("PARAMETERS TO SET THE GAME IN PAUSE")]
    public CanvasGroup pauseMenuWindow;
    public GameObject pauseMenuButtonLayout;
    public GameObject pauseMenuInputsDisplayerWindow;
    public GameObject pauseMenuOptionsWindow;
    public GameObject firstButtonOfPauseMenu;
    public GameObject optionsFirstSelectedButton;
    public GameObject pauseMenuOptionsButton;

    [Header("LAST SELECTED BUTTON")]
    public static GameObject lastSelectedButton;


    public bool pauseWindowIsDisplayed = false;
    [HideInInspector] public bool pauseWindowOptionsAreDisplayed = false;
    [HideInInspector] public bool pauseWindowInputsDisplayerIsDisplayed = false;


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
                else if (pauseWindowIsDisplayed)
                {
                    Resume();
                }
            }
        }

        if (pauseWindowIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
        {
            BackInPauseMenu();
        }
    }

    private void Pause()
    {
        Debug.Log("Set Game To Pause Mode");
        pauseMenuWindow.alpha = 1;
        pauseMenuWindow.blocksRaycasts = true;
        EnableButtonsInLayout(pauseMenuButtonLayout);

        EventSystem.current.SetSelectedGameObject(firstButtonOfPauseMenu);
        GameManager.s_Singleton.gameState = GameState.Pause;

        pauseWindowIsDisplayed = true;
    }

    private void Resume()
    {
        pauseMenuWindow.alpha = 0;
        pauseMenuWindow.blocksRaycasts = false;
        DisableButtonsInLayout(pauseMenuButtonLayout);
        pauseWindowIsDisplayed = false;
        GameManager.s_Singleton.gameState = GameState.PlayMode;

        ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
    }

    public void BackInPauseMenu()
    {
        if(pauseWindowOptionsAreDisplayed)
        {
            UIWindowsHide(pauseMenuOptionsWindow);
            pauseWindowOptionsAreDisplayed = false;
            ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
            //EventSystem.current.SetSelectedGameObject(pauseMenuOptionsButton);
        }
        else if(pauseWindowInputsDisplayerIsDisplayed)
        {
            UIWindowsHide(pauseMenuInputsDisplayerWindow);
            pauseWindowInputsDisplayerIsDisplayed = false;
        }
        else
        {
            HideAPopup(pauseMenuWindow);
            pauseWindowIsDisplayed = false;
            ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
            GameManager.s_Singleton.gameState = GameState.PlayMode;
        }
    }

    public void OnClickResumeButtonInPauseMenu()
    {
        Resume();
    }

    public void OnClickDisplayInputsButtonInPauseMenu()
    {
        UIWindowsDisplay(pauseMenuInputsDisplayerWindow);
        pauseWindowInputsDisplayerIsDisplayed = true;
    }

    public void OnClickDisplayOptionsButtonInPauseMenu()
    {
        UIWindowsDisplay(pauseMenuOptionsWindow);
        pauseWindowOptionsAreDisplayed = true;
        //EventSystem.current.SetSelectedGameObject(optionsFirstSelectedButton);
        ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
    }

    public void OnClickBackToMainMenuButtonInPauseMenu()
    {
        HideAPopup(pauseMenuWindow);
        pauseWindowIsDisplayed = false;
        GameManager.s_Singleton.gameState = GameState.PlayMode;

        SceneManager.LoadScene("Scene_MainMenu");
    }

    public void OnClickQuitButtonInPauseMenu()
    {
        Debug.Log("Quitte le jeu...");
        Application.Quit();
    }

    //

    #region Tools For UI
    //Summary : Affiche une fenêtre popup.
    public void DisplayAPopup(CanvasGroup popupToDisplay)
    {
        //Affichage de la fenêtre de confirmation d'achat en Fade-In
        StartCoroutine(FadeCanvasGroup(popupToDisplay, popupToDisplay.alpha, 1, fadeDuration));
        popupToDisplay.blocksRaycasts = true;
    }

    //Summary : Permet de désafficher une fenêtre popup. 
    public void HideAPopup(CanvasGroup popupToHide)
    {
        StartCoroutine(FadeCanvasGroup(popupToHide, popupToHide.alpha, 0, fadeDuration));
        popupToHide.blocksRaycasts = false;
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
    }
    #endregion

    public void EnableButtonsInLayout(GameObject buttonLayout)
    {
        //Réactivation des boutons contenus dans cette fenêtre (prévient les problèmes liés à la navigation de l'Event System)
        foreach (Button _buttons in buttonLayout.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = true;
        }

        //Reset du premier objet sélectionné par l'Event System et initialisation du nouveau premier objet sélectionné sur "Non" (prévient l'appuie "SPAM")
        ResetEventSystemFirstSelectedGameObjet(buttonLayout.transform.GetChild(1).gameObject);
    }

    public void DisableButtonsInLayout(GameObject buttonLayout)
    {
        foreach (Button _buttons in buttonLayout.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = false;
        }

        PurchaseASpell purchasedSpell = buttonLayout.GetComponentInChildren<PurchaseASpell>();

        if (purchasedSpell != null)
            ResetEventSystemFirstSelectedGameObjet(purchasedSpell.selectedButton.gameObject);
        else
            ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
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
