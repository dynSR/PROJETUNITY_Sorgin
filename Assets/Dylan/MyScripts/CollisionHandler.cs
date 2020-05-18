using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public Collider[] hitColliders;
    public bool isColliding = false;

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;
    public float lerp;


    private void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    private void Update()
    {
        CollisionCheck();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    void CollisionCheck()
    {
        Vector3 desiredObjPosition = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, /*Player.s_Singleton.posToInstantiateTheClone.position*/desiredObjPosition, out hit))
        {
            //transform.position = Vector3.Lerp(transform.parent.GetComponent<Collider>().ClosestPointOnBounds(transform.parent.localPosition), hit.collider.ClosestPointOnBounds(hit.collider.transform.localPosition), Time.deltaTime* smooth);
            distance = Mathf.Clamp((hit.distance * lerp), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
            //transform.position = Player.s_Singleton.posToInstantiateTheClone.position;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);

        int i = 0;

        if (i < hitColliders.Length)
        {
            i++;
            isColliding = true;
        }
        else
        {
            isColliding = false;
        }
    }
}
