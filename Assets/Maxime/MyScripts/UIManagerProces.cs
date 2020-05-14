using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fungus;

public class UIManagerProces : MonoBehaviour
{
    private int nbGoodAnswers;
    private int nbMaxGoodAnswers;
    public TextMeshProUGUI txtGoodAnswers;
    public TextMeshProUGUI txtPointsEarned;
    public Flowchart mainFlowchart;
    public GameObject endSection;

    // Start is called before the first frame update
    void Start()
    {
        nbMaxGoodAnswers = mainFlowchart.GetIntegerVariable("MaxAnswers");
        txtGoodAnswers.text = "0 / " + nbMaxGoodAnswers.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoodAnswersTxtUpdate()
    {
        nbGoodAnswers = mainFlowchart.GetIntegerVariable("GoodAnswers");
        txtGoodAnswers.text = nbGoodAnswers.ToString() + " / " + nbMaxGoodAnswers.ToString();
    }

    public void PointsEarned()
    {
        endSection.SetActive(true);
        int pointsEarned = mainFlowchart.GetIntegerVariable("GoodAnswers") * 100;
        txtPointsEarned.text = "Vous avez obtenu " + pointsEarned + " points de bénédiction.";
    }

    public void GoToEscapeScene()
    {
        SceneManager.LoadScene("SceneBuild");
    }

    public void ButtonRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ButtonMain()
    {
        SceneManager.LoadScene("SBLobbyScene");
    }
}
