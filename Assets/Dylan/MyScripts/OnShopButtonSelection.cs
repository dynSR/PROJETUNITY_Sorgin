using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnShopButtonSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    [SerializeField] private CanvasGroup valueToSubstractDisplayer;
    [SerializeField] private TextMeshProUGUI valueToSubstractText;

    public void OnDeselect(BaseEventData eventData)
    {
        if(EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            valueToSubstractDisplayer.alpha = 0;
            valueToSubstractText.text = null;
        }     
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject && ShopManager.s_Singleton.amntOfSpellBought != 3)
        {
            valueToSubstractText.text = "- " + GetComponent<PurchaseASpell>().selectedButton.GetComponent<ShopButtonBehaviour>().spell.MySpellValue.ToString() + " points";
            valueToSubstractDisplayer.alpha = 1;
        }
    }
}
