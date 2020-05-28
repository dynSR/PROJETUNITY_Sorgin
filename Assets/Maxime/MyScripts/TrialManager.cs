using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fungus;

public class TrialManager : MonoBehaviour
{
    private int actualPointsNumber;

    private int nbGoodAnswers;
    private int nbMaxGoodAnswers;

    public Flowchart mainFlowchart;

    // Start is called before the first frame update
    void Start()
    {
        nbMaxGoodAnswers = 0;
        nbGoodAnswers = 0;
        actualPointsNumber = 0;
    }

    public void GoToEscape()
    {
        actualPointsNumber += mainFlowchart.GetIntegerVariable("goodAnswers") * 250;
        GameManager.s_Singleton.playerPointsValue = actualPointsNumber;
        SceneManager.LoadScene("SceneBuild");
    }
}
