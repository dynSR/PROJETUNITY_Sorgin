using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class BuyPopup : MonoBehaviour
{
    private Animator myAnimator;
    [SerializeField] private PurchaseASpell purchaseASpellMethod;
    [SerializeField] private Image imageToSet;

    // Start is called before the first frame update
    void Start()
    {
        if ((myAnimator = GetComponent<Animator>()) == null)
            Debug.LogError("You need to assign the Animator attached to the parent gameObject");
        else
            myAnimator = GetComponentInParent<Animator>();
    }

    public void SetBuyPopupImage(Sprite newSprite)
    {
        imageToSet.sprite = newSprite;
    }

    public void SetAnimatorBoolValueToTrue()
    {
        myAnimator.SetBool("IsBuying", true);
    }

    public void SetAnimatorBoolValueToFalse()
    {
        myAnimator.SetBool("IsBuying", false);
    }

    public void CanDisplayPurchasedSpellInInventory()
    {
        purchaseASpellMethod.canShowUpInInventory = true;
    }
}
