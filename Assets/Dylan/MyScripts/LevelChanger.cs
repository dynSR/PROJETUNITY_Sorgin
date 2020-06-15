using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class LevelChanger : MonoBehaviour
{
    //public pour DEBUG 
    private int levelToLoadId;

    /*[HideInInspector] */public Animator animator;

    private int mainMenuSceneId = 0;
    private int firstBeforeTrialSceneId = 1;
    private int firstTrialSceneId = 2;
    private int secondBeforeTrialSceneId = 5;
    private int secondTrialSceneId = 6;
    private int exfiltrationSceneId = 4;


    public static LevelChanger s_Singleton;

    private void Awake()
    {
        if (s_Singleton != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            s_Singleton = this;
        }

        animator = GetComponent<Animator>();
    }

    private void LevelToLoad(int levelId)
    {
        levelToLoadId = levelId;
        SetAnimatorTrigger("FadeIn");
    }

    public void SetAnimatorTrigger(string triggerToSet)
    {
        animator.SetTrigger(triggerToSet);
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoadId);
    }

    #region MainMenu
    public void LoadFirstBeforeTrialScene()
    {
        if (GameManager.s_Singleton.trialDayNumber == 1)
        {
            LevelToLoad(firstBeforeTrialSceneId);
        }
        else if (GameManager.s_Singleton.trialDayNumber == 2)
        {
            LevelToLoad(secondBeforeTrialSceneId);
        }
    }
    #endregion

    #region BeforeTrialPhase
    //Pour la phase d'avant-procès uniquement
    public void SetInspectionStateToTrue()
    {
        UIManager_BeforeTrial.singleton.beforeTrialPhaseHasBegun = true;
    }

    public void SetInspectionStateToFalse()
    {
        UIManager_BeforeTrial.singleton.beforeTrialPhaseHasBegun = false;

    }

    public void LoadFirstTrialScene()
    {
        LevelToLoad(firstTrialSceneId);
    }

    public void LoadSecondTrialScene()
    {
        LevelToLoad(secondTrialSceneId);
    }
    #endregion

    #region TrialPhase
    //Pour la phase d'avant-procès uniquement
    public void ActiveFlowCharts()
    {
        foreach (GameObject obj in UIManagerTrial.s_Singleton.flowchartsGameObjects)
        {
            obj.SetActive(true);
        }
    }

    public void DeactivateFlowCharts()
    {
        foreach (GameObject obj in UIManagerTrial.s_Singleton.flowchartsGameObjects)
        {
            obj.SetActive(false);
        }
    }

    public void LoadExfiltrationScene()
    {
        LevelToLoad(exfiltrationSceneId);
    }
    #endregion

    #region Exfiltration
    public void DisplayShopWindow()
    {
        UIManager.s_Singleton.DisplayShopWindow();
    }

    #endregion

    #region Game State
    public void SetStateOfGameManagerToCinematicOrTransition()
    {
        GameManager.s_Singleton.gameState = GameState.CinematicOrTransition;
    }

    public void SetStateOfGameManagerToPlayMode()
    {
        GameManager.s_Singleton.gameState = GameState.PlayMode;
    }
    #endregion
}
