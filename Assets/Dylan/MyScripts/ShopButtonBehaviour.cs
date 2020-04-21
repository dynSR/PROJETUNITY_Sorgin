using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopButtonBehaviour : MonoBehaviour, ISubmitHandler, ISelectHandler, IDeselectHandler
{
    [Header("BUTTON COLORS")]
    [SerializeField] private Color purchasableButtonColor;
    [SerializeField] private Color unpurchasableButtonColor;


    [Header("SPELL ATTACHED TO THE BUTTON")]
    public Spell spell;
    public bool isPurchasable = false;
    [SerializeField] private TextMeshProUGUI spellNameText;

    [Header("VALIDATION POPUP PARAMETERS")]
    [SerializeField] private CanvasGroup validationPopupWindow;
    [SerializeField] private GameObject validationPopupButtonLayout;
    [SerializeField] private GameObject purchaseButton;

    [Header("TOOLTIP PARAMETERS")]
    [SerializeField] private GameObject tooltipGameObject;
    [SerializeField] private Image spellTooltipImage;
    [SerializeField] private TextMeshProUGUI spellTooltipNameText;
    [SerializeField] private TextMeshProUGUI spellTooltipValueText;
    [SerializeField] private TextMeshProUGUI spellTooltipEffectDescriptionText;

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
            StartCoroutine(UIManager.s_Singleton.FadeCanvasGroup(validationPopupWindow, validationPopupWindow.alpha, 1, UIManager.s_Singleton.fadeDuration));

            validationPopupWindow.blocksRaycasts = true;

            foreach (Button _buttons in validationPopupButtonLayout.GetComponentsInChildren<Button>())
            {
                _buttons.enabled = true;
            }

            UIManager.s_Singleton.ResetEventSystemFirstSelectedGameObjet(validationPopupButtonLayout.transform.GetChild(1).gameObject);

            UIManager.s_Singleton.purschaseValidationPopupIsDisplayed = true;
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
        purchaseButton.GetComponent<PurchaseASpell>().selectedButton = GetComponent<Button>();
    }

    void SetTooltipInformations()
    {
        spellTooltipNameText.text = spell.MySpellName;
        spellTooltipValueText.text = spell.MySpellValue.ToString();
        spellTooltipEffectDescriptionText.text = spell.MySpellEffectDescription;
        spellTooltipImage.sprite = spell.MySpellIcon;
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetTooltipInformations();
        tooltipGameObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        tooltipGameObject.SetActive(false);
    }
}
