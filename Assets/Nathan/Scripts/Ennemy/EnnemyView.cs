using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnnemyView : MonoBehaviour
{

    public CharacterController Player;
    public Transform PlayerPos;

    public NavMeshAgent Nav;

    public Camera Cam;

    public Transform Eye;

    public bool OnTrigger;
    public bool IsVisible;

    float Timer;
    float LostTimer;
    bool Done;
    bool Follow;

    public float Detection;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        Detection = Mathf.Clamp(Detection, 0, 1);

        if (OnTrigger)
        {
            if (Timer < 0)
            {
                Raycast();
                Timer = 0.05f;
            }
        }
        else
        {
            IsVisible = false;
        }

        if (IsVisible)
        {
            Detection += (Time.deltaTime/Vector3.Distance(Eye.position, PlayerPos.position))*4;
            LostTimer = 1;
            if (!Done)
            {
                Done = true;
            }
        }
        else
        {
            Detection -= Time.deltaTime/3;

            if (Done)
            {
                Done = false;
            }
        }


        if(Detection >= 1)
        {
            Follow = true;
        }

        if (Follow && LostTimer>0)
        {
            //Nav.destination = PlayerPos.position;
        }

        if(Detection <= 0)
        {
            LostTimer -= Time.deltaTime;
        }

        DetectionLevel.Instance.DetectionAmount = Detection;

    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTrigger = false;
        }
    }

    void Raycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(Eye.position, PlayerPos.position-Eye.position, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("PlayerBody"))
            {
                IsVisible = true;
            }
            else
            {
                IsVisible = false;
            }
        }
    }
}
