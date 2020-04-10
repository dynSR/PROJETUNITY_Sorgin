using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private string spellName;
    [SerializeField] private Sprite spellIcon;
    [SerializeField] private int spellCooldown;
    [SerializeField] private int spellValue;

    public string MySpellName { get => spellName; }
    public int MySpellCooldown { get => spellCooldown; }
    public int MySpellValue { get => spellValue; }
    public Sprite MySpellIcon { get => spellIcon; set => spellIcon = value; }
}
