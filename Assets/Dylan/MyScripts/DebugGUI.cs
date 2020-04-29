using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGUI : MonoBehaviour
{
    [SerializeField] private Vector2 rectPosition;
    [SerializeField] private float rectWidth = 50f;
    [SerializeField] private float rectHeigth = 50f;
    [Multiline]
    [SerializeField] private string textToDisplay;

    private void OnGUI()
    {
        GUI.Box(new Rect(rectPosition.x, rectPosition.y, rectWidth, rectHeigth), textToDisplay);
    }
}
