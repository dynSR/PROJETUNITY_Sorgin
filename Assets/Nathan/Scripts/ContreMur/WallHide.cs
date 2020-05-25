using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHide : MonoBehaviour
{

    public GameObject Wall;
    public GameObject playerCharacter;


    Vector3 ClosestPt;

    public GameObject TextOnWall; //DEBUG
    public GameObject TextOnWallPrefab;

    public Quaternion wantedRotation;
    int layer_mask;
    bool Hided;

    bool Waitforrotation;

    public Detector LeftDetector;
    public Detector RightDetector;
    public Detector BehindDetector;

    float Timer;


    // Start is called before the first frame update
    void Start()
    {
        TextOnWall = Instantiate(TextOnWallPrefab);
        layer_mask = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            Timer -= Time.deltaTime;

            if (Wall != null && !Hided)
            {
                ClosestPt = Wall.GetComponent<Collider>().ClosestPoint(transform.position);

                RaycastHit hit;
                if (Physics.Raycast(transform.position, ClosestPt - transform.position, out hit, Mathf.Infinity, layer_mask))
                {
                    wantedRotation = Quaternion.LookRotation(hit.normal);

                    if (hit.transform.CompareTag("Wall"))
                    {
                        TextOnWall.SetActive(true);
                        if ((ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_X") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_A")) && Player.s_Singleton.inWardrobe == false)
                        {
                            if (Timer <= 0 && !Player.s_Singleton.canPickObject)
                            {
                                Invoke("ChangeHided", 0.05f);
                                Timer = 1;
                                Invoke("ChangeWaitforRot", 0.25f);

                            }
                        }
                    }
                    else
                    {
                        TextOnWall.SetActive(false);
                    }
                }
            }
            else
            {
                TextOnWall.SetActive(false);
            }


            if ((ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_X") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_A")) && Hided)
            {
                if (Timer <= 0)
                {
                    playerCharacter.GetComponent<MoveScript>().TopDownCamera();

                    Invoke("ChangeHided", 0.5f);
                    Timer = 1;
                    Waitforrotation = false;

                }
            }

            if (Hided)
            {
                TextOnWall.SetActive(false);

                playerCharacter.transform.rotation = Quaternion.RotateTowards(playerCharacter.transform.rotation, wantedRotation, 500f * Time.deltaTime);

                if (Waitforrotation)
                {
                    Hide();
                }
            }

            //DEBUG

            TextOnWall.transform.position = ClosestPt;

            Debug.DrawRay(transform.position, ClosestPt - transform.position, Color.red);
            //DEBUG

            Player.s_Singleton.onWall = Hided;
        }
        

    }

    void ChangeWaitforRot()
    {
        Waitforrotation = true;
    }

    void Hide()
    {

        playerCharacter.GetComponent<MoveScript>().MoveWall(RightDetector.On, LeftDetector.On, BehindDetector.On);

        if (BehindDetector.On == false)
        {
            playerCharacter.GetComponent<MoveScript>().WallBack(5);
        }
        else
        {
            playerCharacter.GetComponent<MoveScript>().WallBack(0);
        }

    }

    void ChangeHided()
    {
        Hided = !Hided;
        Debug.Log("Changed");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Wall = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Wall = null;
        }
    }
}
