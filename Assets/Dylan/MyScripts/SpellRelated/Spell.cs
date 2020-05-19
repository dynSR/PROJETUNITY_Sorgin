using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
                Duplication();
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

    private void Duplication()
    {
        //Refs
        UIManager uiManagerReference = UIManager.s_Singleton;
        PlayerObjectsInventory playerObjInventoryRef = PlayerObjectsInventory.s_Singleton;

        Debug.Log("Dupplication");

        for (int i = 0; i < playerObjInventoryRef.objectsCompartments.Count; i++)
        {
            if (playerObjInventoryRef.objectsCompartments[i].MyCompartmentObject != null)
            {
                //Open the Window
                uiManagerReference.DisplayAPopup(uiManagerReference.duplicationWindow);
                uiManagerReference.duplicationValidationPopupIsDisplayed = true;

                //Activation des boutons et attribution du bouton sélectionné par l'Event system
                uiManagerReference.duplicationButtonLayout.transform.GetChild(i).gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(uiManagerReference.duplicationButtonLayout.transform.GetChild(0).gameObject);

                //Attribution de l'objet trouvé 
                uiManagerReference.duplicationButtonLayout.transform.GetChild(i).GetComponent<DuplicationButtons>().objectFound = playerObjInventoryRef.objectsCompartments[i].MyCompartmentObject;

                uiManagerReference.duplicationButtonLayout.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = uiManagerReference.duplicationButtonLayout.transform.GetChild(i).GetComponent<DuplicationButtons>().objectFound.MyObjectIcon;
                EventSystem.current.SetSelectedGameObject(uiManagerReference.duplicationButtonLayout.transform.GetChild(0).gameObject);
            }
        }
    }

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
