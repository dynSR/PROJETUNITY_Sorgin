using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    //Variables des objets contenus dans le jeu
    private enum ObjectType { Stone, Key, Bottle };
    [SerializeField] private ObjectType objectType;

    [SerializeField] private string objectName;
    [SerializeField] private Sprite objectIcon;
    [SerializeField] private int durationOfEffect;

    public string MyObjectName { get => objectName; }
    public int MyDurationOfEffect { get => durationOfEffect; }
    public Sprite MyObjectIcon { get => objectIcon; set => objectIcon = value; }

    public void UseTheObject()
    {
        switch (objectType)
        {
            case ObjectType.Stone:
                break;
            case ObjectType.Key:
                break;
            case ObjectType.Bottle:
                break;
            default:
                break;
        }

    }
}
