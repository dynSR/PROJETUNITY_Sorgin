using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnhideRoom : MonoBehaviour
{
    public Image Img;
    bool Inside;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Inside)
        {
            Img.color = new Color(Img.color.r, Img.color.g, Img.color.b, Mathf.MoveTowards(Img.color.a, 0, 0.001f));
        }
        else
        {
            Img.color = new Color(Img.color.r, Img.color.g, Img.color.b, Mathf.MoveTowards(Img.color.a, 1, 0.001f));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inside = false;
        }
    }
}
