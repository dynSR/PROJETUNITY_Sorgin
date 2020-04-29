using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{

    public bool On;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            On = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            On = false;
        }
    }
}
