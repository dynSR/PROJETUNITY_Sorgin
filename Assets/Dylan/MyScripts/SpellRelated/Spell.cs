using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Spell_", order = 1)]

public class Spell : ScriptableObject
{
    //Variables des sorts contenus dans le jeu
    public enum SpellType { Etourdissement, Crochetage, Radar, Duplication, Clone, TransformationEnChat, TransformationEnSouris };
    public SpellType spellType;
   
    [SerializeField] private string spellName;
    [SerializeField] private Sprite spellIcon;
    [SerializeField] private float spellDurationOfEffect;
    [SerializeField] private int spellValue;
    [TextArea(1, 5)]
    [SerializeField] private string spellEffectDescription;

    public string MySpellName { get => spellName; }
    public float MySpellDurationOfEffect { get => spellDurationOfEffect; }
    public int MySpellValue { get => spellValue; }
    public Sprite MySpellIcon { get => spellIcon; set => spellIcon = value; }
    public string MySpellEffectDescription { get => spellEffectDescription; set => spellEffectDescription = value; }

    public void UseTheSpell()
    {
        switch (spellType)
        {
            case SpellType.Etourdissement:
                Stun();
                break;
            case SpellType.Crochetage:
                LockPicking();
                break;
            case SpellType.Radar:
                Detection();
                break;
            case SpellType.Duplication:
                //Dupplication();
                break;
            case SpellType.Clone:
                Clonage(Player.s_Singleton.defaultCharacterModelPrefab, Player.s_Singleton.posToInstantiateTheClone);
                break;
            case SpellType.TransformationEnSouris:
                if (Player.s_Singleton.playerIsInHumanForm)
                {
                    MouseTransformation(Player.s_Singleton.defaultCharacterModel, Player.s_Singleton.mouseCharacterModel);
                }
                else if (Player.s_Singleton.playerIsTranformedInCat)
                {
                    MouseTransformation(Player.s_Singleton.catCharacterModel, Player.s_Singleton.mouseCharacterModel);
                }
                break;
            case SpellType.TransformationEnChat:
                if (Player.s_Singleton.playerIsInHumanForm)
                {
                    CatTransformation(Player.s_Singleton.defaultCharacterModel, Player.s_Singleton.catCharacterModel);
                }
                else if (Player.s_Singleton.playerIsTranformedInMouse)
                {
                    CatTransformation(Player.s_Singleton.mouseCharacterModel, Player.s_Singleton.catCharacterModel);
                }
                break;
            default:
                break;
        }
    }

    private void CatTransformation(GameObject objToDisactive, GameObject objToActive)
    {
        Debug.Log("Trying to transform the player character in a cat...");

        objToDisactive.SetActive(false);
        objToActive.SetActive(true);

        Player.s_Singleton.playerIsTranformedInCat = true;
        Player.s_Singleton.playerIsTranformedInMouse = false;
        Player.s_Singleton.playerIsInHumanForm = false;
    }

    private void MouseTransformation(GameObject objToDisactive, GameObject objToActive)
    {
        Debug.Log("Trying to transform the player character in a mouse...");

        objToDisactive.SetActive(false);
        objToActive.SetActive(true);

        Player.s_Singleton.playerIsTranformedInMouse = true;
        Player.s_Singleton.playerIsTranformedInCat = false;
        Player.s_Singleton.playerIsInHumanForm = false;
    }

    private void Clonage(GameObject objToClone, Transform posToSpawnClone)
    {
        if (!Player.s_Singleton.playerIsInHumanForm)
            return;

        else
        {
            GameObject objToInstantiate = Instantiate(objToClone, posToSpawnClone) as GameObject;
            objToInstantiate.transform.rotation = posToSpawnClone.rotation;
        }
    }

    //private void Dupplication()
    //{
    //    List<Object> tempObjects = new List<Object>();
    //    Debug.Log("Dupplication");
    //    for (int i = 0; i < PlayerObjectsInventory.s_Singleton.objectsCompartments.Count; i++)
    //    {
    //        if (PlayerObjectsInventory.s_Singleton.objectsCompartments[i].MyCompartmentObject == null || PlayerObjectsInventory.s_Singleton.numberOfObjectInInventory == PlayerObjectsInventory.s_Singleton.objectsCompartments.Count)
    //        {
    //            //Peut pas 
    //            Debug.Log("Impossible to dupplicate an object");
    //        }
    //        else if(PlayerObjectsInventory.s_Singleton.objectsCompartments[i].MyCompartmentObject != null)
    //        {
    //            tempObjects.Add(PlayerObjectsInventory.s_Singleton.objectsCompartments[i].MyCompartmentObject);
    //            UIManager.s_Singleton.DisplayAPopup(UIManager.s_Singleton.dupplicationWindow);

    //            for (int j = 0; j < PlayerObjectsInventory.s_Singleton.numberOfObjectInInventory; j++)
    //            {
    //                //Activer 1, 2 ou 3 boutons - ceux contenant des objets
    //                UIManager.s_Singleton.dupplicationButtonLayout.transform.GetChild(j).gameObject.SetActive(true);
    //                UIManager.s_Singleton.dupplicationButtonLayout.transform.GetChild(j).GetComponent<Button>().enabled = true;



    //                UIManager.s_Singleton.dupplicationButtonLayout.transform.GetChild(j).GetChild(0).GetComponent<Image>().sprite = tempObjects.MyObjectIcon;
    //            }

    //            foreach (Button buttonEnabled in collection)
    //            {

    //            }
    //        }

    //    }
        
    //}

    private void Stun()
    {

    }

    private void LockPicking()
    {

    }

    private void Detection()
    {

    }

}
