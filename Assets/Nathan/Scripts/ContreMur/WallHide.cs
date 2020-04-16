using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHide : MonoBehaviour
{

    public GameObject Wall;
    public GameObject Player;


    Vector3 ClosestPt;

    public GameObject ball; //DEBUG

    Quaternion wantedRotation;
    int layer_mask;
    bool Hided;

    bool Left = true;
    bool Right = true;

    public Detector LeftDetector;
    public Detector RightDetector;
    public Detector BehindDetector;



    // Start is called before the first frame update
    void Start()
    {
        layer_mask = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        if (Wall != null && !Hided)
        {
            ClosestPt = Wall.GetComponent<Collider>().ClosestPoint(transform.position);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, ClosestPt-transform.position, out hit, Mathf.Infinity,layer_mask))
            {
                Debug.Log("bordel ca devrait marcher");
                wantedRotation = Quaternion.LookRotation(hit.normal);


                if (Input.GetKeyDown(KeyCode.M))
                {
                    Invoke("ChangeHided", 0.05f);
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.M) && Hided)
        {
            ChangeHided();
        }

        if (Hided)
        {
            Hide();
        }

        //DEBUG
        ball.transform.position = ClosestPt;
        Debug.DrawRay(transform.position, ClosestPt-transform.position, Color.red);
        //DEBUG

        Player.GetComponent<MoveScript>().OnWall = Hided;

    }

    void Hide()
    {
        Player.transform.rotation = Quaternion.RotateTowards(Player.transform.rotation,wantedRotation,500f*Time.deltaTime);

        Player.GetComponent<MoveScript>().MoveWall(RightDetector.On, LeftDetector.On, BehindDetector.On);

        if (BehindDetector.On == false)
        {
            Player.GetComponent<MoveScript>().WallBack(5);
        }
        else
        {
            Player.GetComponent<MoveScript>().WallBack(0);
        }

    }

    void ChangeHided()
    {
        Hided = !Hided;
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
