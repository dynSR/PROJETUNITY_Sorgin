using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField] private GameObject mapWindow;
    //DEBUG
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private GameObject debugButtons;

    [SerializeField] private string displayingOrHidingMapWwiseEventSoundName;
    [SerializeField] private RectTransform cursorRectTransform;
    private Vector2 cursorRectTransformPos;

    private bool mapIsDisplayed = false;
    private bool canDisplayOrHideMap = true;
    private bool dpadYIsPressed = false;
    private float dpadY;


    private void Start()
    {
        cursorRectTransformPos = cursorRectTransform.localPosition;
    }

    void Update()
    {
        //Debug.Log(Input.GetAxis("PS4_DPadVertical"));

        CheckDpadYValue();

        if (canDisplayOrHideMap && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetAxis("PS4_DPadVertical") >= 0.75f || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetAxis("XBOX_DPadVertical") >= 0.75f))
        {
            if (!mapIsDisplayed)
            {
                //Debug.Log("Display Map");
                DisplayMap();
                canDisplayOrHideMap = false;
            }
            else if (mapIsDisplayed)
            {
                //Debug.Log("Hide Map");
                HideMap();
                canDisplayOrHideMap = false;
            }
        }
    }

    //Summary : Permet de vérifier si la flèche du haut est pressée par le joueur...
    void CheckDpadYValue()
    {
        dpadY = Input.GetAxis("PS4_DPadVertical");
       
        //Si la valeur de cette input dépasse une certaine valeur alors...
        if (dpadY >= 0.75f) 
        { 
            //La touche est pressée.
            dpadYIsPressed = true;
        } 
        else 
        { 
            //Sinon, elle n'est pas pressée par le joueur...
            dpadYIsPressed = false;

            //Alors lejoueur peut de nouveau appuyer sur cette input pour moddifier l'état d'affichage de la carte.
            canDisplayOrHideMap = true;
        }

    }

    //Summary : Permet d'afficher la carte
    void DisplayMap()
    {
        //Debug.Log("Display Map");
        UIManager.s_Singleton.UIWindowsDisplay(mapWindow);
        UIManager.s_Singleton.UIWindowsHide(debugButtons);

        if(shopWindow.activeInHierarchy)
            UIManager.s_Singleton.UIWindowsHide(shopWindow);
        mapIsDisplayed = true;
        AkSoundEngine.PostEvent(displayingOrHidingMapWwiseEventSoundName, this.gameObject);

        cursorRectTransform.localPosition = cursorRectTransformPos;
    }

    //Summary : Permet de cacher la carte
    void HideMap()
    {
        //Debug.Log("Hide Map");
        UIManager.s_Singleton.UIWindowsHide(mapWindow);
        UIManager.s_Singleton.UIWindowsDisplay(debugButtons);
        mapIsDisplayed = false;
        AkSoundEngine.PostEvent(displayingOrHidingMapWwiseEventSoundName, this.gameObject);
    }
}
