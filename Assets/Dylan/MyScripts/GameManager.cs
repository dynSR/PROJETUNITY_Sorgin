using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { PlayMode, Pause, Cinematic, ConsultingShop }

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

        gameState = GameState.PlayMode;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.PlayMode:
                Time.timeScale = 1f;
                break;
            case GameState.Pause:
                //Time.timeScale = 0f;
                break;
            case GameState.Cinematic:
                //Faire quelque chose si besoin...
                break;
            case GameState.ConsultingShop:
                //Faire quelque chose si besoin...
                break;
            default:
                break;
        }
    }
}
