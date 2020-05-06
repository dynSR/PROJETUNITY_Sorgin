using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedController : MonoBehaviour
{
    private int Xbox_One_Controller = 0;
    private int PS4_Controller = 0;

    public bool PS4ControllerIsConnected = false;
    public bool XboxControllerIsConnected = false;

    public static ConnectedController s_Singleton;

    void Awake()
    {
        if (s_Singleton != null)
        {
            Destroy(this);
        }
        else
        {
            s_Singleton = this;
        }
    }

    private void Start()
    {
        InvokeRepeating("CheckWhatTypeOfControllerIsConnected", 0, 2f);
    }

    void CheckWhatTypeOfControllerIsConnected()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                PS4_Controller = 1;
                Xbox_One_Controller = 0;
                PS4ControllerIsConnected = true;
            }
            else if(names[x].Length == 33)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                PS4_Controller = 0;
                Xbox_One_Controller = 1;
                XboxControllerIsConnected = true;
            }
            else
            {
                Debug.LogError("NO CONTROLLER CONNECTED");
                Time.timeScale = 0;
            }
        }
    }
}
