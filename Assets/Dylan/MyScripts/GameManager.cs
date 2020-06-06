using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { PlayMode, Pause, Cinematic, ConsultingShop, InMainMenu }

public class GameManager : MonoBehaviour
{
    [Header("PLAYER POINTS")]
    public int playerPointsValue;

    public GameState gameState;

    public int trialDayNumber = 1;

    public static GameManager s_Singleton;

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
            DontDestroyOnLoad(this.gameObject);
        }

        if(GetTheIntVariable("TrialDayNumber") != 0)
            trialDayNumber = GetTheIntVariable("TrialDayNumber");

        playerPointsValue = GetTheIntVariable("PlayerPoints");
        Debug.Log(trialDayNumber);

        //Pour les tests, à commenter pour les builds etc
        //gameState = GameState.PlayMode;

        //gameState = GameState.InMainMenu;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.PlayMode:
                Time.timeScale = 1f;
                break;
            case GameState.Pause:
                //For Test
                //Time.timeScale = 0f;
                break;
            case GameState.Cinematic:
                Time.timeScale = 1f;
                //Faire quelque chose si besoin...
                break;
            case GameState.ConsultingShop:
                Time.timeScale = 1f;
                //Faire quelque chose si besoin...
                break;
            case GameState.InMainMenu:
                Time.timeScale = 1f;
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            trialDayNumber++;
            SaveTheIntVariable("TrialDayNumber", trialDayNumber);
            Debug.Log(trialDayNumber);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            playerPointsValue += 50;
            SaveTheIntVariable("PlayerPoints", playerPointsValue);
            Debug.Log(trialDayNumber);
        }
    }

    private void OnApplicationQuit()
    {
        SaveTheIntVariable("TrialDayNumber", trialDayNumber);
        SaveTheIntVariable("PlayerPoints", playerPointsValue);

        //A decommenter à la fin pour la build finale 
        PlayerPrefs.DeleteAll();
    }


    #region Save Methods
    public int GetTheIntVariable(string keyName)
    {
        return PlayerPrefs.GetInt(keyName);
    }

    public void SaveTheIntVariable(string keyName, int keyValue)
    {
        PlayerPrefs.SetInt(keyName, keyValue);
    }
    #endregion
}
