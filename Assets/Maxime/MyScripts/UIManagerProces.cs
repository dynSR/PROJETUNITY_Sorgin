using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fungus;

public class UIManagerProces : MonoBehaviour
{
    private int nbGoodAnswers;
    public int nbMaxGoodAnswers;
    public TextMeshProUGUI txtGoodAnswers;
    public Flowchart mainFlowchart;

    // Start is called before the first frame update
    void Start()
    {
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
}
