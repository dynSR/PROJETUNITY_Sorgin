using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnnemyView : MonoBehaviour
{
    public List<Transform> Waypoints;
    public Transform Waypoint;

    public Vector3 Destination;
    Transform PlayerPos;

    public NavMeshAgent Nav;

    public Transform Eye;

    public bool OnTrigger;
    public bool IsVisible;
    public float WaitingTime;

    float Timer;
    float WaypointTimer;
    float LostTimer;
    float SoundTimer;


    bool Done;
    bool Follow;

    public float Detection;

    // Start is called before the first frame update
    void Start()
    {
        Waypoint = Waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            Timer -= Time.deltaTime;
            Detection = Mathf.Clamp(Detection, 0, 1.1f);

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
                Detection += (Time.deltaTime / Vector3.Distance(Eye.position, PlayerPos.position)) * 6;
                LostTimer = 1;
                Destination = PlayerPos.position;
                if (!Done)
                {
                    Done = true;
                }
            }
            else
            {
                Detection -= Time.deltaTime / 3;

                if (Done)
                {
                    Done = false;
                }
            }


            if (Detection >= 1)
            {
                LostTimer = 1;
                Follow = true;
            }

            if (Follow && LostTimer > 0)
            {
                Nav.destination = Destination;
            }

            if (Detection <= 0)
            {
                LostTimer -= Time.deltaTime;
            }

            if (LostTimer <= 0 && WaypointTimer <= 0)
            {
                Nav.destination = Waypoint.position;
                WaypointTimer = WaitingTime;
            }

            if (!Nav.pathPending)
            {
                if (Nav.remainingDistance <= Nav.stoppingDistance)
                {
                    if (!Nav.hasPath || Nav.velocity.sqrMagnitude == 0f)
                    {
                        Waypoint = Waypoints[Random.Range(0, Waypoints.Count)];
                        WaypointTimer -= Time.deltaTime;
                    }
                }
            }
            DetectionLevel.Instance.DetectionAmount = Detection;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTrigger = true;
            PlayerPos = other.transform.Find("Middle");
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
