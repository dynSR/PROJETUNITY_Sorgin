using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public int playerPointsCountValue;
    [SerializeField] private TextMeshProUGUI playerPointsCountValueText;

    public List<SpellCompartment> spellsCompartments;

    public GameObject spellActivationFeedback;
    private bool isSpellCompartmentActive = false;

    [SerializeField] private CanvasGroup purchaseValidationPopup;

    public float fadeTime = 0.25f;


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
    }

    //Utilisé dans les button component pour afficher / désafficher un élément d'UI
    public void UIWindowsDisplayToggle(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void ResetEventSystemFirstSelectedGameObjet(GameObject obj)
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(obj);
    }

    public void HideValidationPopup()
    {
        StartCoroutine(FadeCanvasGroup(purchaseValidationPopup, purchaseValidationPopup.alpha, 0, fadeTime));
        purchaseValidationPopup.blocksRaycasts = false;

        foreach (Button _buttons in purchaseValidationPopup.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = false;
        }

        PurchaseASpell purchasedSpell = purchaseValidationPopup.GetComponentInChildren<PurchaseASpell>();
        ResetEventSystemFirstSelectedGameObjet(purchasedSpell.selectedButton.gameObject);
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
        isSpellCompartmentActive = !isSpellCompartmentActive;
    }

    void DeactivateSpellActivationFeedback()
    {
        spellActivationFeedback.SetActive(false);
        isSpellCompartmentActive = false;
    }

    #region Spells Management
    void UseSpell()
    {
        if (isSpellCompartmentActive && spellsCompartments[0].MyCompartmentSpell != null)
        {
            //Spell activatedSpell = spellsCompartments[0].MyCompartmentSpell;
            spellsCompartments[0].MyCompartmentSpell = null;

            DisableElementImageCompotent(spellsCompartments[0].GetComponent<Image>());

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
                //SWAP
                SwapSpellComponent(spellsCompartments[2], spellsCompartments[0]);

                //Enable and change Image
                EnableAndChangeElementImageComponent(spellsCompartments[2].GetComponent<Image>(), spellsCompartments[0].MyCompartmentSpell.MySpellIcon);
                //Disable previous image
                DisableElementImageCompotent(spellsCompartments[0].GetComponent<Image>());
                spellsCompartments[0].MyCompartmentSpell = null;
            }

            //Sinon si troisième emplacement n'est pas vide --> deuxième emplacement
            else if (spellsCompartments[2].MyCompartmentSpell != null)
            {
                Debug.Log("troisième emplacement n'est pas vide --> deuxième emplacement");
                SwapSpellComponent(spellsCompartments[1], spellsCompartments[2]);


                EnableAndChangeElementImageComponent(spellsCompartments[1].GetComponent<Image>(), spellsCompartments[2].MyCompartmentSpell.MySpellIcon);

                DisableElementImageCompotent(spellsCompartments[2].GetComponent<Image>());
                spellsCompartments[2].MyCompartmentSpell = null;
            }

            //Sinon si deuxième emplacement n'est pas vide --> premier emplacement
            else if (spellsCompartments[1].MyCompartmentSpell != null)
            {
                Debug.Log("deuxième emplacement n'est pas vide --> premier emplacement");
                SwapSpellComponent(spellsCompartments[0], spellsCompartments[1]);


                EnableAndChangeElementImageComponent(spellsCompartments[0].GetComponent<Image>(), spellsCompartments[1].MyCompartmentSpell.MySpellIcon);

                DisableElementImageCompotent(spellsCompartments[1].GetComponent<Image>());
                spellsCompartments[1].MyCompartmentSpell = null;
            }

        }

        else if (ShopManager.s_Singleton.amntOfSpellBought == 2)
        {
            //Si premier emplacement n'est pas vide --> dernier emplacement
            if (spellsCompartments[0].MyCompartmentSpell != null && spellsCompartments[1].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");
                //SWAP
                SwapSpellComponent(spellsCompartments[2], spellsCompartments[0]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(spellsCompartments[2].GetComponent<Image>(), spellsCompartments[0].MyCompartmentSpell.MySpellIcon);

                //
                //SWAP
                SwapSpellComponent(spellsCompartments[0], spellsCompartments[1]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(spellsCompartments[0].GetComponent<Image>(), spellsCompartments[1].MyCompartmentSpell.MySpellIcon);

                //Disable previous image
                DisableElementImageCompotent(spellsCompartments[1].GetComponent<Image>());
                //Reset previous spell
                spellsCompartments[1].MyCompartmentSpell = null;
            }

            else if (spellsCompartments[0].MyCompartmentSpell != null && spellsCompartments[2].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");
                //SWAP
                SwapSpellComponent(spellsCompartments[1], spellsCompartments[2]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(spellsCompartments[1].GetComponent<Image>(), spellsCompartments[2].MyCompartmentSpell.MySpellIcon);

                //
                //SWAP
                SwapSpellComponent(spellsCompartments[2], spellsCompartments[0]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(spellsCompartments[2].GetComponent<Image>(), spellsCompartments[0].MyCompartmentSpell.MySpellIcon);

                //Disable previous image
                DisableElementImageCompotent(spellsCompartments[0].GetComponent<Image>());
                //Reset previous spell
                spellsCompartments[0].MyCompartmentSpell = null;
            }

            else if (spellsCompartments[1].MyCompartmentSpell != null && spellsCompartments[2].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");
                //SWAP
                SwapSpellComponent(spellsCompartments[0], spellsCompartments[1]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(spellsCompartments[0].GetComponent<Image>(), spellsCompartments[1].MyCompartmentSpell.MySpellIcon);

                //
                //SWAP
                SwapSpellComponent(spellsCompartments[1], spellsCompartments[2]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(spellsCompartments[1].GetComponent<Image>(), spellsCompartments[2].MyCompartmentSpell.MySpellIcon);

                //Disable previous image
                DisableElementImageCompotent(spellsCompartments[2].GetComponent<Image>());
                //Reset previous spell
                spellsCompartments[2].MyCompartmentSpell = null;
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
            spellsCompartments[0].GetComponent<Image>().sprite = _spriteSpellCompartment01;

            spellsCompartments[2].MyCompartmentSpell = _spellCompartment00;
            spellsCompartments[2].GetComponent<Image>().sprite = _spriteSpellCompartment00;

            spellsCompartments[1].MyCompartmentSpell = _spellCompartment02;
            spellsCompartments[1].GetComponent<Image>().sprite = _spriteSpellCompartment02;
        }
    }

    void SwapSpellComponent(SpellCompartment spellToChange, SpellCompartment wantedSpell)
    {
        spellToChange.MyCompartmentSpell = wantedSpell.MyCompartmentSpell;
    }

    void EnableAndChangeElementImageComponent(Image imageToChange, Sprite wantedImageSprite)
    {
        if (!imageToChange.enabled)
            imageToChange.enabled = true;

        imageToChange.sprite = wantedImageSprite;
    }

    void DisableElementImageCompotent(Image imageToDisable)
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
