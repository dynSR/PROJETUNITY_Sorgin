using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerGlobalAvProces : MonoBehaviour
{
    public static UIManagerGlobalAvProces singleton;

    public TextMeshProUGUI titreProcesTxt;
    public int numeroProces;
    public string chefAccusation;

    public TextMeshProUGUI docActuelTxt;

    public GameObject finishWindow;

    private void Awake()
    {
        if (singleton)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        titreProcesTxt.text = chefAccusation + " - Proces n°" + numeroProces;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("PS4_Square") || Input.GetButtonDown("XBOX_X"))
        {
            ToogleFinishWindow();
        }
    }

    public void UIUpdateActualDoc(int actualDoc, int maxDoc)
    {
        docActuelTxt.text = "Preuve " + actualDoc + " sur " + maxDoc;
    }

    public void ToogleFinishWindow()
    {
        finishWindow.SetActive(!finishWindow.activeSelf);
    }

    public void ButtonGoToTrial()
    {
        SceneManager.LoadScene("SceneProces001");
    }
}
