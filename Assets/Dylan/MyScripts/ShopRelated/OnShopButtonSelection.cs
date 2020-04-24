using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnShopButtonSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    [SerializeField] private CanvasGroup valueToSubstractDisplayer;
    [SerializeField] private TextMeshProUGUI valueToSubstractText;
    private Image buttonSelectionImage;

    public void OnDeselect(BaseEventData eventData)
    {
        if(EventSystem.current.currentSelectedGameObject == this.gameObject && valueToSubstractDisplayer != null)
        {
            valueToSubstractDisplayer.alpha = 0;
            valueToSubstractText.text = null;
        }

        //GetComponent<Image>().enabled = false;
        //GetComponent<Image>().color = new Color(255, 255, 255, 0);

        buttonSelectionImage = GetComponentInChildren<Image>();
        buttonSelectionImage.enabled = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject && ShopManager.s_Singleton.amntOfSpellBought != 3 && valueToSubstractText != null)
        {
            valueToSubstractText.text = "- " + GetComponent<PurchaseASpell>().selectedButton.GetComponent<ShopButtonBehaviour>().spell.MySpellValue.ToString();
            valueToSubstractDisplayer.alpha = 1;
        }

        AkSoundEngine.PostEvent("UI_MenuClickButtons", this.gameObject);
        //GetComponent<Image>().enabled = true;
        //GetComponent<Image>().color = new Color(255, 255, 255, 255);
        buttonSelectionImage = GetComponentInChildren<Image>();
        buttonSelectionImage.enabled = true;
    }
}
