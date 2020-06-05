using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { PlayMode, Pause, Cinematic, ConsultingShop, InMainMenu }

public class GameManager : MonoBehaviour
{
    [Header("PLAYER POINTS")]
    public int playerPointsValue;

    public GameState gameState;

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
    }
}
