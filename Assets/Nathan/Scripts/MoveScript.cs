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


    private Vector3 moveDirection = Vector3.zero;



    public int speed;
    public int speedTPS;
    public int sensi;

    private Vector3 targetAngles;
    public int smooth;

    bool turning;
    float Timer;

    // Start is called before the first frame update
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

        if (myType == MoveType.TPS)
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
            moveDirection.y -= gravity * Time.deltaTime;
            Controller.Move(moveDirection * Time.deltaTime);

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


        }
        if (myType == MoveType.TopDown)
        {
            CameraTopDown.SetActive(true);
            CameraTPS.SetActive(false);

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Controller.SimpleMove(new Vector3(horizontal * Time.deltaTime * speed, 0, vertical * Time.deltaTime * speed));

            if (Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") != 0)
            {
                transform.eulerAngles = new Vector3(0, Mathf.Atan2(horizontal, vertical) * 180 / Mathf.PI, 0);  
            }



        }

    }

    private void FixedUpdate()
    {
        if (myType == MoveType.TPS)
        {
            //Rb.AddForce(Vector3.forward * speed);
        }
    }
}
