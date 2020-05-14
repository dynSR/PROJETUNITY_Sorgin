using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class MoveScript : MonoBehaviour
{
    public GameObject CameraTPS;
    public GameObject CameraTopDown;
    public CharacterController Controller;
    public Animator Anim;
    public float gravity = 20.0F;

    public SoundSpawner Sound;


    public Vector3 moveDirection = Vector3.zero;


    public float speed;
    public int speedTPS;
    public int sensi;

    private Vector3 targetAngles;
    public int smooth;

    bool turning;
    float Timer;
    public float SoundTimer;
    public float horizontal;
    float vertical;

    public bool OnWall;
    public bool OnArmoire;
    bool ChangedCam = false;


    void Start()
    {

        CameraTPS = GameObject.Find("Cameras").transform.Find("TPS").gameObject;
        CameraTopDown = GameObject.Find("Cameras").transform.Find("TopDown").gameObject;

        CameraTPS.GetComponent<CinemachineVirtualCamera>().m_Follow = transform.Find("Body").transform;
        CameraTPS.GetComponent<CinemachineVirtualCamera>().m_LookAt = transform.Find("Body").transform;

        CameraTopDown.GetComponent<CinemachineVirtualCamera>().m_Follow = transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraTopDown.SetActive(true);
        CameraTPS.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            Anim.SetFloat("Blend", Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")));

            SoundTimer -= Time.deltaTime * (speed);

            if (SoundTimer <= 0)
            {
                Sound.SoundPlay(speed * 4);
                SoundTimer = 2f;
            }

            if (!OnWall)
            {

                vertical = Mathf.Lerp(vertical, Input.GetAxis("Vertical"), 0.05f);
                horizontal = Mathf.Lerp(horizontal, Input.GetAxis("Horizontal"), 0.05f);


                if (Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.05f || Mathf.Abs(Input.GetAxis("Vertical")) >= 0.05f) //Rotation du perso
                {
                    float heading = Mathf.Atan2(horizontal, vertical);
                    transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0);
                } //FIN rotation perso

                moveDirection = new Vector3(horizontal, 0, vertical);
                moveDirection.Normalize();
                Controller.SimpleMove(moveDirection * speed);

                if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                {
                    speed = Mathf.Abs(horizontal) * 5;
                }
                else
                {
                    speed = Mathf.Abs(vertical) * 5;
                }

                ChangedCam = false;
            }
        }
    }

    public void WallCamera()
    {
        CameraTopDown.SetActive(false);
        CameraTPS.SetActive(true);
    }

    public void TopDownCamera()
    {
        CameraTopDown.SetActive(true);
        CameraTPS.SetActive(false);
    }

    public void WallBack(float vert)
    {
        vertical = -vert;
        speed = 0;

    }

    public void MoveWall(bool CanGoRight, bool CanGoLeft, bool ChangeCamera)
    {
        if (!ChangedCam)
        {
            WallCamera();
            ChangedCam = true;
        }


        horizontal = Mathf.Lerp(horizontal, Input.GetAxis("Horizontal"), 0.05f);
        moveDirection = new Vector3(-horizontal, 0, vertical);
        moveDirection = transform.TransformDirection(moveDirection);

        if (horizontal <= 0 && !CanGoRight)
        {
            horizontal = 0;
        }

        if (horizontal >= 0 && !CanGoLeft)
        {
            horizontal = 0;
        }

        if (Input.GetAxis("Horizontal")==0 && !CanGoRight)
        {
            horizontal = 1;
        }

        if (Input.GetAxis("Horizontal") == 0 && !CanGoLeft)
        {
            horizontal = -1;
        }
        Controller.SimpleMove(moveDirection);

    }
}
