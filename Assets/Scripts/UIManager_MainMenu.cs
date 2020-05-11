using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager_MainMenu : MonoBehaviour
{
    [Header("NAME OF SCENES TO LOAD")]
    [SerializeField] private string sceneToLoadOnClickPlayButton;

    [Header("MENU BUTTONS")]
    [SerializeField] private GameObject mainMenuFirstButton;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject optionsFirstButton;

    [Header("FADING PARAMETERS")]
    [SerializeField] private CanvasGroup splashScreenWindow;
    [SerializeField] private CanvasGroup mainMenuWindow;
    [SerializeField] private CanvasGroup inputsDisplayerWindow;
    [SerializeField] private CanvasGroup optionsWindow;
    [SerializeField] private CanvasGroup creditsWindow;
    [SerializeField] private float lerpTime = 0.75f;
    [SerializeField] private float transitionBetweenTwoFades = 1.25f;

    [Header("INPUTS LAYOUT PARAMETERS")]
    [SerializeField] private Image inputsLayoutImage;
    [SerializeField] private Sprite[] inputsLayoutImageArray;
    [SerializeField] private TextMeshProUGUI inputLayoutDisplayedIdx;
    public int imageToDisplayIdx = 0;

    [Header("DEBUG DISPLAYING")]
    public bool splashScreenIsDisplayed = true;
    public bool mainMenuIsDisplayed = false;
    public bool inputsDisplayerIsDisplayed = false;
    public bool optionsAreDisplayed = false;
    public bool creditsAreDisplayed = false;

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

    }

    // Update is called once per frame
    void Update()
    {
        if (splashScreenIsDisplayed && !mainMenuIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_X") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_A")))
        {
            HideSplashScreenAndDisplayMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ButtonReturnMenu();
        }
        if (!mainMenuIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
        {
            StartCoroutine(BackToMainMenu());
        }

        //if (!splashScreenIsDisplayed && mainMenuIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
        //{
        //    HideMainMenuAndDisplaySplashScreen();
        //}

        SwitchInputLayoutDisplayed();
    }

    void SwitchInputLayoutDisplayed()
    {
        if (inputsDisplayerIsDisplayed)
        {
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_L1") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_LB"))
            {
                if (imageToDisplayIdx == 0)
                {
                    imageToDisplayIdx = inputsLayoutImageArray.Length-1;
                }
                else
                {
                    imageToDisplayIdx--;
                }
            }
            else if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_R1") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_RB"))
            {
                if (imageToDisplayIdx == inputsLayoutImageArray.Length-1)
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

        inputsLayoutImage.sprite = inputsLayoutImageArray[imageToDisplayIdx];

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
        mainMenuIsDisplayed = false;
        inputsDisplayerIsDisplayed = true;
    }

    public void OnClickOptionsButton()
    {
        StartCoroutine(AlternateTwoFadingsAtTheSameTime(optionsWindow, mainMenuWindow));
        SetEventSystemFirstSelectedGameObject(optionsFirstButton);
        mainMenuIsDisplayed = false;
        optionsAreDisplayed = true;
    }

    public void OnClickCreditsButton()
    {
        StartCoroutine(AlternateTwoFadingsAtTheSameTime(creditsWindow, mainMenuWindow));
        mainMenuIsDisplayed = false;
        creditsAreDisplayed = true;
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
        windowCanvas.blocksRaycasts = false;
        windowCanvas.interactable = false;
    }

    private void DisplayAWindow(CanvasGroup windowCanvas)
    {
        Debug.Log("Trying to reach DisplayAWindow Function...");
        StartCoroutine(FadeCanvasGroup(windowCanvas, windowCanvas.alpha, 1, lerpTime));
        windowCanvas.blocksRaycasts = true;
        windowCanvas.interactable = true;
    }

    private void HideSplashScreenAndDisplayMainMenu()
    {
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
        StartCoroutine(AlternateTwoFadingsAtTheSameTime(mainMenuWindow ,splashScreenWindow));
        
        splashScreenIsDisplayed = false;
        mainMenuIsDisplayed = true;
    }

    //private void HideMainMenuAndDisplaySplashScreen()
    //{
    //    EventSystem.current.SetSelectedGameObject(null);
    //    StartCoroutine(AlternateTwoFadingsAtTheSameTime(splashScreenWindow, mainMenuWindow));
    //    splashScreenIsDisplayed = true;
    //    mainMenuIsDisplayed = false;
    //}

    void SetEventSystemFirstSelectedGameObject(GameObject objToSetAsFirstGameObject)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(objToSetAsFirstGameObject);
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
        bool canDisplayMainMenu = false;

        if (inputsDisplayerWindow.alpha == 1)
        {
            HideAWindow(inputsDisplayerWindow);
            inputsDisplayerIsDisplayed = false;
            canDisplayMainMenu = true;
            
        }
        else if (optionsWindow.alpha == 1)
        {
            HideAWindow(optionsWindow);
            optionsAreDisplayed = false;
            EventSystem.current.SetSelectedGameObject(optionsButton);
            canDisplayMainMenu = true;
        }
        else if (creditsWindow.alpha == 1)
        {
            HideAWindow(creditsWindow);
            creditsAreDisplayed = false;
            canDisplayMainMenu = true;
        }

        yield return new WaitForSeconds(transitionBetweenTwoFades);

        if(canDisplayMainMenu)
        {
            DisplayAWindow(mainMenuWindow);
            mainMenuIsDisplayed = true;
        }
        //SetEventSystemFirstSelectedGameObject(mainMenuFirstButton);
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
