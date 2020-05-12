using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DefaultUIManager : MonoBehaviour
{
    [Header("FADE DURATION")]
    public float fadeDuration = 0.25f;

    [Header("PARAMETERS TO SET THE GAME IN PAUSE")]
    public GameObject pauseMenuWindow;
    public GameObject pauseMenuInputsDisplayerWindow;
    public GameObject pauseMenuOptionsWindow;
    public GameObject firstButtonOfPauseMenu;
    public GameObject optionsFirstSelectedButton;
    public GameObject lastSelectedButton;
    public GameObject pauseMenuOptionsButton;


    public bool pauseWindowIsDisplayed = false;
    [HideInInspector] public bool pauseWindowOptionsAreDisplayed = false;
    [HideInInspector] public bool pauseWindowInputsDisplayerIsDisplayed = false;


    public virtual void Update()
    {
        if(ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Options") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_Start"))
        {
            Debug.Log("Pressed Options or Start and trying to set the game in pause....");
            TogglePause();
        }

        if (pauseWindowIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
        {
            BackInPauseMenu();
        }
    }

    public void TogglePause()
    {
        Debug.Log("Set Game To Pause Mode");

        if (!pauseWindowIsDisplayed) lastSelectedButton = EventSystem.current.currentSelectedGameObject;

        UIWindowsDisplayToggle(pauseMenuWindow);
        pauseWindowIsDisplayed = !pauseWindowIsDisplayed;
        
        if (pauseWindowIsDisplayed)
        {
            GameManager.s_Singleton.gameStates = GameState.Pause;
            ResetEventSystemFirstSelectedGameObjet(firstButtonOfPauseMenu);
        } 
        else
        {
            GameManager.s_Singleton.gameStates = GameState.PlayMode;
            ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
        }  
    }

    public void BackInPauseMenu()
    {
        if(pauseWindowOptionsAreDisplayed)
        {
            UIWindowsHide(pauseMenuOptionsWindow);
            pauseWindowOptionsAreDisplayed = false;
            EventSystem.current.SetSelectedGameObject(pauseMenuOptionsButton);
        }
        else if(pauseWindowInputsDisplayerIsDisplayed)
        {
            UIWindowsHide(pauseMenuInputsDisplayerWindow);
            pauseWindowInputsDisplayerIsDisplayed = false;
        }
        else
        {
            UIWindowsHide(pauseMenuWindow);
            pauseWindowIsDisplayed = false;
            ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
            GameManager.s_Singleton.gameStates = GameState.PlayMode;
        }
    }

    public void OnClickResumeButtonInPauseMenu()
    {
        UIWindowsHide(pauseMenuWindow);
        pauseWindowIsDisplayed = false;
        GameManager.s_Singleton.gameStates = GameState.PlayMode;

        ResetEventSystemFirstSelectedGameObjet(lastSelectedButton);
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
        EventSystem.current.SetSelectedGameObject(optionsFirstSelectedButton);
    }

    public void OnClickBackToMainMenuButtonInPauseMenu()
    {
        UIWindowsHide(pauseMenuWindow);
        pauseWindowIsDisplayed = false;
        GameManager.s_Singleton.gameStates = GameState.PlayMode;

        SceneManager.LoadScene("Scene_MainMenu");
    }

    public void OnClickQuitButtonInPauseMenu()
    {
        Debug.Log("Quitte le jeu...");
        Application.Quit();
    }

    //

    #region Tools For UI
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
