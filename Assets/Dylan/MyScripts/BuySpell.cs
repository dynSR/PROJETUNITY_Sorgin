using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySpell : MonoBehaviour
{
    public Button buttonSelected;

    public void BuySelectedSpell()
    {
        if (buttonSelected.GetComponent<ShopButton>().isBoughtable)
        {
            for (int i = 0; i < UIManager.s_Singleton.playerSpellsCompartment.Count; i++)
            {
                if (UIManager.s_Singleton.playerSpellsCompartment[i].GetComponent<SpellCompartment>().MyCompartmentSpell == null && ShopManager.s_Singleton.amntOfSpellBought < 3)
                {
                    ShopManager.s_Singleton.amntOfSpellBought++;
                    UIManager.s_Singleton.SetValueToSubstract(buttonSelected.GetComponent<ShopButton>().spell.MySpellValue);
                    UIManager.s_Singleton.playerSpellsCompartment[i].GetComponent<SpellCompartment>().MyCompartmentSpell = buttonSelected.GetComponent<ShopButton>().spell;
                    UIManager.s_Singleton.playerSpellsCompartment[i].GetComponent<Image>().enabled = true;
                    UIManager.s_Singleton.playerSpellsCompartment[i].GetComponent<Image>().sprite = buttonSelected.GetComponent<ShopButton>().spell.MySpellIcon;
                    return;
                }
            }
        }
    }

    public void SetSelectedButton(Button _selectedButton)
    {
        buttonSelected = _selectedButton;
    }
}
