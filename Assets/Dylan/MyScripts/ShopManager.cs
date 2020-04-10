using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject[] spellsAvailableInShop;
    public int amntOfSpellBoughtable = 0;

    public static ShopManager s_Singleton;
    private void Awake()
    {
        if (s_Singleton != null)
        {
            Destroy(this);
        }
        else
        {
            s_Singleton = this;

            if (spellsAvailableInShop.Length == 0)
                Debug.LogError("Please populate the spell avalaible in shop list !!!!!!!!!!");
        }
    }

}
