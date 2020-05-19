using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public GameObject UI;
    NavMeshAgent Nav;

    private void Start()
    {
        Nav = transform.parent.GetComponent<NavMeshAgent>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            UI.SetActive(true);
            Nav.enabled = false;
            other.transform.root.GetComponent<MoveScript>().enabled = false;
            Player.s_Singleton.Death = true;
        }
    }
}
