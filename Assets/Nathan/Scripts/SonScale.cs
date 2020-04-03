using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonScale : MonoBehaviour
{

    public float Taille;
    public Outline Outliner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(Taille,0.1f,Taille), 2f * Time.deltaTime);
        if (Outliner.OutlineWidth > 0)
        {
            Outliner.OutlineWidth -= 10f*Time.deltaTime;
        }
        else
        {
            Outliner.OutlineWidth =0;
        }
    }
}
