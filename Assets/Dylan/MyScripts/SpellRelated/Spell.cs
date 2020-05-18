using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                break;
            case SpellType.Crochetage:
                break;
            case SpellType.Radar:
                break;
            case SpellType.Duplication:
                break;
            case SpellType.Clone:
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

    public void CatTransformation(GameObject objToDisactive, GameObject objToActive)
    {
        Debug.Log("Trying to transform the player character in a cat...");

        objToDisactive.SetActive(false);
        objToActive.SetActive(true);

        Player.s_Singleton.playerIsTranformedInCat = true;
        Player.s_Singleton.playerIsTranformedInMouse = false;
        Player.s_Singleton.playerIsInHumanForm = false;
    }

    public void MouseTransformation(GameObject objToDisactive, GameObject objToActive)
    {
        Debug.Log("Trying to transform the player character in a mouse...");

        objToDisactive.SetActive(false);
        objToActive.SetActive(true);

        Player.s_Singleton.playerIsTranformedInMouse = true;
        Player.s_Singleton.playerIsTranformedInCat = false;
        Player.s_Singleton.playerIsInHumanForm = false;
    }

}
