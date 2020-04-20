using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopButtonBehaviour : MonoBehaviour, ISubmitHandler, ISelectHandler
{
    [SerializeField] private Color purchasableButtonColor;
    [SerializeField] private Color unpurchasableButtonColor;

    [SerializeField] private TextMeshProUGUI spellNameText;

    public Spell spell;
    public bool isPurchasable = false;

    [SerializeField] private CanvasGroup validationPopupWindow;
    [SerializeField] private GameObject buttonsLayoutValidationPopup;
    [SerializeField] private GameObject purchaseValidationButton;

    [SerializeField] private Image tooltipSpellImage;
    [SerializeField] private TextMeshProUGUI tooltipSpellNameText;
    [SerializeField] private TextMeshProUGUI tooltipSpellValueText;
    [SerializeField] private TextMeshProUGUI tooltipSpellEffectDescriptionText;

    private void Start()
    {
        if (spell == null)
            spell = GetComponent<Spell>();

        if (spell != null)
        {
            spellNameText.text = spell.MySpellName;
        }
        
        //DEBUG
        gameObject.name = spell.MySpellName;
            
        CheckIfPlayerCanPurchaseASpell();
    }


    public bool CheckIfPlayerCanPurchaseASpell()
    {
        //Possible d'acheter la compétence --> Update de la couleur du bouton (achetable)
        if (UIManager.s_Singleton.playerPointsCountValue >= spell.MySpellValue)
        {
            Debug.Log("Can buy " + spell.MySpellName);
            isPurchasable = true;
            SetButtonColor(purchasableButtonColor);
            return true;
        }

        //Impossible d'acheter la compétence --> Update de la couleur du bouton (non-achetable)
        else if (UIManager.s_Singleton.playerPointsCountValue < spell.MySpellValue)
        {
            isPurchasable = false;
            SetButtonColor(unpurchasableButtonColor);
            return false;
        }

        return CheckIfPlayerCanPurchaseASpell();
    }

    public void DisplayValidationPopup()
    {
        if (isPurchasable)
        {
            Debug.Log("Display Validation Popup");
            StartCoroutine(UIManager.s_Singleton.FadeCanvasGroup(validationPopupWindow, validationPopupWindow.alpha, 1, UIManager.s_Singleton.fadeTime));

            validationPopupWindow.blocksRaycasts = true;

            foreach (Button _buttons in buttonsLayoutValidationPopup.GetComponentsInChildren<Button>())
            {
                _buttons.enabled = true;
            }

            UIManager.s_Singleton.ResetEventSystemFirstSelectedGameObjet(buttonsLayoutValidationPopup.transform.GetChild(1).gameObject);

            //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(validationPopupWindow.transform.GetChild(1).GetChild(1).gameObject);
        }     
    }

    void SetButtonColor(Color _color)
    {
        gameObject.GetComponent<Button>().image.color = _color;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("On Submit click event");
        DisplayValidationPopup();
        purchaseValidationButton.GetComponent<PurchaseASpell>().selectedButton = GetComponent<Button>();
    }

    void SetTooltipInformations()
    {
        tooltipSpellNameText.text = spell.MySpellName;
        tooltipSpellValueText.text = spell.MySpellValue.ToString();
        tooltipSpellEffectDescriptionText.text = spell.MySpellEffectDescription;
        tooltipSpellImage.sprite = spell.MySpellIcon;
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetTooltipInformations();
    }
}
