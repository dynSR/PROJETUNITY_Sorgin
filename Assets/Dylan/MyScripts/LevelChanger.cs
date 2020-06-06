using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class LevelChanger : MonoBehaviour
{
    //[SerializeField] private Image fadeImage;
    //[Range(0,255)]
    //[SerializeField] private float alphaValue;
    private int levelToLoadId;

    private Animator animator;

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

        //fadeImage.color.a.Equals(alphaValue);

        animator = GetComponent<Animator>();
    }

    public void LevelToLoad(int levelId)
    {
        levelToLoadId = levelId;
        animator.SetTrigger("FadeIn");
    }

    public void FadeOutOnLevelLoaded(string triggerToSet)
    {
        animator.SetTrigger(triggerToSet);
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoadId);
    }

    //Pour la phase d'avant-procès uniquement
    public void SetInspectionStateToTrue()
    {
        UIManager_BeforeTrial.singleton.inspectionHasBegun = true;
    }

    public void SetInspectionStateToFalse()
    {
        UIManager_BeforeTrial.singleton.inspectionHasBegun = false;

    }
}
