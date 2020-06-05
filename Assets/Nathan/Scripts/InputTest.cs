using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    public float Delay;
    float Timer;
    private void Start()
    {
        Timer = Delay;
    }
    void Update()
    {
        if (Timer <= 0)
        {
            if (ConnectedController.s_Singleton.XboxControllerIsConnected)
            {
                Debug.Log("Left Trigger Value = " + Input.GetAxis("XBOX_LT"));
                Debug.Log("Right Trigger Value = " + Input.GetAxis("XBOX_RT"));

                Debug.Log("Right Joystick Horizontal = " + Input.GetAxis("XBOX_RStick_Horizontal"));
                Debug.Log("Right Joystick Vertical = " + Input.GetAxis("XBOX_RStick_Vertical"));

                Debug.Log("Left Joystick Horizontal = " + Input.GetAxis("XBOX_LStick_Horizontal"));
                Debug.Log("Left Joystick Vertical = " + Input.GetAxis("XBOX_LStick_Vertical"));
            }

            Timer = Delay;
        }

    }
}
