using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerTrial : DefaultUIManager
{
    // Start is called before the first frame update
    void Start()
    {
        SetOSTStateAndPostEvent("state_Trial");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
