using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManagerSB : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonProces()
    {
        SceneManager.LoadScene("SBSceneProces");
    }

    public void ButtonShop()
    {
        SceneManager.LoadScene("00_SceneTest");
    }

    public void ButtonExfiltration()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }
}
