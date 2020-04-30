using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConnectedController : MonoBehaviour
{
    private int Xbox_One_Controller = 0;
    private int PS4_Controller = 0;

    void Awake()
    {
        CheckWhatTypeOfControllerIsConnected();
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
            }
            if (names[x].Length == 33)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4_Controller = 0;
                Xbox_One_Controller = 1;
            }
        }
    }
}
