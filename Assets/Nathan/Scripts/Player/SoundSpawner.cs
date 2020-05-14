using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSpawner : MonoBehaviour
{

    public GameObject Son;
    public GameObject PrefabSon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    SoundPlay(5);
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    SoundPlay(10);
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    SoundPlay(15);
        //}
    }

    public void SoundPlay(float Taille)
    {
        Son = Instantiate(PrefabSon, transform.position, Quaternion.Euler(90,0,0));
        Son.GetComponent<SonScale>().range = Taille;
    }
}
