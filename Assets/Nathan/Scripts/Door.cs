using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{

    public bool Locked;
    public GameObject ImageHide;

    public Slider VerrouLife;
    Rigidbody Rb;

    bool InTrigger;
    public float Verrou;

    public GameObject Clé;
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && InTrigger)
        {
            Verrou -= 20;
        }

        Verrou = Mathf.MoveTowards(Verrou, 100, 0.25f);

        if (Verrou <= 0)
        {
            Locked = false;
        }

        if (Verrou >= 100)
        {
            VerrouLife.gameObject.SetActive(false);
        }

        VerrouLife.value = Verrou;

        if (Locked)
        {
            ImageHide.SetActive(true);
            Rb.isKinematic = true;
            if (Verrou < 100)
            {
                VerrouLife.gameObject.SetActive(true);
            }
        }

        else
        {
            VerrouLife.gameObject.SetActive(false);
            ImageHide.SetActive(false);
            Rb.isKinematic = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InTrigger = false;
        }
    }
}
