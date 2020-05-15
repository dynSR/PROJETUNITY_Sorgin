using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { CommonDoor, LastDoor }
public class OppeningDoor : MonoBehaviour
{
    public DoorType doorType;
    public bool doorIsLocked = true;
    Rigidbody parentRigidBody;
    Animator parentAnimator;
    // Start is called before the first frame update
    void Start()
    {
        parentAnimator = GetComponentInParent<Animator>();
        parentRigidBody = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        parentRigidBody.isKinematic = doorIsLocked;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && doorIsLocked)
        {
            PlayerObjectsInventory.s_Singleton.doorNearPlayerCharacter = this;
            parentAnimator.SetBool("PlayerIsInTrigger", true);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!doorIsLocked && other.gameObject.CompareTag("Player"))
        {
            parentAnimator.SetBool("PlayerIsInTrigger", false);
            PlayerObjectsInventory.s_Singleton.doorNearPlayerCharacter = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerObjectsInventory.s_Singleton.doorNearPlayerCharacter = null;
            parentAnimator.SetBool("PlayerIsInTrigger", false);
        }   
    }

    public void UnlockDoor()
    {
        doorIsLocked = false;
        parentRigidBody.isKinematic = false;
    }
}
