using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPopup : MonoBehaviour
{
    private Animator myAnimator;

    private void Start()
    {
        if ((myAnimator = GetComponent<Animator>()) == null)
            Debug.LogError("You need to assign the Animator attached to the parent gameObject");
        else
            myAnimator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            myAnimator.SetBool("PlayerIsInTrigger", true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            myAnimator.SetBool("PlayerIsInTrigger", false);
    }
}
