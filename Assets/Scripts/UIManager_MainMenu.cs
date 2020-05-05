using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager_MainMenu : MonoBehaviour
{
    [Header("NAME OF SCENES TO LOAD")]
    [SerializeField] private string sceneToLoadOnClickPlayButton;

    [Header("FADING PARAMETERS")]
    [SerializeField] private CanvasGroup mainMenuWindow;
    [SerializeField] private CanvasGroup inputsDisplayerWindow;
    [SerializeField] private CanvasGroup optionsWindow;
    [SerializeField] private CanvasGroup creditsWindow;
    [SerializeField] private float lerpTime = 0.75f;
    [SerializeField] private float transitionBetweenTwoFades = 1.25f;
    private float transitionBetweenTwoFadesAtStart;

    public static UIManager_MainMenu s_Singleton;
    private void Awake()
    {
        if (s_Singleton != null)
        {
            Destroy(this);
        }
        else
        {
            s_Singleton = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transitionBetweenTwoFadesAtStart = transitionBetweenTwoFades;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ButtonReturnMenu();
        }
        if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B"))
        {
            StartCoroutine(BackToMainMenu());
        }
    }

    #region Debug Buttons
    public void ButtonAvantProces()
    {
        SceneManager.LoadScene("SBSceneAvProces");
    }

    public void ButtonProces()
    {
        SceneManager.LoadScene("SBSceneProces");
    }

    public void ButtonShop()
    {
        SceneManager.LoadScene("00_SceneTest");
    }

    public void ButtonExfiltration()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ButtonReturnMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Scene_MainMenu");
    }
    #endregion

    #region Main menu buttons
    public void OnClickPlayButton()
    {
        SceneManager.LoadScene(sceneToLoadOnClickPlayButton);
    }

    public void OnClickDisplayInputsButton()
    {
        StartCoroutine(AlternateTwoFadingsAtTheSameTime(inputsDisplayerWindow, mainMenuWindow));
    }

    public void OnClickOptionsButton()
    {
        StartCoroutine(AlternateTwoFadingsAtTheSameTime(optionsWindow, mainMenuWindow));
    }

    public void OnClickCreditsButton()
    {
        StartCoroutine(AlternateTwoFadingsAtTheSameTime(creditsWindow, mainMenuWindow));
    }

    public void OnClickQuitButton()
    {
        Debug.Log("Quitte le jeu");
        Application.Quit();
    }
    #endregion
    private void HideAWindow(CanvasGroup windowCanvas)
    {
        Debug.Log("Trying to reach HideAWindow Function...");
        StartCoroutine(FadeCanvasGroup(windowCanvas, windowCanvas.alpha, 0, lerpTime));
    }

    private void DisplayAWindow(CanvasGroup windowCanvas)
    {
        Debug.Log("Trying to reach DisplayAWindow Function...");
        StartCoroutine(FadeCanvasGroup(windowCanvas, windowCanvas.alpha, 1, lerpTime));
    }

    IEnumerator AlternateTwoFadingsAtTheSameTime(CanvasGroup windowToDisplay, CanvasGroup windowToHide)
    {
        Debug.Log("Access to AlternateTwoFadingsAtTheSameTime, trying to hide.....");
        HideAWindow(windowToHide);

        yield return new WaitForSeconds(transitionBetweenTwoFades);

        Debug.Log("Trying to display.....");
        DisplayAWindow(windowToDisplay);
    }

    IEnumerator BackToMainMenu()
    {
        if (inputsDisplayerWindow.alpha == 1)
        {
            HideAWindow(inputsDisplayerWindow);
        }
        else if (optionsWindow.alpha == 1)
        {
            HideAWindow(optionsWindow);
        }
        else if (creditsWindow.alpha == 1)
        {
            HideAWindow(creditsWindow);
        }

        yield return new WaitForSeconds(transitionBetweenTwoFades);

        DisplayAWindow(mainMenuWindow);
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
