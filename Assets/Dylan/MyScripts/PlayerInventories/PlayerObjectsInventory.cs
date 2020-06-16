using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObjectsInventory : MonoBehaviour
{
    [Header("OBJECT COMPARTMENT PARAMETERS")]
    public List<ObjectCompartment> objectsCompartments;
    [HideInInspector] public bool objectCompartmentIsActive = false;
    public GameObject objectActivationFeedback;
    public Object objectInObjectCompartment;

    public int numberOfObjectInInventory = 0;
    
    public static PlayerObjectsInventory s_Singleton;

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

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            #region R1/RB
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_R1") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_RB"))
            {
                Debug.Log("R1 pressed");
                SwitchObjectsInPlayerInventory();
            }
            #endregion

            #region R2/RT
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetAxis("PS4_R2") >= 0.5f || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetAxis("XBOX_RT") >= 0.5f)
            {
                if (PlayerSpellsInventory.s_Singleton.spellCompartmentIsActive)
                {
                    PlayerSpellsInventory.s_Singleton.DeactivateSpellActivationFeedback();
                }
                Debug.Log("R2 pressed");
                ToggleObjectActivationFeedback();
            }
            #endregion

            #region Square/X
            if (objectCompartmentIsActive && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X")))
            {
                Debug.Log("Square pressed");
                if (!Player.s_Singleton.playerIsInHumanForm)
                {
                    CantUseTheObject();
                    return;
                }
                if (objectCompartmentIsActive && objectsCompartments[0].MyCompartmentObject != null)
                {
                    objectInObjectCompartment = objectsCompartments[0].MyCompartmentObject;
                    TryToUseTheObject();
                }
                    
            }
            #endregion
        }
    }

    public void TryToUseTheObject()
    {
        switch (objectInObjectCompartment.objectType)
        {
            case Object.ObjectType.Stone:
                break;
            case Object.ObjectType.Key:
                if (objectInObjectCompartment.MyObjectID == 1)
                {
                    CheckIfPlayerHasTheNecessaryObject(1);
                }
                else if (objectInObjectCompartment.MyObjectID == 2)
                {
                    CheckIfPlayerHasTheNecessaryObject(2);
                }
                else
                {
                    PlayerDoesNotHaveTheNecessaryObject();
                }
                break;
            case Object.ObjectType.Bottle:
                break;
            default:
                break;
        }
    }

    void CheckIfPlayerHasTheNecessaryObject(int necessaryObjectID)
    {
        OppeningDoor doorNearPlayerCharacter = Player.s_Singleton.doorNearPlayerCharacter;

        if (necessaryObjectID == objectInObjectCompartment.MyObjectID)
        {
            if (doorNearPlayerCharacter != null)
            {
                if (necessaryObjectID == 1 && doorNearPlayerCharacter.doorType == DoorType.CommonDoor && doorNearPlayerCharacter.doorIsLocked)
                {
                    objectInObjectCompartment.UseObject();
                    UseObjectInObjectCompartment();
                }
                else if (necessaryObjectID == 2 && doorNearPlayerCharacter.doorType == DoorType.LastDoor && doorNearPlayerCharacter.doorIsLocked)
                {
                    objectInObjectCompartment.UseObject();
                    UseObjectInObjectCompartment();
                }
                else
                {
                    PlayerDoesNotHaveTheNecessaryObject();
                }
            }
            else
            {
                CantUseTheObject();
            }
        }
    }

    void PlayerDoesNotHaveTheNecessaryObject()
    {
        StartCoroutine(UIManager.s_Singleton.FadeInAndOutObjectFeedBack(UIManager.s_Singleton.cantUseAnObjectFeedback));
        DeactivateObjectActivationFeedback();
    }

    public void CantUseTheObject()
    {
        StartCoroutine(UIManager.s_Singleton.FadeInAndOutObjectFeedBack(UIManager.s_Singleton.cantUseAnObjectFeedback));
        DeactivateObjectActivationFeedback();
    }

    //Summary : Permet de gérer le switch des sort équipés
    #region Objects Management
    void UseObjectInObjectCompartment()
    {
        OppeningDoor doorNearPlayerCharacter = Player.s_Singleton.doorNearPlayerCharacter;

        objectsCompartments[0].MyCompartmentObject = null;

        DisableImageCompotent(objectsCompartments[0].GetComponent<Image>());

        DeactivateObjectActivationFeedback();
        numberOfObjectInInventory--;
    }

    private void ChangeObjectCompartmentInputLandmark()
    {
        UIManager.s_Singleton.objectCompartmentInputActionText.text = UIManager.s_Singleton.objectCompartmentIsActive;
        UIManager.s_Singleton.objectCompartmentInputIcon.sprite = UIManager.s_Singleton.objectCompartmentInputSprites[1];
    }

    private void ResetObjectCompartmentInputLandmark()
    {
        UIManager.s_Singleton.objectCompartmentInputActionText.text = UIManager.s_Singleton.objectCompartmentIsNotActive;
        UIManager.s_Singleton.objectCompartmentInputIcon.sprite = UIManager.s_Singleton.objectCompartmentInputSprites[0];
    }

    //Summary : Permet d'afficher ou désafficher le feedback d'activation des sorts en fonction de l'appui Input.
    public void ToggleObjectActivationFeedback()
    {
        objectActivationFeedback.SetActive(!objectActivationFeedback.activeSelf);
        objectCompartmentIsActive = !objectCompartmentIsActive;

        if(objectActivationFeedback.activeInHierarchy)
            ChangeObjectCompartmentInputLandmark();
        else
            ResetObjectCompartmentInputLandmark();
    }

    //Summary : Désaffiche le feedback d'activation des sorts
    public void DeactivateObjectActivationFeedback()
    {
        objectActivationFeedback.SetActive(false);
        objectCompartmentIsActive = false;

        ResetObjectCompartmentInputLandmark();
    }

    void SwitchObjectsInPlayerInventory()
    {
        // Check si les compartiments possèdent un spell renseigné
        if (numberOfObjectInInventory == 0)
            return;

        else if (numberOfObjectInInventory == 1)
        {
            //Si premier emplacement n'est pas vide --> dernier emplacement
            if (objectsCompartments[0].MyCompartmentObject != null)
            {
                //Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

                //Swap
                SwapObjectInObjectCompartment(objectsCompartments[2], objectsCompartments[0]);

                //Enable and change Image
                SwapImageSprite(objectsCompartments[2].GetComponent<Image>(), objectsCompartments[0].MyCompartmentObject.MyObjectIcon);

                //Disable and reset previous image
                DisableImageCompotent(objectsCompartments[0].GetComponent<Image>());
                ResetObjectInObjectCompartment(objectsCompartments[0]);
            }

            //Sinon si troisième emplacement n'est pas vide --> deuxième emplacement
            else if (objectsCompartments[2].MyCompartmentObject != null)
            {
                //Debug.Log("troisième emplacement n'est pas vide --> deuxième emplacement");

                //Swap
                SwapObjectInObjectCompartment(objectsCompartments[1], objectsCompartments[2]);

                //Enable and change Image
                SwapImageSprite(objectsCompartments[1].GetComponent<Image>(), objectsCompartments[2].MyCompartmentObject.MyObjectIcon);

                //Disable and reset previous image
                DisableImageCompotent(objectsCompartments[2].GetComponent<Image>());
                ResetObjectInObjectCompartment(objectsCompartments[2]);
            }

            //Sinon si deuxième emplacement n'est pas vide --> premier emplacement
            else if (objectsCompartments[1].MyCompartmentObject != null)
            {
                //Debug.Log("deuxième emplacement n'est pas vide --> premier emplacement");

                //Swap
                SwapObjectInObjectCompartment(objectsCompartments[0], objectsCompartments[1]);

                //Enable and change Image
                SwapImageSprite(objectsCompartments[0].GetComponent<Image>(), objectsCompartments[1].MyCompartmentObject.MyObjectIcon);

                //Disable and reset previous image
                DisableImageCompotent(objectsCompartments[1].GetComponent<Image>());
                ResetObjectInObjectCompartment(objectsCompartments[1]);
            }

        }

        else if (numberOfObjectInInventory == 2)
        {
            //Si premier emplacement n'est pas vide --> dernier emplacement
            if (objectsCompartments[0].MyCompartmentObject != null && objectsCompartments[1].MyCompartmentObject != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

                //SWAP
                SwapObjectInObjectCompartment(objectsCompartments[2], objectsCompartments[0]);
                //Enable and change Image
                SwapImageSprite(objectsCompartments[2].GetComponent<Image>(), objectsCompartments[0].MyCompartmentObject.MyObjectIcon);

                //SWAP
                SwapObjectInObjectCompartment(objectsCompartments[0], objectsCompartments[1]);
                //Enable and change Image
                SwapImageSprite(objectsCompartments[0].GetComponent<Image>(), objectsCompartments[1].MyCompartmentObject.MyObjectIcon);

                //Disable and reset previous image
                DisableImageCompotent(objectsCompartments[1].GetComponent<Image>());
                //Reset previous spell
                ResetObjectInObjectCompartment(objectsCompartments[1]);
            }

            else if (objectsCompartments[0].MyCompartmentObject != null && objectsCompartments[2].MyCompartmentObject != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

                //SWAP
                SwapObjectInObjectCompartment(objectsCompartments[1], objectsCompartments[2]);
                //Enable and change Image
                SwapImageSprite(objectsCompartments[1].GetComponent<Image>(), objectsCompartments[2].MyCompartmentObject.MyObjectIcon);

                //SWAP
                SwapObjectInObjectCompartment(objectsCompartments[2], objectsCompartments[0]);
                //Enable and change Image
                SwapImageSprite(objectsCompartments[2].GetComponent<Image>(), objectsCompartments[0].MyCompartmentObject.MyObjectIcon);

                //Disable and reset previous image
                DisableImageCompotent(objectsCompartments[0].GetComponent<Image>());
                //Reset previous spell
                ResetObjectInObjectCompartment(objectsCompartments[0]);
            }

            else if (objectsCompartments[1].MyCompartmentObject != null && objectsCompartments[2].MyCompartmentObject != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

                //SWAP
                SwapObjectInObjectCompartment(objectsCompartments[0], objectsCompartments[1]);
                //Enable and change Image
                SwapImageSprite(objectsCompartments[0].GetComponent<Image>(), objectsCompartments[1].MyCompartmentObject.MyObjectIcon);

                //SWAP
                SwapObjectInObjectCompartment(objectsCompartments[1], objectsCompartments[2]);
                //Enable and change Image
                SwapImageSprite(objectsCompartments[1].GetComponent<Image>(), objectsCompartments[2].MyCompartmentObject.MyObjectIcon);

                //Disable and reset previous image
                DisableImageCompotent(objectsCompartments[2].GetComponent<Image>());
                //Reset previous spell
                ResetObjectInObjectCompartment(objectsCompartments[2]);
            }
        }

        else if (numberOfObjectInInventory == 3)
        {
            Object _objCompartment00 = objectsCompartments[0].MyCompartmentObject;
            Object _objCompartment01 = objectsCompartments[1].MyCompartmentObject;
            Object _objCompartment02 = objectsCompartments[2].MyCompartmentObject;

            Sprite _spriteObjCompartment00 = objectsCompartments[0].GetComponent<Image>().sprite;
            Sprite _spriteObjCompartment01 = objectsCompartments[1].GetComponent<Image>().sprite;
            Sprite _spriteObjCompartment02 = objectsCompartments[2].GetComponent<Image>().sprite;

            objectsCompartments[0].MyCompartmentObject = _objCompartment01;
            SwapImageSprite(objectsCompartments[0].GetComponent<Image>(), _spriteObjCompartment01);

            objectsCompartments[2].MyCompartmentObject = _objCompartment00;
            SwapImageSprite(objectsCompartments[2].GetComponent<Image>(), _spriteObjCompartment00);

            objectsCompartments[1].MyCompartmentObject = _objCompartment02;
            SwapImageSprite(objectsCompartments[1].GetComponent<Image>(), _spriteObjCompartment02);
        }
    }

    void ResetObjectInObjectCompartment(ObjectCompartment objectCompartmentToReset)
    {
        objectCompartmentToReset.MyCompartmentObject = null;
    }

    void SwapObjectInObjectCompartment(ObjectCompartment objectToChange, ObjectCompartment wantedObject)
    {
        objectToChange.MyCompartmentObject = wantedObject.MyCompartmentObject;
    }

    void SwapImageSprite(Image imageToChange, Sprite wantedImageSprite)
    {
        if (!imageToChange.enabled)
            imageToChange.enabled = true;

        imageToChange.sprite = wantedImageSprite;
    }

    void DisableImageCompotent(Image imageToDisable)
    {
        imageToDisable.enabled = false;
        imageToDisable.sprite = null;
    }
    #endregion

}
