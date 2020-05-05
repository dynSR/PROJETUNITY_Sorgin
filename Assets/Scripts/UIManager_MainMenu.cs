using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager_MainMenu : MonoBehaviour
{
    [Header("NAME OF SCENES TO LOAD")]
    [SerializeField] private string sceneToLoadOnClickPlayButton;

    [Header("WWISE EVENT SOUND NAME")]
    [SerializeField] private string displayingWindowWwiseEventSoundName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ButtonReturnMenu();
        }
    }

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
        SceneManager.LoadScene("SBLobbyScene");
    }

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene(sceneToLoadOnClickPlayButton);
    }

    public void OnClickDisplayInputsButton()
    {
        AkSoundEngine.PostEvent(displayingWindowWwiseEventSoundName, this.gameObject);
    }

    public void OnClickOptionsButton()
    {
        AkSoundEngine.PostEvent(displayingWindowWwiseEventSoundName, this.gameObject);
    }

    public void OnClickCreditsButton()
    {
        AkSoundEngine.PostEvent(displayingWindowWwiseEventSoundName, this.gameObject);
    }

    public void OnClickQuitButton()
    {
        Debug.Log("Quitte le jeu");
        Application.Quit();
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
