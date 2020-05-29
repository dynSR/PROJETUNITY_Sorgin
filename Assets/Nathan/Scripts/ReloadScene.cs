using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    NavMeshAgent Nav;
    EnnemyView EnnemiScript;

    public Animator Anim;

    private void Start()
    {
        Nav = transform.parent.GetComponent<NavMeshAgent>();
        EnnemiScript = transform.parent.GetComponent<EnnemyView>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Scene_MainMenu");
            Nav.enabled = false;
            other.transform.root.GetComponent<MoveScript>().enabled = false;
            Player.s_Singleton.isDead = true;
            EnnemiScript.enabled = false;
            Anim.SetBool("run", false);
            Anim.SetBool("walk", false);
        }
    }
}
