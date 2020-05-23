using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager_MainMenu : DefaultUIManager
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

    //[Header("SUB-MENUS INPUTS LANDMARK")]
    //[SerializeField] private GameObject mainMenuInputsLandmark;
    //[SerializeField] private GameObject inputsDisplayerInputsLandmark;
    //[SerializeField] private GameObject optionsInputsLandmark;
    //[SerializeField] private GameObject creditsInputsLandmark;

    [Header("DEBUG DISPLAYING")]
    public bool splashScreenIsDisplayed = true;
    public bool mainMenuIsDisplayed = false;
    public bool inputsDisplayerIsDisplayed = false;
    public bool optionsAreDisplayed = false;
    public bool creditsAreDisplayed = false;

    public static UIManager_MainMenu s_Singleton;

    #region Singleton
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (s_Singleton != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            s_Singleton = this;
            GameManager.s_Singleton.gameState = GameState.InMainMenu;

            if (playerIsBackToMainMenu)
            {
                HideSplashScreenAndDisplayMainMenu();
                playerIsBackToMainMenu = false;

                Debug.Log("Player is back to main menu " + playerIsBackToMainMenu);
            }
        }

       
    }
    #endregion

    private void Start()
    {

        
    }

    new void Update()
    {
        if (splashScreenIsDisplayed && !mainMenuIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_X") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_A")))
        {
            HideSplashScreenAndDisplayMainMenu();
        }
        if (!mainMenuIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
        {
            StartCoroutine(BackToMainMenuFromAnySubMenu());
        }

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
        SceneManager.LoadScene("SceneAvProces001");
    }

    public void ButtonProces()
    {
        SceneManager.LoadScene("SceneProces001");
    }

    public void ButtonShop()
    {
        SceneManager.LoadScene("00_SceneTest");
    }

    public void ButtonExfiltration()
    {
        SceneManager.LoadScene("SceneBuild");
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
        GameManager.s_Singleton.gameState = GameState.PlayMode;
        SceneManager.LoadScene(sceneToLoadOnClickPlayButton);
    }

    public void OnClickDisplayInputsButton()
    {
        StartCoroutine(AlternateTwoFadingsAtTheSameTime(inputsDisplayerWindow, mainMenuWindow));
        mainMenuIsDisplayed = false;
        inputsDisplayerIsDisplayed = true;
        //HideASubMenuAndDisplayAnotherOne(mainMenuInputsLandmark, inputsDisplayerInputsLandmark);
    }

    public void OnClickOptionsButton()
    {
        StartCoroutine(AlternateTwoFadingsAtTheSameTime(optionsWindow, mainMenuWindow));
        SetEventSystemFirstSelectedGameObject(optionsFirstButton);
        mainMenuIsDisplayed = false;
        optionsAreDisplayed = true;
        //HideASubMenuAndDisplayAnotherOne(mainMenuInputsLandmark, optionsInputsLandmark);
    }

    public void OnClickCreditsButton()
    {
        StartCoroutine(AlternateTwoFadingsAtTheSameTime(creditsWindow, mainMenuWindow));
        mainMenuIsDisplayed = false;
        creditsAreDisplayed = true;
        //HideASubMenuAndDisplayAnotherOne(mainMenuInputsLandmark, creditsInputsLandmark);
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
        Debug.Log("Hiding SplashScreen");
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        StartCoroutine(AlternateTwoFadingsAtTheSameTime(mainMenuWindow ,splashScreenWindow));
        
        splashScreenIsDisplayed = false;
        mainMenuIsDisplayed = true;

        //HideASubMenuAndDisplayAnotherOne(null, mainMenuInputsLandmark);
    }

    void SetEventSystemFirstSelectedGameObject(GameObject objToSetAsFirstGameObject)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(objToSetAsFirstGameObject);
    }

    void HideASubMenuAndDisplayAnotherOne(GameObject subMenuToHide, GameObject subMenuToDisplay)
    {
        if(subMenuToHide != null)
            subMenuToHide.SetActive(false);

        if(subMenuToDisplay != null)
            subMenuToDisplay.SetActive(true);
    }

    IEnumerator AlternateTwoFadingsAtTheSameTime(CanvasGroup windowToDisplay, CanvasGroup windowToHide)
    {
        Debug.Log("Access to AlternateTwoFadingsAtTheSameTime, trying to hide.....");
        HideAWindow(windowToHide);

        yield return new WaitForSeconds(transitionBetweenTwoFades);

        Debug.Log("Trying to display.....");
        DisplayAWindow(windowToDisplay);
    }

    IEnumerator BackToMainMenuFromAnySubMenu()
    {
        bool canDisplayMainMenu = false;

        if (inputsDisplayerWindow.alpha == 1)
        {
            HideAWindow(inputsDisplayerWindow);
            inputsDisplayerIsDisplayed = false;
            canDisplayMainMenu = true;
            //HideASubMenuAndDisplayAnotherOne(inputsDisplayerInputsLandmark, mainMenuInputsLandmark);
        }
        else if (optionsWindow.alpha == 1)
        {
            HideAWindow(optionsWindow);
            optionsAreDisplayed = false;
            EventSystem.current.SetSelectedGameObject(optionsButton);
            canDisplayMainMenu = true;
            //HideASubMenuAndDisplayAnotherOne(optionsInputsLandmark, mainMenuInputsLandmark);
        }
        else if (creditsWindow.alpha == 1)
        {
            HideAWindow(creditsWindow);
            creditsAreDisplayed = false;
            canDisplayMainMenu = true;
            //HideASubMenuAndDisplayAnotherOne(creditsInputsLandmark, mainMenuInputsLandmark);
        }

        yield return new WaitForSeconds(transitionBetweenTwoFades);

        if(canDisplayMainMenu)
        {
            DisplayAWindow(mainMenuWindow);
            mainMenuIsDisplayed = true;
        }
    }
}
