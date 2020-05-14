using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fungus;

public class UIManagerProces : MonoBehaviour
{
    public static int actualPointsNumber;

    private int nbGoodAnswers;
    private int nbMaxGoodAnswers;

    public Flowchart mainFlowchart;

    public GameObject endSection;

    // Start is called before the first frame update
    void Start()
    {
        nbMaxGoodAnswers = mainFlowchart.GetIntegerVariable("maxGoodAnswers");
        actualPointsNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PointsEarned()
    {
        actualPointsNumber += mainFlowchart.GetIntegerVariable("GoodAnswers") * 125;
    }

    public void GoToEscape()
    {
        SceneManager.LoadScene("SceneBuild");
    }
}
