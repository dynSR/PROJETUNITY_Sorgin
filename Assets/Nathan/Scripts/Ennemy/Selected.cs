using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    public bool IsSelected;
    public int IndexPlace;
    public GameObject SelectedArrow;

    void Update()
    {
        SelectedArrow.SetActive(IsSelected);
    }

}
