using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchMap : MonoBehaviour
{
    public MapHandler MapScript;

    public void OpenMap()
    {
        MapScript.DisplayMap();
    }
}
