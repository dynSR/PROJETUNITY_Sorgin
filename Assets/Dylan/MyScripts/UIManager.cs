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

    public GameObject playerSpellActivationCompartment;
    private bool spellCompartmentIsActive = false;

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
            ActivateObjectCompartment();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Debug.Log("X pressed");
            UseObject();
        }
    }

    public void UIWindowsDisplayToggle(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void OnClickAddPointsButton(int valueToAdd)
    {
        playerPointsCountValue += valueToAdd;
        SetPlayerPointsCountValue();

        foreach (GameObject obj in ShopManager.s_Singleton.spellsAvailableInShop)
        {
            obj.GetComponent<ShopButton>().CheckIfCanBuySpells();
        }
    }

    public void OnClickSpellButton(int valueToSubstract)
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

    void ActivateObjectCompartment()
    {
        playerSpellActivationCompartment.SetActive(!playerSpellActivationCompartment.activeSelf);
        spellCompartmentIsActive = !spellCompartmentIsActive;
    }

    void UseObject()
    {
        if (spellCompartmentIsActive)
        {
            Spell activatedSpell = playerSpellsCompartment[0].MyCompartmentSpell;
            Debug.Log(activatedSpell.MySpellName);
        }
    }

    void SwitchCompetences()
    {
        Spell tempSpell01 = playerSpellsCompartment[0].MyCompartmentSpell;
        Spell tempSpell02 = playerSpellsCompartment[1].MyCompartmentSpell;
        Spell tempSpell03 = playerSpellsCompartment[2].MyCompartmentSpell;

        Sprite tempSpellIcon01 = playerSpellsCompartment[0].MyCompartmentSpell.MySpellIcon;
        Sprite tempSpellIcon02 = playerSpellsCompartment[1].MyCompartmentSpell.MySpellIcon;
        Sprite tempSpellIcon03 = playerSpellsCompartment[2].MyCompartmentSpell.MySpellIcon;

        //FRONT TO BACK
        playerSpellsCompartment[0].MyCompartmentSpell = tempSpell02;
        playerSpellsCompartment[0].GetComponent<Image>().sprite = tempSpellIcon02;
        //MID TO FRONT
        playerSpellsCompartment[1].MyCompartmentSpell = tempSpell03;
        playerSpellsCompartment[1].GetComponent<Image>().sprite = tempSpellIcon03;
        //BACK TO MID
        playerSpellsCompartment[2].MyCompartmentSpell = tempSpell01;
        playerSpellsCompartment[2].GetComponent<Image>().sprite = tempSpellIcon01;
    }
}
