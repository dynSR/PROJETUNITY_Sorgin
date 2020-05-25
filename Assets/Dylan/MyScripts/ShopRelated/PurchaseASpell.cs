using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseASpell : MonoBehaviour
{
    public Button selectedButton;
    [SerializeField] private BuyPopup buyPopup;
    public bool canShowUpInInventory = false;

    //Fonction attachée au clique sur "Oui" de la fenêtre popup de validation d'achat...
    public void PurchaseSelectedSpell()
    {
        //Est-que le sort contenu dans la bouton est achetable ? ...
        if (selectedButton.GetComponent<ShopButton>().isPurchasable)
        {
            for (int i = 0; i < PlayerSpellsInventory.s_Singleton.spellsCompartments.Count; i++)
            {
                //Si un des compartiments de sort est vide et le joueur n'a pas encore atteint la limite de sort achetable...
                if (PlayerSpellsInventory.s_Singleton.spellsCompartments[i].GetComponent<SpellCompartment>().MyCompartmentSpell == null && ShopManager.s_Singleton.amntOfSpellBought < 3)
                {
                    //Incrémentation du nombre de sort acheté par le joueur
                    ShopManager.s_Singleton.amntOfSpellBought++;

                    //Définition de la valeur à soustraire aux points totaux du joueur
                    UIManager.s_Singleton.SetValueToSubstract(selectedButton.GetComponent<ShopButton>().spell.MySpellValue);

                    buyPopup.SetBuyPopupImage(selectedButton.GetComponent<ShopButton>().spell.MySpellIcon);
                    DisplayPurchasePopup();

                    //Activation du component image + changement de son sprite du compartiment de sort dans lequel le sort acheté a été ajouté
                    PlayerSpellsInventory.s_Singleton.spellsCompartments[i].GetComponent<SpellCompartment>().MyCompartmentSpell = selectedButton.GetComponent<ShopButton>().spell;

                    StartCoroutine(ShowPurchasedSpellInInventory(PlayerSpellsInventory.s_Singleton.spellsCompartments[i].GetComponent<Image>(), selectedButton.GetComponent<ShopButton>().spell.MySpellIcon));

                    //PlayerSpellsInventory.s_Singleton.spellsCompartments[i].GetComponent<Image>().enabled = true;
                    //PlayerSpellsInventory.s_Singleton.spellsCompartments[i].GetComponent<Image>().sprite = selectedButton.GetComponent<ShopButton>().spell.MySpellIcon;
                    
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

    void DisplayPurchasePopup()
    {
        buyPopup.gameObject.SetActive(true);
        buyPopup.SetAnimatorBoolValueToTrue();
    }

    public IEnumerator ShowPurchasedSpellInInventory(Image imageToDisplay, Sprite newImage)
    {
        yield return new WaitUntil(() => canShowUpInInventory);
        imageToDisplay.enabled = true;
        imageToDisplay.sprite = newImage;

        canShowUpInInventory = false;
    }
}
