using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationBase : MonoBehaviour
{
    public List<GameObject> StartPos;
    public List<GameObject> SpecialRoomPos;
    public List<GameObject> EndPos;
    public List<GameObject> BasicRoomPos;

    public GameObject StartPrefab;
    public GameObject EndPrefab;
    public List<GameObject> SpecialRoomPrefab;
    public List<GameObject> BasicRoomPrefab;


    // Start is called before the first frame update
    void Start()
    {
        StartSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartSpawn();
        }
    }

    void StartSpawn()
    {
        int Rand = Random.Range(0, StartPos.Count);
        Instantiate(StartPrefab, StartPos[Rand].transform.position,Quaternion.identity,StartPos[Rand].transform);
    }
}
