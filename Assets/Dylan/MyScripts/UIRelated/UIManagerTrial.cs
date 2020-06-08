using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerTrial : DefaultUIManager
{
    [Header("FLOWCHARTS")]
    public GameObject[] flowchartsGameObjects;

    public static UIManagerTrial s_Singleton;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelChanger.s_Singleton.DeactivateFlowCharts();
        LevelChanger.s_Singleton.SetAnimatorTrigger("FadeOut");
        SetOSTStateAndPostEvent("state_Trial");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
