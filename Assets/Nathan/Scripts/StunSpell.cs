using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunSpell : MonoBehaviour
{

    public List<GameObject> EnnemyInTrigger = new List<GameObject>();

    public List<GameObject> PossibleTargets = new List<GameObject>();

    public GameObject Target;

    float CooldownRaycast;
    public int TargetIndex;

    public Transform middle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CooldownRaycast -= Time.deltaTime;

        if (TargetIndex == PossibleTargets.Count && PossibleTargets.Count > 0)
        {
            TargetIndex = PossibleTargets.Count - 1;
        }

        if (PossibleTargets.Count > 0)
        {
            Target = PossibleTargets[TargetIndex];

            foreach (GameObject ennemy in PossibleTargets)
            {
                if (ennemy == Target)
                {
                    ennemy.GetComponent<Selected>().IsSelected = true;
                }
                else
                {
                    ennemy.GetComponent<Selected>().IsSelected = false;
                }
            }          
        }



        if (Input.GetKeyDown(KeyCode.L))
        {
            if (TargetIndex == 0)
            {
                TargetIndex = PossibleTargets.Count-1;
            }
            else
            {
                TargetIndex--;
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if(TargetIndex == PossibleTargets.Count-1)
            {
                TargetIndex = 0;
            }
            else
            {
                TargetIndex++;
            }
        }


        if (EnnemyInTrigger.Count>0 && CooldownRaycast<=0)
        {
            CooldownRaycast = 1;
            Debug.Log("Ray Launching");
            for (int i = 0; i < EnnemyInTrigger.Count; i++)
            {
                RaycastEnnemies(i);
            }
        }
    }

    void RaycastEnnemies(int i)
    {
        RaycastHit hit;

        if (Physics.Raycast(middle.position, EnnemyInTrigger[i].transform.position - middle.position, out hit))
        {
            Debug.DrawRay(middle.position, EnnemyInTrigger[i].transform.position - middle.position, Color.yellow, 0.5f);

            if (hit.collider.tag == "Ennemy")
            {
                Debug.Log("Ca touhe");

                if (!PossibleTargets.Contains(hit.transform.gameObject))
                {
                    PossibleTargets.Add(EnnemyInTrigger[i]);
                }
            }
            else
            {
                Debug.Log("Ca touche pas");

                EnnemyInTrigger[i].GetComponent<Selected>().IsSelected = false;
                if (!PossibleTargets.Contains(hit.transform.gameObject))
                {
                    PossibleTargets.Remove(EnnemyInTrigger[i]);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ennemy"))
        {
            EnnemyInTrigger.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ennemy"))
        {
            other.GetComponent<Selected>().IsSelected = false;
            EnnemyInTrigger.Remove(other.gameObject);
            PossibleTargets.Remove(other.gameObject);
        }
    }
}
