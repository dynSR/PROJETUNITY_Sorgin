using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    //Variables des sorts contenus dans le jeu
    public enum SpellType { Etourdissement, Crochetage, Radar, Duplication, Clone, TransformationEnSouris, TransformationEnChat };
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

    //public void UseTheSpell()
    //{
    //    switch (spellType)
    //    {
    //        case SpellType.Etourdissement:
    //            break;
    //        case SpellType.Crochetage:
    //            break;
    //        case SpellType.Radar:
    //            break;
    //        case SpellType.Duplication:
    //            break;
    //        case SpellType.Clone:
    //            break;
    //        case SpellType.TransformationEnSouris:
    //            PlayerSpellsInventory.s_Singleton.MouseTransformation(playerCharacter, mouseCharacterModel);
    //            PlayerSpellsInventory.s_Singleton.playerIsTranformed = true;
    //            break;
    //        case SpellType.TransformationEnChat:
    //            PlayerSpellsInventory.s_Singleton.CatTransformation(playerCharacter, catCharacterModel);
    //            PlayerSpellsInventory.s_Singleton.playerIsTranformed = true;
    //            break;
    //        default:
    //            break;
    //    }
    //}

}
