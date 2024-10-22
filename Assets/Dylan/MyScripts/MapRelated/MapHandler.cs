﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField] private GameObject mapWindow;
    [SerializeField] private GameObject mapCamera;
    //DEBUG
    [SerializeField] private GameObject shopWindow;

    [SerializeField] private TextMeshProUGUI textToModifyOnExfiltrationEnd;
    [SerializeField] private GameObject objectToDisplayOnExfiltrationEnd;

    [SerializeField] private string displayingOrHidingMapWwiseEventSoundName;
    [SerializeField] private RectTransform cursorRectTransform;
    private Vector2 cursorRectTransformPos;

    public CursorHandler CursorHandlerScript;


    public bool mapIsDisplayed = false;
    private bool canDisplayOrHideMap = true;
    private bool dpadYIsPressed = false;
    private float dpadY;

    public static MapHandler s_Singleton;

    #region Singleton
    private void Awake()
    {
        if (s_Singleton != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            s_Singleton = this;
        }
    }
    #endregion

    private void Start()
    {
        HideMap();
        cursorRectTransformPos = cursorRectTransform.localPosition;
    }

    void Update()
    {
        //Debug.Log(Input.GetAxis("PS4_DPadVertical"));

        if (GameManager.s_Singleton.exfiltrationHasBegun && GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            CheckDpadYValue();

            #region Arrow Up Dpad
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
            #endregion
        }
        else if(!GameManager.s_Singleton.exfiltrationHasBegun && GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            CheckDpadYValue();

            #region Arrow Up Dpad
            if (canDisplayOrHideMap && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetAxis("PS4_DPadVertical") >= 0.75f || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetAxis("XBOX_DPadVertical") >= 0.75f))
            {
                //HideMap();
                canDisplayOrHideMap = false;
                LevelChanger.s_Singleton.LoadFirstBeforeTrialScene();
            }
            #endregion
        }
    }

    //Summary : Permet de vérifier si la flèche du haut est pressée par le joueur...
    void CheckDpadYValue()
    {
        if (ConnectedController.s_Singleton.PS4ControllerIsConnected)
            dpadY = Input.GetAxis("PS4_DPadVertical");

        else if (ConnectedController.s_Singleton.XboxControllerIsConnected)
            dpadY = Input.GetAxis("XBOX_DPadVertical");

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
    public void DisplayMap()
    {
        //Debug.Log("Display Map");
        UIManager.s_Singleton.UIWindowsDisplay(mapWindow);
        mapCamera.SetActive(true);

        //if(shopWindow.activeInHierarchy)
        //    UIManager.s_Singleton.UIWindowsHide(shopWindow);

        mapIsDisplayed = true;
        AkSoundEngine.PostEvent(displayingOrHidingMapWwiseEventSoundName, this.gameObject);

        cursorRectTransform.localPosition = cursorRectTransformPos;

        if (GameManager.s_Singleton.exfiltrationHasBegun)
        {
            textToModifyOnExfiltrationEnd.text = "Désafficher la carte";

            if(objectToDisplayOnExfiltrationEnd.activeInHierarchy)
                objectToDisplayOnExfiltrationEnd.SetActive(false);
        }
        else
        {
            textToModifyOnExfiltrationEnd.text = "Continuer";

            if (!objectToDisplayOnExfiltrationEnd.activeInHierarchy)
                objectToDisplayOnExfiltrationEnd.SetActive(true);
        }

        //Player.s_Singleton.LookAtMap = true;
    }

    //Summary : Permet de cacher la carte
    void HideMap()
    {
        CursorHandlerScript.Save();
        //Debug.Log("Hide Map");
        UIManager.s_Singleton.UIWindowsHide(mapWindow);
        mapCamera.SetActive(false);
        mapIsDisplayed = false;

        if(GameManager.s_Singleton.exfiltrationHasBegun)
            AkSoundEngine.PostEvent(displayingOrHidingMapWwiseEventSoundName, this.gameObject);
        //Player.s_Singleton.LookAtMap = false;
    }
}
