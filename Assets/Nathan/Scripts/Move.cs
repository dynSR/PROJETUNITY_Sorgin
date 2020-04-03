using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    public NavMeshAgent Nav;
    public GameObject Pos;
    public LayerMask Layer;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveCharacter();
        }
    }

    void MoveCharacter()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,Mathf.Infinity,Layer))
        {
            Nav.destination = hit.point;
        }
    }
}
