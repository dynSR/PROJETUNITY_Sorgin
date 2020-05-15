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
    
    public OppeningDoor doorNearPlayerCharacter;

    [Header("WWISE SOUND EVENT NAME")]
    [SerializeField] private string oppeningADoorSFX;
    
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
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_R2") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_RT"))
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
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X"))
            {
                Debug.Log("Square pressed");
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
        if(necessaryObjectID == objectInObjectCompartment.MyObjectID)
        {
            if (doorNearPlayerCharacter != null)
            {
                if (necessaryObjectID == 1 && doorNearPlayerCharacter.doorType == DoorType.CommonDoor && doorNearPlayerCharacter.doorIsLocked)
                {
                    UseObject();
                }
                else if (necessaryObjectID == 2 && doorNearPlayerCharacter.doorType == DoorType.LastDoor && doorNearPlayerCharacter.doorIsLocked)
                {
                    UseObject();
                }
                else
                {
                    PlayerDoesNotHaveTheNecessaryObject();
                }
            }
        }
    }

    void PlayerDoesNotHaveTheNecessaryObject()
    {
        StartCoroutine(UIManager.s_Singleton.FadeInAndOutObjectFeedBack(UIManager.s_Singleton.cantUseAnObjectFeedback));
        DeactivateObjectActivationFeedback();
    }

    //Summary : Permet de gérer le switch des sort équipés
    #region Objects Management
    void UseObject()
    {
        if (objectInObjectCompartment.objectType == Object.ObjectType.Key)
        {
            doorNearPlayerCharacter.UnlockDoor();
            AkSoundEngine.PostEvent(oppeningADoorSFX, doorNearPlayerCharacter.transform.gameObject);
        }

        objectsCompartments[0].MyCompartmentObject = null;

        DisableImageCompotent(objectsCompartments[0].GetComponent<Image>());

        DeactivateObjectActivationFeedback();
        numberOfObjectInInventory--;
    }

    //Summary : Permet d'afficher ou désafficher le feedback d'activation des sorts en fonction de l'appui Input.
    public void ToggleObjectActivationFeedback()
    {
        objectActivationFeedback.SetActive(!objectActivationFeedback.activeSelf);
        objectCompartmentIsActive = !objectCompartmentIsActive;
    }

    //Summary : Désaffiche le feedback d'activation des sorts
    public void DeactivateObjectActivationFeedback()
    {
        objectActivationFeedback.SetActive(false);
        objectCompartmentIsActive = false;
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
            Object _spellCompartment00 = objectsCompartments[0].MyCompartmentObject;
            Object _spellCompartment01 = objectsCompartments[1].MyCompartmentObject;
            Object _spellCompartment02 = objectsCompartments[2].MyCompartmentObject;

            Sprite _spriteSpellCompartment00 = objectsCompartments[0].GetComponent<Image>().sprite;
            Sprite _spriteSpellCompartment01 = objectsCompartments[1].GetComponent<Image>().sprite;
            Sprite _spriteSpellCompartment02 = objectsCompartments[2].GetComponent<Image>().sprite;

            objectsCompartments[0].MyCompartmentObject = _spellCompartment01;
            SwapImageSprite(objectsCompartments[0].GetComponent<Image>(), _spriteSpellCompartment01);

            objectsCompartments[2].MyCompartmentObject = _spellCompartment00;
            SwapImageSprite(objectsCompartments[2].GetComponent<Image>(), _spriteSpellCompartment00);

            objectsCompartments[1].MyCompartmentObject = _spellCompartment02;
            SwapImageSprite(objectsCompartments[1].GetComponent<Image>(), _spriteSpellCompartment02);
        }
    }

    void ResetObjectInObjectCompartment(ObjectCompartment spellCompartmentToReset)
    {
        spellCompartmentToReset.MyCompartmentObject = null;
    }

    void SwapObjectInObjectCompartment(ObjectCompartment spellToChange, ObjectCompartment wantedSpell)
    {
        spellToChange.MyCompartmentObject = wantedSpell.MyCompartmentObject;
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
