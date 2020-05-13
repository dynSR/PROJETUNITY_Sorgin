using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCompartment : MonoBehaviour
{
    [SerializeField] private Object compartmentObject;

    public Object MyCompartmentObject { get => compartmentObject; set => compartmentObject = value; }
}
