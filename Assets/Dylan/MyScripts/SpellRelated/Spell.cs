using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    //Variables des sorts contenus dans le jeu
    [SerializeField] private string spellName;
    [SerializeField] private Sprite spellIcon;
    [SerializeField] private int spellCooldown;
    [SerializeField] private int spellValue;
    [TextArea(1, 5)]
    [SerializeField] private string spellEffectDescription;

    public string MySpellName { get => spellName; }
    public int MySpellCooldown { get => spellCooldown; }
    public int MySpellValue { get => spellValue; }
    public Sprite MySpellIcon { get => spellIcon; set => spellIcon = value; }
    public string MySpellEffectDescription { get => spellEffectDescription; set => spellEffectDescription = value; }
}
