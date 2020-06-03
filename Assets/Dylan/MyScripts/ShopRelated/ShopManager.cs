using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<Transform> spellsAvailableInShop = new List<Transform>();
    public int amntOfSpellBought = 0;
    public static bool isBuying = false;
    [SerializeField] private Transform spellButtonGroup;

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

        }

        PopulateShopArray();
    }

    void PopulateShopArray()
    {
        for (int i = 0; i < spellButtonGroup.transform.childCount; i++)
        {
            Transform objectFound = spellButtonGroup.transform.GetChild(i);
            spellsAvailableInShop.Add(objectFound);
        }
    }
}
