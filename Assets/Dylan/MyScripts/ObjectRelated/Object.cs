using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object_", order = 1)]
public class Object : ScriptableObject
{
    //Variables des objets contenus dans le jeu
    private enum ObjectType { Stone, Key, Bottle };
    [SerializeField] private ObjectType objectType;

    [SerializeField] private string objectName;
    [SerializeField] private Sprite objectIcon;
    [SerializeField] private GameObject objectPrefab;
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
