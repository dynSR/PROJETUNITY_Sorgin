using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class MoveScript : MonoBehaviour
{
    GameObject CameraTPS;
    GameObject CameraTopDown;
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

    bool OnWall;
    public GameObject WallLight;
    public GameObject StandardView;
    bool CanGoR;
    bool CanGoL;

    bool OnArmoire;
    bool ChangedCam = false;


    void Start()
    {

        CameraTPS = GameObject.Find("Cameras").transform.Find("TPS").gameObject;
        CameraTopDown = GameObject.Find("Cameras").transform.Find("TopDown").gameObject;


        CameraTopDown.GetComponent<CinemachineVirtualCamera>().m_Follow = transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraTopDown.SetActive(true);
        CameraTPS.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode && !MapHandler.s_Singleton.mapIsDisplayed)
        {
            OnArmoire = Player.s_Singleton.inWardrobe;
            OnWall = Player.s_Singleton.onWall;

            Anim.SetFloat("Blend", Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            StandardView.SetActive(true);
            WallLight.SetActive(false);

            SoundTimer -= Time.deltaTime * (speed);

            if (SoundTimer <= 0)
            {
                Sound.SoundPlay(speed * 2.5f);
                SoundTimer = 2f;
            }

            if (!OnWall)
            {
                Anim.SetBool("Lean", false);

                if (ConnectedController.s_Singleton.PS4ControllerIsConnected)
                {
                    vertical = Mathf.MoveTowards(vertical, Input.GetAxis("PS4_LStick_Vertical"), 0.1f);
                    horizontal = Mathf.MoveTowards(horizontal, Input.GetAxis("PS4_LStick_Horizontal"), 0.1f);
                }
                if (ConnectedController.s_Singleton.XboxControllerIsConnected)
                {
                    vertical = Mathf.MoveTowards(vertical, Input.GetAxis("XBOX_LStick_Vertical"), 0.1f);
                    horizontal = Mathf.MoveTowards(horizontal, Input.GetAxis("XBOX_LStick_Horizontal"), 0.1f);                
                }


                if (Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) >= 0.1f) //Rotation du perso
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
            else
            {
                StandardView.SetActive(false);
                WallLight.SetActive(true);
                Anim.SetBool("Lean", true);

                horizontal = Mathf.MoveTowards(horizontal, Input.GetAxis("Horizontal"), 0.05f);
                moveDirection = new Vector3(-horizontal, 0, vertical);
                moveDirection = transform.TransformDirection(moveDirection);

                if (horizontal <= 0 && !CanGoR)
                {
                    horizontal = 0;
                    Anim.SetBool("L", true);
                }
                else
                {
                    Anim.SetBool("L", false);
                }

                if (horizontal >= 0 && !CanGoL)
                {
                    horizontal = 0;
                    Anim.SetBool("R", true);
                }
                else
                {
                    Anim.SetBool("R", false);
                }

                if (Input.GetAxis("Horizontal") == 0 && !CanGoR)
                {
                    horizontal = 1;
                }

                if (Input.GetAxis("Horizontal") == 0 && !CanGoL)
                {
                    horizontal = -1;
                }
                Controller.SimpleMove(moveDirection * 1.5f);
            }
        }
    }

    public void WallCamera()
    {
        CameraTopDown.SetActive(false);
        CameraTPS.SetActive(true);

        OnWall = !OnWall;
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
        CanGoR = CanGoRight;
        CanGoL = CanGoLeft;
    }
}
