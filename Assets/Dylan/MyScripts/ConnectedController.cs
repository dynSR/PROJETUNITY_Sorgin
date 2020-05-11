using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectedController : MonoBehaviour
{
    [SerializeField] private string PS4ValidationButtonName = "Validation_";
    [SerializeField] private string XBOXValidationButtonName = "Validation_";

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
        InvokeRepeating("CheckWhatTypeOfControllerIsConnected", 1, 2f);
    }

    void CheckWhatTypeOfControllerIsConnected()
    {
        StandaloneInputModule standaloneInputModule = EventSystem.current.GetComponent<StandaloneInputModule>();
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                PS4ControllerIsConnected = true;
                standaloneInputModule.submitButton = PS4ValidationButtonName;
                return;
            }
            else if(names[x].Length == 33)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                XboxControllerIsConnected = true;
                standaloneInputModule.submitButton = XBOXValidationButtonName;
                return;
            }
            else
            {
                Debug.LogError("NO CONTROLLER CONNECTED");
                standaloneInputModule.submitButton = XBOXValidationButtonName;
            }
        }
    }
}
