using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
public class ArmoireAnimation : MonoBehaviour
{

    GameObject Player;

    public GameObject PlayerPositionHolder;
    public Animator Anim;
    public CinemachineVirtualCamera CamArmoire;
    public float Speed;


    bool Lerp;
    bool Inside;
    bool Animating;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Lerp)
        {
            Player.transform.position = Vector3.MoveTowards(Player.transform.position, PlayerPositionHolder.transform.position, Time.deltaTime * Speed);
            Player.transform.rotation = Quaternion.RotateTowards(Player.transform.rotation, PlayerPositionHolder.transform.rotation, Speed*Time.deltaTime*100);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Space) && !Animating && !Inside)
            {
                Animating = true;
                Player = other.gameObject;
                LaunchAnim();
            }

            if (Input.GetKeyDown(KeyCode.Space) && !Animating && Inside)
            {
                Animating = true;
                Player = other.gameObject;
                OutAnim();
            }
        }
    }

    void LaunchAnim()
    {
        Player.GetComponent<MoveScript>().enabled = false;
        Lerp = true;
        Invoke("Animate", 0.2f);
        Invoke("CameraOn", 1.5f);
    }

    void OutAnim()
    {
        Invoke("AnimateOut", 0.2f);
        Invoke("CameraOff", 1.5f);
    }

    void Animate()
    {
        //Player.transform.parent = PlayerPositionHolder.transform;
        Anim.SetBool("Enter", true);
        Player.GetComponentInChildren<Animator>().SetBool("Enter", true);
        //Director.Play();
    }
    void AnimateOut()
    {
        Anim.SetBool("Enter", false);
        Player.GetComponentInChildren<Animator>().SetBool("Enter", false);
    }

    void CameraOff()
    {
        CamArmoire.Priority = 8;
        Player.transform.parent = null;
        Player.GetComponent<MoveScript>().enabled = true;
        Animating = false;
        Inside = false;
        Lerp = false;
    }

    void CameraOn()
    {
        CamArmoire.Priority = 12;
        Animating = false;
        Inside = true;
    }
}
