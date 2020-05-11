using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ValidationPopupButtonSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    [SerializeField] private CanvasGroup valueToSubstractDisplayer;
    [SerializeField] private TextMeshProUGUI valueToSubstractText;
    private Image buttonSelectionImage;

    //Summary : À la désélection du bouton --> désaffiche la valeur du sort à soustraire au total des points du joueur et la reset + désaffichage du réticule de sélection.
    public void OnDeselect(BaseEventData eventData)
    {
        if (valueToSubstractDisplayer != null)
        {
            if (EventSystem.current.currentSelectedGameObject == this.gameObject)
            {
                valueToSubstractDisplayer.alpha = 0;
                valueToSubstractText.text = null;
            }
        }

        buttonSelectionImage = GetComponentInChildren<Image>();
        buttonSelectionImage.enabled = false;
    }

    //Summary : À la sélection du bouton --> affiche la valeur du sort à soustraire au total des points du joueur + affichage du réticule de sélection.
    public void OnSelect(BaseEventData eventData)
    {
        if(valueToSubstractText != null)
        {
            if (EventSystem.current.currentSelectedGameObject == this.gameObject && ShopManager.s_Singleton.amntOfSpellBought != 3)
            {
                valueToSubstractText.text = "- " + GetComponent<PurchaseASpell>().selectedButton.GetComponent<ShopButton>().spell.MySpellValue.ToString();
                valueToSubstractDisplayer.alpha = 1;
            }
        }
        
        buttonSelectionImage = GetComponentInChildren<Image>();
        buttonSelectionImage.enabled = true;
    }
}
