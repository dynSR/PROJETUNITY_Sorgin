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
        if (Input.GetKeyDown(KeyCode.V))
        {
            SoundPlay(5);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SoundPlay(10);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SoundPlay(15);
        }
    }

    void SoundPlay(float Taille)
    {
        if (Son != null)
        {
            Destroy(Son.gameObject);
        }
        Son = Instantiate(PrefabSon, transform.position, transform.rotation);
        Son.GetComponent<SonScale>().Taille = Taille;
    }
}
