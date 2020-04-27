using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField] private GameObject mapWindow;

    private bool mapIsDisplayed = false;
    private bool canDisplayOrHideMap = true;
    private bool dpadYIsPressed = false;
    public float dpadY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxis("PS4_DPadVertical"));

        CheckDpadYValue();

        if (Input.GetAxis("PS4_DPadVertical") >= 0.75f && canDisplayOrHideMap)
        {
            if (!mapIsDisplayed)
            {
                Debug.Log("Display Map");
                DisplayMap();
                canDisplayOrHideMap = false;
            }
            else if (mapIsDisplayed)
            {
                Debug.Log("Hide Map");
                HideMap();
                canDisplayOrHideMap = false;
            }
        }
    }

    void CheckDpadYValue()
    {
        dpadY = Input.GetAxis("PS4_DPadVertical");
       
        if (dpadY >= 0.75f) 
        { 
            dpadYIsPressed = true;
        } 
        else 
        { 
            dpadYIsPressed = false;
            canDisplayOrHideMap = true;
        }

    }


    void DisplayMap()
    {
        UIManager.s_Singleton.UIWindowsDisplay(mapWindow);
        mapIsDisplayed = true;
    }

    void HideMap()
    {
        UIManager.s_Singleton.UIWindowsHide(mapWindow);
        mapIsDisplayed = false;
    }
}
