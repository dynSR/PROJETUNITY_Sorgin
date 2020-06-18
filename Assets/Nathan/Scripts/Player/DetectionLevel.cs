using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DetectionLevel : MonoBehaviour
{

    #region SINGLETON
    private static DetectionLevel _instance;
    public static DetectionLevel Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion  

    public Image EyeFill;

    public float DetectionAmount;

    public string Ennemi;

    public GameObject ParentToDisable;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DetectionAmount <= 0.05f)
        {
            ParentToDisable.SetActive(false);
        }
        if (DetectionAmount > 0.05f)
        {
            ParentToDisable.SetActive(true);
        }
    }

    public void Detection(string EnnemiActuel, float DetectionValue)
    {
        
        if (DetectionAmount < DetectionValue)
        {
            Ennemi = EnnemiActuel;
            DetectionAmount = DetectionValue;

            EyeFill.fillAmount = DetectionAmount;
        }

        if (EnnemiActuel == Ennemi)
        {
            DetectionAmount = DetectionValue;

            EyeFill.fillAmount = DetectionAmount;
        }
    }
}
