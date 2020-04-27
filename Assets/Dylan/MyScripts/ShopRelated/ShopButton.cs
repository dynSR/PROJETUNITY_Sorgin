using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopButton : MonoBehaviour, ISubmitHandler, ISelectHandler, IDeselectHandler
{
    [Header("BUTTON COLORS")]
    [SerializeField] private Color purchasableButtonColor = Color.white;
    [SerializeField] private Color unpurchasableButtonColor = Color.red;


    [Header("SPELL ATTACHED TO THE BUTTON - DEBUG")]
    public Spell spell;
    public bool isPurchasable = false;
    [SerializeField] private TextMeshProUGUI spellNameText;

    [Header("VALIDATION POPUP PARAMETERS")]
    [SerializeField] private CanvasGroup validationPopupWindow;
    [SerializeField] private GameObject validationPopupButtonLayout;
    [SerializeField] private GameObject validationPopupPurchaseButton;

    [Header("SPELL TOOLTIP PARAMETERS")]
    [SerializeField] private GameObject spellTooltipGameObject;
    [SerializeField] private Image spellTooltipImage;
    [SerializeField] private TextMeshProUGUI spellTooltipNameText;
    [SerializeField] private TextMeshProUGUI spellTooltipValueText;
    [SerializeField] private TextMeshProUGUI spellTooltipEffectDescriptionText;

    private void Start()
    {
        if (spell == null)
            spell = GetComponent<Spell>();

        if (spellNameText == null)
            spellNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (spell != null)
        {
            spellNameText.text = spell.MySpellName;
        }
        
        //DEBUG
        gameObject.name = spell.MySpellName;
            
        CheckIfPlayerCanPurchaseASpell(UIManager.s_Singleton.playerPointsValue);
    }


    public bool CheckIfPlayerCanPurchaseASpell(int valueToCompare)
    {
        //Possible d'acheter la compétence --> Update de la couleur du bouton (achetable)
        if (valueToCompare >= spell.MySpellValue)
        {
            Debug.Log("Can buy " + spell.MySpellName);
            isPurchasable = true;
            SetButtonColor(purchasableButtonColor);
            return true;
        }

        //Impossible d'acheter la compétence --> Update de la couleur du bouton (non-achetable)
        else if (valueToCompare < spell.MySpellValue)
        {
            isPurchasable = false;
            SetButtonColor(unpurchasableButtonColor);
            return false;
        }

        return CheckIfPlayerCanPurchaseASpell(valueToCompare);
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
        validationPopupPurchaseButton.GetComponent<PurchaseASpell>().selectedButton = GetComponent<Button>();
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
        spellTooltipGameObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        spellTooltipGameObject.SetActive(false);
    }
}
