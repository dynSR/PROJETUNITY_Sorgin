using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object_", order = 1)]
public class Object : ScriptableObject
{
    //Variables des objets contenus dans le jeu
    public enum ObjectType { Stone, Key, Bottle };
    public ObjectType objectType;

    [Header("SETTINGS")]
    [SerializeField] private string objectName;
    [SerializeField] private Sprite objectIcon;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int durationOfEffect;
    [SerializeField] private int objectID;

    public string MyObjectName { get => objectName; }
    public int MyDurationOfEffect { get => durationOfEffect; }
    public Sprite MyObjectIcon { get => objectIcon; set => objectIcon = value; }
    public int MyObjectID { get => objectID; }
    public GameObject ObjectPrefab { get => objectPrefab; }

    public void UseObject()
    {
        if (objectType == Object.ObjectType.Key)
        {
            if (Player.s_Singleton.doorNearPlayerCharacter != null)
            {
                Player.s_Singleton.doorNearPlayerCharacter.UnlockDoor();
            }
            //else
            //{
            //    PlayerObjectsInventory.s_Singleton.CantUseTheObject();
            //}
            
        }
    }
}
