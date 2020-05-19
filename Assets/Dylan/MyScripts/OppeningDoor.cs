using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType { CommonDoor, LastDoor }

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class OppeningDoor : MonoBehaviour
{
    [Header("SETTINGS")]
    public DoorType doorType;
    public bool doorIsLocked = true;
    private Rigidbody parentRigidBody;
    private Animator parentAnimator;

    [Header("WWISE SOUND EVENT NAME")]
    [SerializeField] private string oppeningADoorSFX;
    // Start is called before the first frame update
    private void Awake()
    {
        if ((parentAnimator = GetComponentInParent<Animator>()) == null)
            Debug.LogError("You need to assign the Animator attached to the parent gameObject");
        else
            parentAnimator = GetComponentInParent<Animator>();

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
            parentAnimator.SetBool("PlayerIsInTrigger", true);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!doorIsLocked && other.gameObject.CompareTag("Player"))
        {
            parentAnimator.SetBool("PlayerIsInTrigger", false);
            Player.s_Singleton.doorNearPlayerCharacter = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.s_Singleton.doorNearPlayerCharacter = null;
            parentAnimator.SetBool("PlayerIsInTrigger", false);
        }   
    }

    public void UnlockDoor()
    {
        doorIsLocked = false;
        parentRigidBody.isKinematic = false;
        AkSoundEngine.PostEvent(oppeningADoorSFX, transform.gameObject);
        Player.s_Singleton.doorNearPlayerCharacter = null;
    }
}
