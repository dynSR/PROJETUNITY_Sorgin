using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { CommonDoor, LastDoor }

[RequireComponent(typeof(Rigidbody))]
public class OppeningDoor : MonoBehaviour
{
    [Header("SETTINGS")]
    [SerializeField] private GameObject interactionPopup;
    public DoorType doorType;
    public bool doorIsLocked = true;
    private Rigidbody parentRigidBody;
    

    [Header("WWISE SOUND EVENT NAME")]
    [SerializeField] private string oppeningADoorSFX;
    // Start is called before the first frame update
    private void Awake()
    {
        if ((parentRigidBody = GetComponentInParent<Rigidbody>()) == null)
            Debug.LogError("You need to assign the Rigidbody attached to the parent gameObject");
        else
        {
            parentRigidBody = GetComponentInParent<Rigidbody>();
            parentRigidBody.isKinematic = doorIsLocked;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && doorIsLocked)
        {
            Player.s_Singleton.doorNearPlayerCharacter = this;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.s_Singleton.doorNearPlayerCharacter = null;
        }   
    }

    public void UnlockDoor()
    {
        doorIsLocked = false;
        parentRigidBody.isKinematic = false;
        AkSoundEngine.PostEvent(oppeningADoorSFX, transform.gameObject);
        Player.s_Singleton.doorNearPlayerCharacter = null;
        interactionPopup.SetActive(false);
    }
}
