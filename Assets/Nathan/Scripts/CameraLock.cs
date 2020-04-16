using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    public int x;
    public int y;
    public int z;

    public bool FollowCamera;

    public Transform Cam;

    // Update is called once per frame
    void Update()
    {
        if(!FollowCamera)
            transform.rotation = Quaternion.Euler(new Vector3(x, y, z));

        if (FollowCamera)
            transform.rotation = Cam.transform.rotation;
    }
}
