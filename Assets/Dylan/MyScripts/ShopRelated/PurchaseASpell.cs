using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseASpell : MonoBehaviour
{
    public Button selectedButton;

    //Fonction attachée au clique sur "Oui" de la fenêtre popup de validation d'achat...
    public void PurchaseSelectedSpell()
    {
        //Est-que le sort contenu dans la bouton est achetable ? ...
        if (selectedButton.GetComponent<ShopButtonBehaviour>().isPurchasable)
        {
            for (int i = 0; i < UIManager.s_Singleton.spellsCompartments.Count; i++)
            {
                //Si un des compartiments de sort est vide et le joueur n'a pas encore atteint la limite de sort achetable...
                if (UIManager.s_Singleton.spellsCompartments[i].GetComponent<SpellCompartment>().MyCompartmentSpell == null && ShopManager.s_Singleton.amntOfSpellBought < 3)
                {
                    //Incrémentation du nombre de sort acheté par le joueur
                    ShopManager.s_Singleton.amntOfSpellBought++;

                    //Définition de la valeur à soustraire aux points totaux du joueur
                    UIManager.s_Singleton.SetValueToSubstract(selectedButton.GetComponent<ShopButtonBehaviour>().spell.MySpellValue);

                    //Activation du component image + changement de son sprite du compartiment de sort dans lequel le sort acheté a été ajouté
                    UIManager.s_Singleton.spellsCompartments[i].GetComponent<SpellCompartment>().MyCompartmentSpell = selectedButton.GetComponent<ShopButtonBehaviour>().spell;
                    UIManager.s_Singleton.spellsCompartments[i].GetComponent<Image>().enabled = true;
                    UIManager.s_Singleton.spellsCompartments[i].GetComponent<Image>().sprite = selectedButton.GetComponent<ShopButtonBehaviour>().spell.MySpellIcon;

                    return;
                }
            }
        }
    }

    //Renseigné sur chacun des boutons du magasin.
    //Lorsque le joueur clique sur un de ces boutons, il est référencé dans BuySelectedSpell(), au-dessus, afin de récupérer le sort "contenu" dans le bouton.
    public void SetSelectedButton(Button _selectedButton)
    {
        selectedButton = _selectedButton;
    }
}
