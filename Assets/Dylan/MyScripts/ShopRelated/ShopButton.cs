using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopButton : MonoBehaviour, ISubmitHandler/*, ISelectHandler, IDeselectHandler*/
{
    [Header("BUTTON COLORS")]
    [SerializeField] private Color purchasableButtonColor = Color.white;
    [SerializeField] private Color unpurchasableButtonColor = Color.red;
    public Color inventoryIsFullColor = Color.grey;

    [Header("SPELL ATTACHED TO THE BUTTON - DEBUG")]
    public Spell spell;
    public bool isPurchasable = false;
    [SerializeField] private TextMeshProUGUI spellNameText;

    [Header("VALIDATION POPUP PARAMETERS")]
    [SerializeField] private GameObject validationPopupPurchaseButton;

    [Header("SPELL TOOLTIP PARAMETERS")]
    [SerializeField] private GameObject spellTooltipGameObject;
    [SerializeField] private Image spellTooltipImage;
    [SerializeField] private TextMeshProUGUI spellTooltipNameText;
    [SerializeField] private TextMeshProUGUI spellTooltipValueText;
    [SerializeField] private TextMeshProUGUI spellTooltipEffectDescriptionText;

    [Header("SPELLSHOP BUTTON PARAMETERS")]
    [SerializeField] private GameObject avalaibleSpellShopButtonIcon;
    [SerializeField] private GameObject avalaibleSpellShopButtonBackground;
    [SerializeField] private Image spellShopButtonImage;
    [SerializeField] private TextMeshProUGUI spellShopButtonNameText;
    [SerializeField] private TextMeshProUGUI spellShopButtonValueText;
    [SerializeField] private TextMeshProUGUI spellShopButtonEffectDescriptionText;

    private void Start()
    {
        if (spellNameText == null)
            spellNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (spell != null)
            spellNameText.text = spell.MySpellName;
        
        //DEBUG
        gameObject.name = spell.MySpellName;
            
        CheckIfPlayerCanPurchaseASpell(GameManager.s_Singleton.playerPointsValue);

        SetShopButtonButtonInformations();
    }

    //Summary : Permet de vérifier sur les boutons du magasin, si un des sorts contenus dans ceux-ci peut être acheté
    public bool CheckIfPlayerCanPurchaseASpell(int valueToCompare)
    {
        //Si la valeur comparée (ex : valeur des points du joueur) >= à la valeur du sort...
        if (valueToCompare >= spell.MySpellValue)
        {
            //Alors la couleur du bouton change pour signifier au joueur qu'il peut appuyer dessus et acheter ce sort.
            //Debug.Log("Can buy " + spell.MySpellName);
            isPurchasable = true;
            SetButtonColor(purchasableButtonColor);
            return true;
        }
        //Si la valeur comparée (ex : valeur des points du joueur) < à la valeur du sort...
        else if (valueToCompare < spell.MySpellValue)
        {
            //Alors la couleur du bouton change pour signifier au joueur qu'il ne peut pas appuyer dessus et ne peut pas acheter ce sort.
            isPurchasable = false;
            SetButtonColor(unpurchasableButtonColor);
            return false;
        }
        
        return CheckIfPlayerCanPurchaseASpell(valueToCompare);
    }

    //Summary : Affiche la fenêtre de confirmation d'achat lorsque le joueur appuie sur un bouton contenant un sort qu'il peut acheter.
    public void DisplayPurchaseValidationPopupWindow()
    {
        if (isPurchasable)
        {
            Debug.Log("Display Validation Popup");

            UIManager.s_Singleton.DisplayAPopup(UIManager.s_Singleton.purchaseValidationPopupWindow);
            UIManager.s_Singleton.EnableButtonsInLayout(UIManager.s_Singleton.purchaseValidationPopupButtonLayout, UIManager.s_Singleton.purchaseValidationPopupButtonLayout.transform.GetChild(1).gameObject);
            UIManager.s_Singleton.purchaseValidationPopupIsDisplayed = true;
        }     
    }

    //Summary : Permet d'attribuer une couleur définie à un bouton.
    public void SetButtonColor(Color _color)
    {
        gameObject.GetComponent<Button>().image.color = _color;

        for (int i = 0; i < avalaibleSpellShopButtonIcon.transform.childCount; i++)
        {
            avalaibleSpellShopButtonIcon.transform.GetChild(i).GetComponent<Image>().color = _color;
        }
    }

    //Summary : Lorsque le joueur appuie sur "X", "A", la fenêtre de validation s'affiche + référence du sort étant en train d'être acheté.
    public void OnSubmit(BaseEventData eventData)
    {
        if (!ShopManager.isBuying && ShopManager.s_Singleton.amntOfSpellBought < 3)
        {
            DisplayPurchaseValidationPopupWindow();
            validationPopupPurchaseButton.GetComponent<PurchaseASpell>().selectedButton = GetComponent<Button>();
        }
    }

    ////Summary : Permet de mettre à jour les informations contenues dans le tooltip des sorts.
    //void SetTooltipInformations()
    //{
    //    spellTooltipNameText.text = spell.MySpellName;
    //    spellTooltipValueText.text = spell.MySpellValue.ToString();
    //    spellTooltipEffectDescriptionText.text = spell.MySpellEffectDescription;
    //    spellTooltipImage.sprite = spell.MySpellIcon;
    //}

    void SetShopButtonButtonInformations()
    {
        spellShopButtonNameText.text = spell.MySpellName;
        spellShopButtonValueText.text = spell.MySpellValue.ToString();
        spellShopButtonEffectDescriptionText.text = spell.MySpellEffectDescription;
        spellShopButtonImage.sprite = spell.MySpellIcon;
       
    }

    ////Summary : À la sélection du bouton --> affiche et set le tooltip.
    //public void OnSelect(BaseEventData eventData)
    //{
    //    SetTooltipInformations();
    //    spellTooltipGameObject.SetActive(true);
    //}

    ////Summary : À la désélection du bouton --> désaffiche le tooltip.
    //public void OnDeselect(BaseEventData eventData)
    //{
    //    spellTooltipGameObject.SetActive(false);
    //}
}
