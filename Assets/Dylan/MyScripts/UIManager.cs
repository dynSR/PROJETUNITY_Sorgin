using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public int playerPointsCountValue;
    [SerializeField] private TextMeshProUGUI playerPointsCountValueText;

    public List<SpellCompartment> playerSpellsCompartment;

    public GameObject playerSpellActivationFeedback;
    private bool spellCompartmentIsActive = false;

    [SerializeField] private CanvasGroup validationPopupWindow;
    public float canvasFadeTime;


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

            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
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
            SwitchCompetences();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            Debug.Log("L2 pressed");
            SetStateOfSpellActivationFeedback();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Debug.Log("Square pressed");
            UseSpell();
        }
    }

    //General Display Toggle for every UI elements that need it
    public void UIWindowsDisplayToggle(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void HidePopupValidationWindow()
    {
        StartCoroutine(FadeCanvasGroup(validationPopupWindow, validationPopupWindow.alpha, 0, canvasFadeTime));
        validationPopupWindow.blocksRaycasts = false;

        foreach (Button _buttons in validationPopupWindow.GetComponentsInChildren<Button>())
        {
            _buttons.enabled = false;
        }

        BuySpell _bS = validationPopupWindow.GetComponentInChildren<BuySpell>();

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_bS.buttonSelected.gameObject);
    }

    public void AddPointsToPlayerScore(int valueToAdd)
    {
        playerPointsCountValue += valueToAdd;
        SetPlayerPointsCountValue();

        foreach (GameObject obj in ShopManager.s_Singleton.spellsAvailableInShop)
        {
            obj.GetComponent<ShopButton>().CheckIfCanBuySpells();
        }
    }

    public void SetValueToSubstract(int valueToSubstract)
    {
        playerPointsCountValue -= valueToSubstract;
        SetPlayerPointsCountValue();

        foreach (GameObject obj in ShopManager.s_Singleton.spellsAvailableInShop)
        {
            obj.GetComponent<ShopButton>().CheckIfCanBuySpells();
        }
    }

    void SetPlayerPointsCountValue()
    {
        playerPointsCountValueText.text = playerPointsCountValue.ToString();
    }

    void SetStateOfSpellActivationFeedback()
    {
        playerSpellActivationFeedback.SetActive(!playerSpellActivationFeedback.activeSelf);
        spellCompartmentIsActive = !spellCompartmentIsActive;
    }

    void UseSpell()
    {
        if (spellCompartmentIsActive && playerSpellsCompartment[0].MyCompartmentSpell != null)
        {
            Spell activatedSpell = playerSpellsCompartment[0].MyCompartmentSpell;
            playerSpellsCompartment[0].MyCompartmentSpell = null;

            DisableElementImageCompotent(playerSpellsCompartment[0].GetComponent<Image>());

            SetStateOfSpellActivationFeedback();
            ShopManager.s_Singleton.amntOfSpellBought--;
            Debug.Log(activatedSpell.MySpellName);
        }
    }

    void SwitchCompetences()
    {
        // Check si les compartiments possèdent un spell renseigné
        if (ShopManager.s_Singleton.amntOfSpellBought == 0)
        {
            Debug.Log("No spell referenced");
            return;
        }

        else if (ShopManager.s_Singleton.amntOfSpellBought == 1)
        {
            //Si premier emplacement n'est pas vide --> dernier emplacement
            if (playerSpellsCompartment[0].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");
                //SWAP
                SwapAndResetSpells(playerSpellsCompartment[2], playerSpellsCompartment[0]);
                
                //Enable and change Image
                EnableAndChangeElementImageComponent(playerSpellsCompartment[2].GetComponent<Image>(), playerSpellsCompartment[0].MyCompartmentSpell.MySpellIcon);
                //Disable previous image
                DisableElementImageCompotent(playerSpellsCompartment[0].GetComponent<Image>());
                playerSpellsCompartment[0].MyCompartmentSpell = null;
            }

            //Sinon si troisième emplacement n'est pas vide --> deuxième emplacement
            else if (playerSpellsCompartment[2].MyCompartmentSpell != null)
            {
                Debug.Log("troisième emplacement n'est pas vide --> deuxième emplacement");
                SwapAndResetSpells(playerSpellsCompartment[1], playerSpellsCompartment[2]);
                

                EnableAndChangeElementImageComponent(playerSpellsCompartment[1].GetComponent<Image>(), playerSpellsCompartment[2].MyCompartmentSpell.MySpellIcon);

                DisableElementImageCompotent(playerSpellsCompartment[2].GetComponent<Image>());
                playerSpellsCompartment[2].MyCompartmentSpell = null;
            }

            //Sinon si deuxième emplacement n'est pas vide --> premier emplacement
            else if (playerSpellsCompartment[1].MyCompartmentSpell != null)
            {
                Debug.Log("deuxième emplacement n'est pas vide --> premier emplacement");
                SwapAndResetSpells(playerSpellsCompartment[0], playerSpellsCompartment[1]);
               

                EnableAndChangeElementImageComponent(playerSpellsCompartment[0].GetComponent<Image>(), playerSpellsCompartment[1].MyCompartmentSpell.MySpellIcon);

                DisableElementImageCompotent(playerSpellsCompartment[1].GetComponent<Image>());
                playerSpellsCompartment[1].MyCompartmentSpell = null;
            }

        }

        else if (ShopManager.s_Singleton.amntOfSpellBought == 2)
        {
            //Si premier emplacement n'est pas vide --> dernier emplacement
            if (playerSpellsCompartment[0].MyCompartmentSpell != null && playerSpellsCompartment[1].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");
                //SWAP
                SwapAndResetSpells(playerSpellsCompartment[2], playerSpellsCompartment[0]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(playerSpellsCompartment[2].GetComponent<Image>(), playerSpellsCompartment[0].MyCompartmentSpell.MySpellIcon);

                //
                //SWAP
                SwapAndResetSpells(playerSpellsCompartment[0], playerSpellsCompartment[1]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(playerSpellsCompartment[0].GetComponent<Image>(), playerSpellsCompartment[1].MyCompartmentSpell.MySpellIcon);

                //Disable previous image
                DisableElementImageCompotent(playerSpellsCompartment[1].GetComponent<Image>());
                //Reset previous spell
                playerSpellsCompartment[1].MyCompartmentSpell = null;
            }

            else if (playerSpellsCompartment[0].MyCompartmentSpell != null && playerSpellsCompartment[2].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");
                //SWAP
                SwapAndResetSpells(playerSpellsCompartment[1], playerSpellsCompartment[2]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(playerSpellsCompartment[1].GetComponent<Image>(), playerSpellsCompartment[2].MyCompartmentSpell.MySpellIcon);

                //
                //SWAP
                SwapAndResetSpells(playerSpellsCompartment[2], playerSpellsCompartment[0]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(playerSpellsCompartment[2].GetComponent<Image>(), playerSpellsCompartment[0].MyCompartmentSpell.MySpellIcon);

                //Disable previous image
                DisableElementImageCompotent(playerSpellsCompartment[0].GetComponent<Image>());
                //Reset previous spell
                playerSpellsCompartment[0].MyCompartmentSpell = null;
            }

            else if (playerSpellsCompartment[1].MyCompartmentSpell != null && playerSpellsCompartment[2].MyCompartmentSpell != null)
            {
                Debug.Log("premier emplacement n'est pas vide --> dernier emplacement");
                //SWAP
                SwapAndResetSpells(playerSpellsCompartment[0], playerSpellsCompartment[1]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(playerSpellsCompartment[0].GetComponent<Image>(), playerSpellsCompartment[1].MyCompartmentSpell.MySpellIcon);

                //
                //SWAP
                SwapAndResetSpells(playerSpellsCompartment[1], playerSpellsCompartment[2]);
                //Enable and change Image
                EnableAndChangeElementImageComponent(playerSpellsCompartment[1].GetComponent<Image>(), playerSpellsCompartment[2].MyCompartmentSpell.MySpellIcon);

                //Disable previous image
                DisableElementImageCompotent(playerSpellsCompartment[2].GetComponent<Image>());
                //Reset previous spell
                playerSpellsCompartment[2].MyCompartmentSpell = null;
            }
        }

        else if (ShopManager.s_Singleton.amntOfSpellBought == 3)
        {
            Spell _spellCompartment00 = playerSpellsCompartment[0].MyCompartmentSpell;
            Spell _spellCompartment01 = playerSpellsCompartment[1].MyCompartmentSpell;
            Spell _spellCompartment02 = playerSpellsCompartment[2].MyCompartmentSpell;

            Sprite _spriteSpellCompartment00 = playerSpellsCompartment[0].GetComponent<Image>().sprite;
            Sprite _spriteSpellCompartment01 = playerSpellsCompartment[1].GetComponent<Image>().sprite;
            Sprite _spriteSpellCompartment02 = playerSpellsCompartment[2].GetComponent<Image>().sprite;

            playerSpellsCompartment[0].MyCompartmentSpell = _spellCompartment01;
            playerSpellsCompartment[0].GetComponent<Image>().sprite = _spriteSpellCompartment01;

            playerSpellsCompartment[2].MyCompartmentSpell = _spellCompartment00;
            playerSpellsCompartment[2].GetComponent<Image>().sprite = _spriteSpellCompartment00;

            playerSpellsCompartment[1].MyCompartmentSpell = _spellCompartment02;
            playerSpellsCompartment[1].GetComponent<Image>().sprite = _spriteSpellCompartment02;
        }
    }

    void SwapAndResetSpells(SpellCompartment spellToChange, SpellCompartment wantedSpell)
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
}
