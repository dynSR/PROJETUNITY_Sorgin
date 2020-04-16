using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType { TopDown, TPS }

public class MoveScript : MonoBehaviour
{
    public MoveType myType;

    public GameObject CameraTPS;
    public GameObject CameraTopDown;
    public CharacterController Controller;
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
    float horizontal;
    float vertical;

    public bool OnWall;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (myType == MoveType.TPS)
            {
                myType = MoveType.TopDown;
            }

            else if (myType == MoveType.TopDown)
            {
                myType = MoveType.TPS;
            }
        }

        /*if (myType == MoveType.TPS)
        {
            CameraTPS.SetActive(true);
            CameraTopDown.SetActive(false);

            transform.Rotate(new Vector3(0,Input.GetAxis("Mouse X"), 0) * Time.deltaTime * sensi);

            if (Controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speedTPS;
            }
            //moveDirection.y -= gravity * Time.deltaTime;
            Controller.SimpleMove(moveDirection);

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                turning = true;
            }

            if (turning)
            {
                Timer += Time.deltaTime;
                targetAngles = transform.eulerAngles + 180f * Vector3.up;
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, smooth * Time.deltaTime);
                if (Timer > 0.125f)
                {
                    Timer = 0;
                    turning = false;
                }
            }
        }*/

        SoundTimer -= Time.deltaTime*(speed);

        if(SoundTimer <= 0)
        {
            Sound.SoundPlay(speed * 2);
            SoundTimer = 2f;
        }

        if (myType == MoveType.TopDown && !OnWall)
        {
            CameraTopDown.SetActive(true);
            CameraTPS.SetActive(false);

            vertical = Mathf.Lerp(vertical, Input.GetAxis("Vertical"), 0.05f);
            horizontal = Mathf.Lerp(horizontal, Input.GetAxis("Horizontal"), 0.05f);


            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) //Rotation du perso
            {
                float heading = Mathf.Atan2(horizontal, vertical);
                transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0);
            } //FIN rotation perso

            moveDirection = new Vector3(horizontal, 0, vertical);
            moveDirection.Normalize();
            Controller.SimpleMove(moveDirection * speed);

        }

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            speed = Mathf.Abs(horizontal)*5;
        }
        else
        {
            speed = Mathf.Abs(vertical)*5;
        }

    }

    public void WallBack(float vert)
    {
        vertical = -vert;
    }

    public void MoveWall(bool CanGoRight, bool CanGoLeft, bool ChangeCamera)
    {
        if (ChangeCamera)
        {
            CameraTopDown.SetActive(false);
            CameraTPS.SetActive(true);
        }

        horizontal = Mathf.Lerp(horizontal, Input.GetAxis("Horizontal"), 0.05f);
        moveDirection = new Vector3(-horizontal, 0, vertical);
        moveDirection = transform.TransformDirection(moveDirection);

        if (horizontal <= 0 && CanGoRight)
        {
            Controller.SimpleMove(moveDirection);
        }
        if (horizontal >= 0 && CanGoLeft)
        {
            Controller.SimpleMove(moveDirection);
        }
    }
}
