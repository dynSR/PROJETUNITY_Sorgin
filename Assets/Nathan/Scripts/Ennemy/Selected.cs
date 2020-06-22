using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    public bool IsSelected;
    public int IndexPlace;
    public GameObject SelectedArrow;
    EnnemyView EnnemiScript;

    private void Start()
    {
        EnnemiScript = transform.GetComponent<EnnemyView>();
    }
    void Update()
    {
        if (EnnemiScript.Stunned == true)
        {
            IsSelected = false;
        }
        SelectedArrow.SetActive(IsSelected);
    }

}
