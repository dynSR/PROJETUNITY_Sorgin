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

    public Spell spell;
    public bool isBoughtable = false;

    [SerializeField] private CanvasGroup validationPopupWindow;

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
            isBoughtable = true;
            //Debug.Log("Can buy " + gameObject.name);
            SetButtonColor(enabledButtonColor);
            return true;
        }

        //Impossible d'acheter la compétence --> Update de la couleur du bouton (disabled)
        else if (UIManager.s_Singleton.playerPointsCountValue < spell.MySpellValue)
        {
            isBoughtable = false;
            //Debug.Log("Cannot buy " + gameObject.name);
            SetButtonColor(disabledButtonColor);
            return false;
        }

        return CheckIfCanBuySpells();
    }

    public void DisplayPopupValidationWindow()
    {
        if (isBoughtable)
        {
            StartCoroutine(UIManager.s_Singleton.FadeCanvasGroup(validationPopupWindow, validationPopupWindow.alpha, 1, UIManager.s_Singleton.canvasFadeTime));

            validationPopupWindow.blocksRaycasts = true;

            foreach (Button _buttons in validationPopupWindow.GetComponentsInChildren<Button>())
            {
                _buttons.enabled = true;
            }

            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(validationPopupWindow.transform.GetChild(1).GetChild(1).gameObject);
        }
            
    }

    void SetButtonColor(Color _color)
    {
        gameObject.GetComponent<Button>().image.color = _color;
    }
}
