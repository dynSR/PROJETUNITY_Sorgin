using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCompartment : MonoBehaviour
{
    [SerializeField] private Spell compartmentSpell;

    public Spell MyCompartmentSpell { get => compartmentSpell; set => compartmentSpell = value; }
}
