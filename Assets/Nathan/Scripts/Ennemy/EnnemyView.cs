﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnnemyView : MonoBehaviour
{
    public List<Transform> Waypoints;
    public Transform Waypoint;

    public Vector3 Destination;
    public Transform PlayerPos;

    public NavMeshAgent Nav;
    public Animator Anim;
    public Transform Eye;
    public LookAt LookatScript;

    public bool OnTrigger;
    public bool IsVisible;
    public float WaitingTime;

    float Timer;
    float WaypointTimer;
    float LostTimer;
    float SoundTimer;

    public bool Stunned;
    float StunDuration;

    bool Done;
    bool Follow;

    int WaypointLevel;

    public float Detection;

    // Start is called before the first frame update
    void Start()
    {
        Waypoint = Waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Stunned)
        {
            StunDuration -= Time.deltaTime;
            Detection = 0;
            DetectionLevel.Instance.Detection(gameObject.name, Detection);

            if (StunDuration <= 0)
            {
                Anim.SetBool("Stun", false);
                Stunned = false;
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
            }
        }

        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            Timer -= Time.deltaTime;

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

            if (IsVisible && !Stunned)
            {
                Detection += (Time.deltaTime / Vector3.Distance(Eye.position, PlayerPos.position)) *4f;
                LostTimer = 1;
                Destination = PlayerPos.position;
                if (!Done)
                {
                    Done = true;
                }
            }
            if(!IsVisible)
            {
                if (Follow)
                {
                    Detection -= Time.deltaTime / 12f;

                }
                else
                {
                    Detection -= Time.deltaTime / 5;
                }
                Detection = Mathf.Clamp(Detection, 0, 1.1f);

                if (Done)
                {
                    Done = false;
                }
            }


            if (Detection >= 0.75f)
            {
                LostTimer = 1;
                Follow = true;
            }

            if (Detection >= 0.5f)
            {
                LookatScript.m_Target = PlayerPos.gameObject;
                LookatScript.enabled = true;
            }

            if (Follow && LostTimer > 0)
            {
                Nav.destination = Destination;
                Nav.speed = 4f;
                Anim.SetBool("run", true);
            }

            if (Detection <= 0)
            {
                LostTimer -= Time.deltaTime;
            }

            if (LostTimer <= 0 && WaypointTimer <= 0)
            {
                Follow = false;
                LookatScript.enabled = false;
                Anim.SetBool("run", false);
                Anim.SetBool("walk", true);
                Nav.speed = 2f;
                Waypoint = Waypoints[WaypointLevel];
                Nav.destination = Waypoint.position;
                WaypointLevel++;
                if (WaypointLevel == Waypoints.Count)
                {
                    WaypointLevel = 0;
                }
                WaypointTimer = WaitingTime;
            }

            if (Nav.enabled == true)
            {
                if (!Nav.pathPending)
                {
                    if (Nav.remainingDistance <= Nav.stoppingDistance)
                    {
                        if (!Nav.hasPath || Nav.velocity.sqrMagnitude == 0f)
                        {
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, Waypoint.transform.rotation, 15);
                            WaypointTimer -= Time.deltaTime;
                            Anim.SetBool("walk", false);
                            Anim.SetBool("run", false);
                        }
                    }
                }
            }

            DetectionLevel.Instance.Detection(gameObject.name, Detection);
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

    public void Stun(float Duration)
    {
        Anim.SetBool("Stun", true);
        StunDuration = Duration;
        Stunned = true;
    }
}
