using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : DefaultUIManager
{
    [Header("PLAYER POINTS TEXT")]
    [SerializeField] private TextMeshProUGUI playerPointsValueText;

    [Header("SPELL COMPARTMENT PARAMETERS")]
    public List<SpellCompartment> spellsCompartments;
    public GameObject spellActivationFeedback;
    private bool spellCompartmentIsActive = false;

    [Header("PURCHASE VALIDATION POPUP PARAMETERS")]
    [SerializeField] private CanvasGroup validationPopupWindow;
    [SerializeField] private GameObject validationPopupButtonLayout;
    [HideInInspector]
    public bool validationPopupIsDisplayed = false;

    public static UIManager s_Singleton;

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
        //Initialisation des points du joueur au start
        SetPlayerPointsCountValue();
    }

    public override void Update()
    {
        base.Update();

        if(GameManager.s_Singleton.gameStates == GameState.PlayMode)
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
                Debug.Log("L2 pressed");
                ToggleSpellActivationFeedback();
            }
            #endregion

            #region Square/X
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X"))
            {
                Debug.Log("Square pressed");
                UseSpell();
            }
            #endregion

            #region Circle/B
            if (validationPopupIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_O") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_B")))
            {
                Debug.Log("Circle pressed");
                HideValidationPopup();
            }
            #endregion
        }
        
    }

    //Summary : Affiche la fenêtre de confirmation d'achat lorsque le joueur appuie sur un bouton contenant un sort qu'il peut acheter.
    public void DisplayValidationPopup()
    {
        //Affichage de la fenêtre de confirmation d'achat en Fade-In
        StartCoroutine(FadeCanvasGroup(validationPopupWindow, validationPopupWindow.alpha, 1, fadeDuration));
        validationPopupWindow.blocksRaycasts = true;

        //Réactivation des boutons contenus dans cette fenêtre (prévient les problèmes liés à la navigation de l'Event System)
        foreach (Button _buttons in validationPopupButtonLayout.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = true;
        }

        //Reset du premier objet sélectionné par l'Event System et initialisation du nouveau premier objet sélectionné sur "Non" (prévient l'appuie "SPAM")
        ResetEventSystemFirstSelectedGameObjet(validationPopupButtonLayout.transform.GetChild(1).gameObject);

        validationPopupIsDisplayed = true;
    }

    //Summary : Permet de désafficher la fenêtre de confirmation d'achat 
    public void HideValidationPopup()
    {
        StartCoroutine(FadeCanvasGroup(validationPopupWindow, validationPopupWindow.alpha, 0, fadeDuration));
        validationPopupWindow.blocksRaycasts = false;

        foreach (Button _buttons in validationPopupWindow.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = false;
        }

        PurchaseASpell purchasedSpell = validationPopupWindow.GetComponentInChildren<PurchaseASpell>();
        ResetEventSystemFirstSelectedGameObjet(purchasedSpell.selectedButton.gameObject);

        validationPopupIsDisplayed = false;
    }


    //Summary : Permet d'attribuer un nombre de points au joueur. Dès que le joueur reçoit de nouveaux points, on vérifie si de nouveaux sort sont maintenant achetables.
    public void AddPointsToPlayerScore(int valueToAdd)
    {
        GameManager.s_Singleton.playerPointsValue += valueToAdd;
        SetPlayerPointsCountValue();

        foreach (GameObject obj in ShopManager.s_Singleton.spellsAvailableInShop)
        {
            obj.GetComponent<ShopButton>().CheckIfPlayerCanPurchaseASpell(GameManager.s_Singleton.playerPointsValue);
        }
    }

    //Summary : Permet de set up la valeur du sort qui est en train d'être acheté + Opération de soustraction de celle-ci.
    public void SetValueToSubstract(int valueToSubstract)
    {
        int tempPlayerPointsValue = GameManager.s_Singleton.playerPointsValue;
        tempPlayerPointsValue -= valueToSubstract;

        foreach (GameObject obj in ShopManager.s_Singleton.spellsAvailableInShop)
        {
            obj.GetComponent<ShopButton>().CheckIfPlayerCanPurchaseASpell(tempPlayerPointsValue);
        }

        StartCoroutine(SubstractionCoroutine(valueToSubstract, 50));
        SetPlayerPointsCountValue();
    }

    //Summary : Permet de mettre à jour la valeur des points possédés par le joueur.
    void SetPlayerPointsCountValue()
    {
        playerPointsValueText.text = GameManager.s_Singleton.playerPointsValue.ToString();
    }

    //Summary : Utiliser pour faire défiler les points lorsque le joueur achète quelque chose (effet de décrémentation dynamique) jusqu'à l'atteinte de la valeur totale à soustraite.
    IEnumerator SubstractionCoroutine(int substractValueToReach, int valueToSubtractPerTicks)
    {
        int startValue = 0;
        do
        {
            startValue += valueToSubtractPerTicks;
            GameManager.s_Singleton.playerPointsValue -= valueToSubtractPerTicks;
            SetPlayerPointsCountValue();
            yield return new WaitForEndOfFrame();
            if (GameManager.s_Singleton.gameStates == GameState.Pause)
            {
                yield return new WaitUntil(() => GameManager.s_Singleton.gameStates == GameState.PlayMode);
            }

        } while (startValue != substractValueToReach);
    }


    //Summary : Permet d'afficher ou désafficher le feedback d'activation des sorts en fonction de l'appui Input.
    void ToggleSpellActivationFeedback()
    {
        spellActivationFeedback.SetActive(!spellActivationFeedback.activeSelf);
        spellCompartmentIsActive = !spellCompartmentIsActive;
    }

    //Summary : Désaffiche le feedback d'activation des sorts
    void DeactivateSpellActivationFeedback()
    {
        spellActivationFeedback.SetActive(false);
        spellCompartmentIsActive = false;
    }

    //Summary : Permet de gérer le switch des sort équipés
    #region Spells Management
    void UseSpell()
    {
        if (spellCompartmentIsActive && spellsCompartments[0].MyCompartmentSpell != null)
        {
            spellsCompartments[0].MyCompartmentSpell = null;

            DisableImageCompotent(spellsCompartments[0].GetComponent<Image>());

            DeactivateSpellActivationFeedback();
            ShopManager.s_Singleton.amntOfSpellBought--;
        }
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
                SwapSpellInCompartmentSpell(spellsCompartments[2], spellsCompartments[0]);

                //Enable and change Image
                SwapImageSprite(spellsCompartments[2].GetComponent<Image>(), spellsCompartments[0].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[0].GetComponent<Image>());
                ResetSpellInCompartmentSpell(spellsCompartments[0]);
            }

            //Sinon si troisième emplacement n'est pas vide --> deuxième emplacement
            else if (spellsCompartments[2].MyCompartmentSpell != null)
            {
                //Debug.Log("troisième emplacement n'est pas vide --> deuxième emplacement");

                //Swap
                SwapSpellInCompartmentSpell(spellsCompartments[1], spellsCompartments[2]);

                //Enable and change Image
                SwapImageSprite(spellsCompartments[1].GetComponent<Image>(), spellsCompartments[2].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[2].GetComponent<Image>());
                ResetSpellInCompartmentSpell(spellsCompartments[2]);
            }

            //Sinon si deuxième emplacement n'est pas vide --> premier emplacement
            else if (spellsCompartments[1].MyCompartmentSpell != null)
            {
                //Debug.Log("deuxième emplacement n'est pas vide --> premier emplacement");

                //Swap
                SwapSpellInCompartmentSpell(spellsCompartments[0], spellsCompartments[1]);

                //Enable and change Image
                SwapImageSprite(spellsCompartments[0].GetComponent<Image>(), spellsCompartments[1].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[1].GetComponent<Image>());
                ResetSpellInCompartmentSpell(spellsCompartments[1]);
            }

        }

        else if (ShopManager.s_Singleton.amntOfSpellBought == 2)
        {
            //Si premier emplacement n'est pas vide --> dernier emplacement
            if (spellsCompartments[0].MyCompartmentSpell != null && spellsCompartments[1].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

                //SWAP
                SwapSpellInCompartmentSpell(spellsCompartments[2], spellsCompartments[0]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[2].GetComponent<Image>(), spellsCompartments[0].MyCompartmentSpell.MySpellIcon);

                //SWAP
                SwapSpellInCompartmentSpell(spellsCompartments[0], spellsCompartments[1]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[0].GetComponent<Image>(), spellsCompartments[1].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[1].GetComponent<Image>());
                //Reset previous spell
                ResetSpellInCompartmentSpell(spellsCompartments[1]);
            }

            else if (spellsCompartments[0].MyCompartmentSpell != null && spellsCompartments[2].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

                //SWAP
                SwapSpellInCompartmentSpell(spellsCompartments[1], spellsCompartments[2]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[1].GetComponent<Image>(), spellsCompartments[2].MyCompartmentSpell.MySpellIcon);

                //SWAP
                SwapSpellInCompartmentSpell(spellsCompartments[2], spellsCompartments[0]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[2].GetComponent<Image>(), spellsCompartments[0].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[0].GetComponent<Image>());
                //Reset previous spell
                ResetSpellInCompartmentSpell(spellsCompartments[0]);
            }

            else if (spellsCompartments[1].MyCompartmentSpell != null && spellsCompartments[2].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

                //SWAP
                SwapSpellInCompartmentSpell(spellsCompartments[0], spellsCompartments[1]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[0].GetComponent<Image>(), spellsCompartments[1].MyCompartmentSpell.MySpellIcon);

                //SWAP
                SwapSpellInCompartmentSpell(spellsCompartments[1], spellsCompartments[2]);
                //Enable and change Image
                SwapImageSprite(spellsCompartments[1].GetComponent<Image>(), spellsCompartments[2].MyCompartmentSpell.MySpellIcon);

                //Disable and reset previous image
                DisableImageCompotent(spellsCompartments[2].GetComponent<Image>());
                //Reset previous spell
                ResetSpellInCompartmentSpell(spellsCompartments[2]);
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

    void ResetSpellInCompartmentSpell(SpellCompartment spellCompartmentToReset)
    {
        spellCompartmentToReset.MyCompartmentSpell = null;
    }

    void SwapSpellInCompartmentSpell(SpellCompartment spellToChange, SpellCompartment wantedSpell)
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
