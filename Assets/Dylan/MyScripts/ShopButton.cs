using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private Color enabledButtonColor;
    [SerializeField] private Color disabledButtonColor;

    [SerializeField] private TextMeshProUGUI spellNameText;
    [SerializeField] private TextMeshProUGUI spellValueText;
    
    [SerializeField] private Spell spell;

    private void Start()
    {
        spellNameText.text = spell.MySpellName;
        spellValueText.text = spell.MySpellValue.ToString();

        //DEBUG
        gameObject.name = spell.MySpellName;
            
        CheckIfCanBuySpells();
    }


    public bool CheckIfCanBuySpells()
    {
        //Possible d'acheter la compétence --> Update de la couleur du bouton (enabled)
        if (UIManager.s_Singleton.playerPointsCountValue >= spell.MySpellValue)
        {
            Debug.Log("Can buy " + gameObject.name);
            SetButtonColor(enabledButtonColor);
            return true;
        }

        //Impossible d'acheter la compétence --> Update de la couleur du bouton (disabled)
        else if (UIManager.s_Singleton.playerPointsCountValue < spell.MySpellValue)
        {
            Debug.Log("Cannot buy " + gameObject.name);
            SetButtonColor(disabledButtonColor);
            return false;
        }

        return CheckIfCanBuySpells();
    }

    public void BuySpell()
    {
        if (UIManager.s_Singleton.playerPointsCountValue >= spell.MySpellValue)
        {
            for (int i = 0; i < UIManager.s_Singleton.playerSpellsCompartment.Count; i++)
            {
                if (UIManager.s_Singleton.playerSpellsCompartment[i].GetComponent<SpellCompartment>().MyCompartmentSpell == null && ShopManager.s_Singleton.amntOfSpellBoughtable < 3)
                {
                    ShopManager.s_Singleton.amntOfSpellBoughtable++;
                    UIManager.s_Singleton.OnClickSpellButton(spell.MySpellValue);
                    UIManager.s_Singleton.playerSpellsCompartment[i].GetComponent<SpellCompartment>().MyCompartmentSpell = spell;
                    UIManager.s_Singleton.playerSpellsCompartment[i].GetComponent<Image>().enabled = true;
                    UIManager.s_Singleton.playerSpellsCompartment[i].GetComponent<Image>().sprite = spell.MySpellIcon;
                    return;
                }
            }
        }
    }

    void SetButtonColor(Color _color)
    {
        gameObject.GetComponent<Button>().image.color = _color;
    }
}
