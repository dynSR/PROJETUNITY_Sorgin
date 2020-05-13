using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    //Variables des sorts contenus dans le jeu
    private enum SpellType { Etourdissement, Crochetage, Radar, Duplication, Clone, TransformationEnSouris, TransformationEnChat };
    [SerializeField] private SpellType spellType;
   
    [SerializeField] private string spellName;
    [SerializeField] private Sprite spellIcon;
    [SerializeField] private int durationOfEffect;
    [SerializeField] private int spellValue;
    [TextArea(1, 5)]
    [SerializeField] private string spellEffectDescription;

    [SerializeField] private Transform playerCharacter;
    [SerializeField] private GameObject defaultCharacterModel;
    [SerializeField] private GameObject mouseCharacterModel;
    [SerializeField] private GameObject catCharacterModel;

    public string MySpellName { get => spellName; }
    public int MyDurationOfEffect { get => durationOfEffect; }
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
                MouseTransformation(playerCharacter, mouseCharacterModel);
                break;
            case SpellType.TransformationEnChat:
                CatTransformation(playerCharacter, catCharacterModel);
                break;
            default:
                break;
        }
    }


    public void CatTransformation(Transform activePlayerCharacter, GameObject newPlayerCharacterModel)
    {
        Debug.Log("Trying to transform the player character in a cat...");

        GameObject activeCharacterModel = activePlayerCharacter.GetChild(0).gameObject;
        Destroy(activeCharacterModel);

        GameObject modelToSwitchTo = Instantiate(newPlayerCharacterModel, activePlayerCharacter) as GameObject;
        modelToSwitchTo.transform.SetParent(activePlayerCharacter);
    }

    public void MouseTransformation(Transform activePlayerCharacter, GameObject newPlayerCharacterModel)
    {
        Debug.Log("Trying to transform the player character in a mouse...");
        GameObject activeCharacterModel = activePlayerCharacter.GetChild(0).gameObject;
        Destroy(activeCharacterModel);

        GameObject modelToSwitchTo = Instantiate(newPlayerCharacterModel, activePlayerCharacter) as GameObject;
        modelToSwitchTo.transform.SetParent(activePlayerCharacter);
    }

}
