using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class LevelChanger : MonoBehaviour
{
    private int levelToLoadId;

    public Animator animator;

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

    public void LevelToLoad(int levelId)
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
    #endregion

    public void SetStateOfGameManagerToCinematicOrTransition()
    {
        GameManager.s_Singleton.gameState = GameState.CinematicOrTransition;
    }

    public void SetStateOfGameManagerToPlayMode()
    {
        GameManager.s_Singleton.gameState = GameState.PlayMode;
    }
}
