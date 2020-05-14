using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpellsInventory : MonoBehaviour
{
    [Header("SPELL COMPARTMENT PARAMETERS")]
    public List<SpellCompartment> spellsCompartments;
    public GameObject spellActivationFeedback;
    [HideInInspector] public bool spellCompartmentIsActive = false;

    [Header("SPELL IN THE COMPARTMENT")]
    [SerializeField] private Animator playerAnimator;
    public Spell spellInSpellCompartment;
    public bool playerIsTranformedInMouse = false;
    public bool playerIsTranformedInCat = false;
    public bool playerIsInHumanForm = true;
    float _durationOfEffectSinceLaunched = 0;
    

    [Header("PLAYER MODELS")]
    //public Transform playerCharacter;
    public GameObject defaultCharacterModel;
    public GameObject mouseCharacterModel;
    public GameObject catCharacterModel;

    public static PlayerSpellsInventory s_Singleton;

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
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            #region L1/LB
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_L1") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_LB"))
            {
                Debug.Log("L1 pressed");
                SwitchSpellsInPlayerInventory();
            }
            #endregion

            #region L2/LT
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_L2") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_LT"))
            {
                if (PlayerObjectsInventory.s_Singleton.objectCompartmentIsActive)
                {
                    PlayerObjectsInventory.s_Singleton.DeactivateObjectActivationFeedback();
                }
                Debug.Log("L2 pressed");
                ToggleSpellActivationFeedback();
            }
            #endregion

            #region Square/X
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X"))
            {
                Debug.Log("Square pressed");
                ActiveSpellInSpellCompartment();
            }
            #endregion

            #region Transformation Duration
            if (playerIsTranformedInCat || playerIsTranformedInMouse)
            {
                _durationOfEffectSinceLaunched -= Time.deltaTime;

                if (_durationOfEffectSinceLaunched <= 1.5f)
                {
                    Debug.Log("End of transformation");
                    playerAnimator.SetBool("EndOfTransformation", true);
                }
                if (_durationOfEffectSinceLaunched <= 0)
                {
                    Debug.Log("Transformation into human");
                    if(playerIsTranformedInCat)
                    {
                        HumanTransformation(catCharacterModel, defaultCharacterModel);
                    }
                    else if (playerIsTranformedInMouse)
                    {
                        HumanTransformation(mouseCharacterModel, defaultCharacterModel);
                    }
                }  
            }
            #endregion
        }
    }

    private void UseTheSpellInSpellCompartment()
    {
        switch (spellInSpellCompartment.spellType)
        {
            case Spell.SpellType.Etourdissement:
                break;
            case Spell.SpellType.Crochetage:
                break;
            case Spell.SpellType.Radar:
                break;
            case Spell.SpellType.Duplication:
                break;
            case Spell.SpellType.Clone:
                break;
            case Spell.SpellType.TransformationEnSouris:
                if (playerIsInHumanForm) MouseTransformation(defaultCharacterModel, mouseCharacterModel);
                else if (playerIsTranformedInCat) MouseTransformation(catCharacterModel, mouseCharacterModel);
                break;
            case Spell.SpellType.TransformationEnChat:
                if (playerIsInHumanForm) CatTransformation(defaultCharacterModel, catCharacterModel);
                else if (playerIsTranformedInCat) CatTransformation(mouseCharacterModel, catCharacterModel);
                break;
            default:
                break;
        }

    }

    public void CatTransformation(GameObject objToDisactive, GameObject objToActive)
    {
        Debug.Log("Trying to transform the player character in a cat...");

        objToDisactive.SetActive(false);
        objToActive.SetActive(true);

        // Suppression - Instance
        //GameObject activeCharacterModel = activePlayerCharacter.GetChild(0).gameObject;
        //Destroy(activeCharacterModel);

        //GameObject modelToSwitchTo = Instantiate(newPlayerCharacterModel, activePlayerCharacter) as GameObject;
        //modelToSwitchTo.transform.SetParent(activePlayerCharacter);

        playerIsTranformedInCat = true;
        playerIsTranformedInMouse = false;
        playerIsInHumanForm = false;
    }

    public void MouseTransformation(GameObject objToDisactive, GameObject objToActive)
    {
        Debug.Log("Trying to transform the player character in a mouse...");

        objToDisactive.SetActive(false);
        objToActive.SetActive(true);

        // Suppression - Instance
        //GameObject activeCharacterModel = activePlayerCharacter.GetChild(0).gameObject;
        //Destroy(activeCharacterModel);

        //GameObject modelToSwitchTo = Instantiate(newPlayerCharacterModel, activePlayerCharacter) as GameObject;
        //modelToSwitchTo.transform.SetParent(activePlayerCharacter);

        playerIsTranformedInMouse = true;
        playerIsTranformedInCat = false;
        playerIsInHumanForm = false;
    }

    private void HumanTransformation(GameObject objToDisactive, GameObject objToActive)
    {
        Debug.Log("Trying to transform the player character in a human...");

        objToDisactive.SetActive(false);
        objToActive.SetActive(true);

        // Suppression - Instance
        //GameObject activeCharacterModel = activePlayerCharacter.GetChild(0).gameObject;
        //Destroy(activeCharacterModel);

        //GameObject modelToSwitchTo = Instantiate(newPlayerCharacterModel, activePlayerCharacter) as GameObject;
        //modelToSwitchTo.transform.SetParent(activePlayerCharacter);

        playerIsTranformedInMouse = false;
        playerIsTranformedInCat = false;
        playerIsInHumanForm = true;
        playerAnimator.SetBool("EndOfTransformation", false);
    }

    //Summary : Permet de gérer le switch des sort équipés
    #region Spells Management
    void ActiveSpellInSpellCompartment()
    {
        if (spellCompartmentIsActive && spellsCompartments[0].MyCompartmentSpell != null)
        {
            Debug.Log("Trying To use the spell");
            spellInSpellCompartment = spellsCompartments[0].MyCompartmentSpell;
            _durationOfEffectSinceLaunched = spellsCompartments[0].MyCompartmentSpell.MySpellDurationOfEffect;
            UseTheSpellInSpellCompartment();

            spellsCompartments[0].MyCompartmentSpell = null;
            

            DisableImageCompotent(spellsCompartments[0].GetComponent<Image>());

            DeactivateSpellActivationFeedback();
            ShopManager.s_Singleton.amntOfSpellBought--;
        }
    }

    //Summary : Permet d'afficher ou désafficher le feedback d'activation des sorts en fonction de l'appui Input.
    public void ToggleSpellActivationFeedback()
    {
        spellActivationFeedback.SetActive(!spellActivationFeedback.activeSelf);
        spellCompartmentIsActive = !spellCompartmentIsActive;
    }

    //Summary : Désaffiche le feedback d'activation des sorts
    public void DeactivateSpellActivationFeedback()
    {
        spellActivationFeedback.SetActive(false);
        spellCompartmentIsActive = false;
    }

    void SwitchSpellsInPlayerInventory()
    {
        // Check si les compartiments possèdent un spell renseigné
        if (ShopManager.s_Singleton.amntOfSpellBought == 0)
            return;

        else if (ShopManager.s_Singleton.amntOfSpellBought == 1)
        {
            //Si premier emplacement n'est pas vide --> dernier emplacement
            if (spellsCompartments[0].MyCompartmentSpell != null)
            {
                //Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

                //Swap
                SwapSpellInSpellCompartment(spellsCompartments[2], spellsCompartments[0]);

                //Enable and change Image
                SwapImageSprite(spellsCompartments[2].GetComponent<Image>(), spellsCompartments[0].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[0].GetComponent<Image>());
                ResetSpellInSpellCompartment(spellsCompartments[0]);
            }

            //Sinon si troisième emplacement n'est pas vide --> deuxième emplacement
            else if (spellsCompartments[2].MyCompartmentSpell != null)
            {
                //Debug.Log("troisième emplacement n'est pas vide --> deuxième emplacement");

                //Swap
                SwapSpellInSpellCompartment(spellsCompartments[1], spellsCompartments[2]);

                //Enable and change Image
                SwapImageSprite(spellsCompartments[1].GetComponent<Image>(), spellsCompartments[2].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[2].GetComponent<Image>());
                ResetSpellInSpellCompartment(spellsCompartments[2]);
            }

            //Sinon si deuxième emplacement n'est pas vide --> premier emplacement
            else if (spellsCompartments[1].MyCompartmentSpell != null)
            {
                //Debug.Log("deuxième emplacement n'est pas vide --> premier emplacement");

                //Swap
                SwapSpellInSpellCompartment(spellsCompartments[0], spellsCompartments[1]);

                //Enable and change Image
                SwapImageSprite(spellsCompartments[0].GetComponent<Image>(), spellsCompartments[1].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[1].GetComponent<Image>());
                ResetSpellInSpellCompartment(spellsCompartments[1]);
            }

        }

        else if (ShopManager.s_Singleton.amntOfSpellBought == 2)
        {
            //Si premier emplacement n'est pas vide --> dernier emplacement
            if (spellsCompartments[0].MyCompartmentSpell != null && spellsCompartments[1].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

                //SWAP
                SwapSpellInSpellCompartment(spellsCompartments[2], spellsCompartments[0]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[2].GetComponent<Image>(), spellsCompartments[0].MyCompartmentSpell.MySpellIcon);

                //SWAP
                SwapSpellInSpellCompartment(spellsCompartments[0], spellsCompartments[1]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[0].GetComponent<Image>(), spellsCompartments[1].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[1].GetComponent<Image>());
                //Reset previous spell
                ResetSpellInSpellCompartment(spellsCompartments[1]);
            }

            else if (spellsCompartments[0].MyCompartmentSpell != null && spellsCompartments[2].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

                //SWAP
                SwapSpellInSpellCompartment(spellsCompartments[1], spellsCompartments[2]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[1].GetComponent<Image>(), spellsCompartments[2].MyCompartmentSpell.MySpellIcon);

                //SWAP
                SwapSpellInSpellCompartment(spellsCompartments[2], spellsCompartments[0]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[2].GetComponent<Image>(), spellsCompartments[0].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[0].GetComponent<Image>());
                //Reset previous spell
                ResetSpellInSpellCompartment(spellsCompartments[0]);
            }

            else if (spellsCompartments[1].MyCompartmentSpell != null && spellsCompartments[2].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

                //SWAP
                SwapSpellInSpellCompartment(spellsCompartments[0], spellsCompartments[1]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[0].GetComponent<Image>(), spellsCompartments[1].MyCompartmentSpell.MySpellIcon);

                //SWAP
                SwapSpellInSpellCompartment(spellsCompartments[1], spellsCompartments[2]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[1].GetComponent<Image>(), spellsCompartments[2].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[2].GetComponent<Image>());
                //Reset previous spell
                ResetSpellInSpellCompartment(spellsCompartments[2]);
            }
        }

        else if (ShopManager.s_Singleton.amntOfSpellBought == 3)
        {
            Spell _spellCompartment00 = spellsCompartments[0].MyCompartmentSpell;
            Spell _spellCompartment01 = spellsCompartments[1].MyCompartmentSpell;
            Spell _spellCompartment02 = spellsCompartments[2].MyCompartmentSpell;

            Sprite _spriteSpellCompartment00 = spellsCompartments[0].GetComponent<Image>().sprite;
            Sprite _spriteSpellCompartment01 = spellsCompartments[1].GetComponent<Image>().sprite;
            Sprite _spriteSpellCompartment02 = spellsCompartments[2].GetComponent<Image>().sprite;

            spellsCompartments[0].MyCompartmentSpell = _spellCompartment01;
            SwapImageSprite(spellsCompartments[0].GetComponent<Image>(), _spriteSpellCompartment01);

            spellsCompartments[2].MyCompartmentSpell = _spellCompartment00;
            SwapImageSprite(spellsCompartments[2].GetComponent<Image>(), _spriteSpellCompartment00);

            spellsCompartments[1].MyCompartmentSpell = _spellCompartment02;
            SwapImageSprite(spellsCompartments[1].GetComponent<Image>(), _spriteSpellCompartment02);
        }
    }

    void ResetSpellInSpellCompartment(SpellCompartment spellCompartmentToReset)
    {
        spellCompartmentToReset.MyCompartmentSpell = null;
    }

    void SwapSpellInSpellCompartment(SpellCompartment spellToChange, SpellCompartment wantedSpell)
    {
        spellToChange.MyCompartmentSpell = wantedSpell.MyCompartmentSpell;
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
