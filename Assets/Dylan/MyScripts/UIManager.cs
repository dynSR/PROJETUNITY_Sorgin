using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("PLAYER POINTS PARAMETERS")]
    public int playerPointsCountValue;
    [SerializeField] private TextMeshProUGUI playerPointsCountValueText;

    [Header("SPELL COMPARTMENT PARAMETERS")]
    public List<SpellCompartment> spellsCompartments;
    public GameObject spellActivationFeedback;
    private bool spellCompartmentIsActive = false;

    [Header("PURCHASE VALIDATION POPUP")]
    [SerializeField] private CanvasGroup purchaseValidationPopup;
    [HideInInspector]
    public bool purschaseValidationPopupIsDisplayed = false;

    [Header("FADE DURATION")]
    public float fadeDuration = 0.25f;


    public static UIManager s_Singleton;
    private void Awake()
    {
        if (s_Singleton != null)
        {
            Destroy(this);
        }
        else
        {
            s_Singleton = this;
        }
    }

    private void Start()
    {
        //Initialisation des points du joueur au start
        SetPlayerPointsCountValue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            Debug.Log("L1 pressed");
            SwitchSpellsInPlayerInventory();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            Debug.Log("L2 pressed");
            ToggleSpellActivationFeedback();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Debug.Log("Square pressed");
            UseSpell();
        }

        if (purschaseValidationPopupIsDisplayed && Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            Debug.Log("Square pressed");
            HideValidationPopup();
        }
    }

    //Utilisé dans les button component pour afficher / désafficher un élément d'UI
    public void UIWindowsDisplayToggle(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void ResetEventSystemFirstSelectedGameObjet(GameObject obj)
    {
        //Permet de reset et de déterminer le premier objet sélectionné dans l'Event System (obligatoire à cause de l'utilisation de la manette)
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(obj);
    }

    public void HideValidationPopup()
    {
        StartCoroutine(FadeCanvasGroup(purchaseValidationPopup, purchaseValidationPopup.alpha, 0, fadeDuration));
        purchaseValidationPopup.blocksRaycasts = false;

        foreach (Button _buttons in purchaseValidationPopup.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = false;
        }

        PurchaseASpell purchasedSpell = purchaseValidationPopup.GetComponentInChildren<PurchaseASpell>();
        ResetEventSystemFirstSelectedGameObjet(purchasedSpell.selectedButton.gameObject);

        purschaseValidationPopupIsDisplayed = false;
    }

    public void AddPointsToPlayerScore(int valueToAdd)
    {
        playerPointsCountValue += valueToAdd;
        SetPlayerPointsCountValue();

        foreach (GameObject obj in ShopManager.s_Singleton.spellsAvailableInShop)
        {
            obj.GetComponent<ShopButtonBehaviour>().CheckIfPlayerCanPurchaseASpell();
        }
    }

    public void SetValueToSubstract(int valueToSubstract)
    {
        playerPointsCountValue -= valueToSubstract;
        SetPlayerPointsCountValue();

        foreach (GameObject obj in ShopManager.s_Singleton.spellsAvailableInShop)
        {
            obj.GetComponent<ShopButtonBehaviour>().CheckIfPlayerCanPurchaseASpell();
        }
    }

    void SetPlayerPointsCountValue()
    {
        playerPointsCountValueText.text = playerPointsCountValue.ToString();
    }

    void ToggleSpellActivationFeedback()
    {
        spellActivationFeedback.SetActive(!spellActivationFeedback.activeSelf);
        spellCompartmentIsActive = !spellCompartmentIsActive;
    }

    void DeactivateSpellActivationFeedback()
    {
        spellActivationFeedback.SetActive(false);
        spellCompartmentIsActive = false;
    }

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
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");

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
                Debug.Log("troisième emplacement n'est pas vide --> deuxième emplacement");

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
                Debug.Log("deuxième emplacement n'est pas vide --> premier emplacement");

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

    #region Canvas Fade Coroutine
    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {
        float _timerStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timerStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timerStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
