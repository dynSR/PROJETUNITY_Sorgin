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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectionAmount = Mathf.Clamp(DetectionAmount, 0, 1);

        EyeFill.fillAmount = DetectionAmount;
    }
}
