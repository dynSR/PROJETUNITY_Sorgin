using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideEnnemyView : MonoBehaviour
{

    public EnnemyView EnnemiScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnnemiScript.OnTrigger = true;
            EnnemiScript.PlayerPos = other.transform.Find("Middle");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnnemiScript.OnTrigger = false;
        }
    }
}
