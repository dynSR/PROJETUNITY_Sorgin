//using Fungus;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;
//using UnityEngine.UIElements;

//public class FieldOfView : MonoBehaviour
//{
//    public float viewRadius;
//    [Range(0, 360)]
//    public float viewAngle;

//    public LayerMask targetMask;
//    public LayerMask obstacleMask;

//    //[HideInInspector]
//    public List<Transform> visibleTargets = new List<Transform>();

//    public float meshResolution;
//    public int edgeResolveResolutions;
//    public float edgeDistanceThreshold;

//    public MeshFilter viewMeshFilter;
//    private Mesh viewMesh;


//    void Start()
//    {
//        viewMesh = new Mesh();
//        viewMesh.name = "view Mesh";
//        viewMeshFilter.mesh = viewMesh;
//        StartCoroutine(FindTargetWithDelay(.2f));
//    }

//    private void LateUpdate()
//    {
//        DrawFieldOfView();
//    }

//    IEnumerator FindTargetWithDelay(float delay)
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(delay);
//            FindVisibleTargets();
//        }
//    }

//    void HasATargetInSight(Transform target)
//    {
//        if (gameObject.CompareTag("Player"))
//        {
//            Player.s_Singleton.hasATarget = true;

//            Renderer targetRenderer = target.GetComponent<Renderer>();
//            targetRenderer.material.color = Color.red;
//            target.GetChild(0).gameObject.SetActive(true);

//            Debug.Log("Target Found");
//        }

//        if (visibleTargets.Count >= 2)
//        {
//            for (int i = 0; i < visibleTargets.Count; i++)
//            {
//                Renderer targetRenderer = visibleTargets[i].GetComponent<Renderer>();
//                targetRenderer.material.color = Color.white;
//                visibleTargets[i].GetChild(0).gameObject.SetActive(false);
//            }
//        }
//    }

//    void HasNoTargetInSight(Transform target)
//    {
//        if (gameObject.CompareTag("Player"))
//        {
//            Player.s_Singleton.hasATarget = false;
            
//            Renderer targetRenderer = target.GetComponent<Renderer>();
//            targetRenderer.material.color = Color.white;
//            target.GetChild(0).gameObject.SetActive(false);
//            Debug.Log("No Target Found");
//        }
//    }

//    void FindVisibleTargets()
//    {
//        visibleTargets.Clear();

//        Player.s_Singleton.playerTargets.Clear();
//        Player.s_Singleton.actualPlayerTarget = null;

//        Collider[] targetsInviewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

//        for (int i = 0; i < targetsInviewRadius.Length; i++)
//        {
//            Transform target = targetsInviewRadius[i].transform;

//            HasNoTargetInSight(target);

//            Vector3 dirToTarget = (target.position - transform.position).normalized;

//            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
//            {
//                float distToTarget = Vector3.Distance(transform.position, target.position);

//                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
//                {
//                    //visibleTargets.Clear();
//                    visibleTargets.Add(target);
//                    Player.s_Singleton.playerTargets.Add(target);
//                    Player.s_Singleton.actualPlayerTarget = target;
//                    HasATargetInSight(target);
//                }
//            }
//        }
//    }

//    void DrawFieldOfView()
//    {
//        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
//        float stepAngleSize = viewAngle / stepCount;
//        List<Vector3> viewPoints = new List<Vector3>();
//        ViewCastInfo oldViewCast = new ViewCastInfo();

//        for (int i = 0; i <= stepCount; i++)
//        {
//            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
//            ViewCastInfo newViewCast = ViewCast(angle);

//            if(i > 0)
//            {
//                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;

//                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded))
//                {
//                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
//                    if (edge.pointA != Vector3.zero)
//                    {
//                        viewPoints.Add(edge.pointA);
//                    }
//                    if (edge.pointB != Vector3.zero)
//                    {
//                        viewPoints.Add(edge.pointB);
//                    }
//                }
//            }

//            viewPoints.Add(newViewCast.point);
//            oldViewCast = newViewCast;
//        }

//        int vertexCount = viewPoints.Count + 1;
//        Vector3[] vertices = new Vector3[vertexCount];
//        int[] triangles = new int[(vertexCount - 2) * 3];

//        vertices[0] = Vector3.zero;
//        for (int i = 0; i < vertexCount - 1; i++)
//        {
//            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

//            if (i < vertexCount - 2)
//            {
//                triangles[i * 3] = 0;
//                triangles[i * 3 + 1] = i + 1;
//                triangles[i * 3 + 2] = i + 2;
//            }
//        }

//        viewMesh.Clear();
//        viewMesh.vertices = vertices;
//        viewMesh.triangles = triangles;
//        viewMesh.RecalculateNormals();
//    }

//    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
//    {
//        float minAngle = minViewCast.angle;
//        float maxAngle = maxViewCast.angle;
//        Vector3 minPoint = Vector3.zero;
//        Vector3 maxPoint = Vector3.zero;

//        for (int i = 0; i < edgeResolveResolutions; i++)
//        {
//            float angle = (minAngle + maxAngle) / 2;
//            ViewCastInfo newViewCast = ViewCast(angle);

//            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;

//            if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
//            {
//                minAngle = angle;
//                minPoint = newViewCast.point;
//            }
//            else
//            {
//                maxAngle = angle;
//                maxPoint = newViewCast.point;
//            }
//        }

//        return new EdgeInfo(minPoint, maxPoint);
//    }

//    ViewCastInfo ViewCast(float globalAngle)
//    {
//        Vector3 dir = DirFromAngle(globalAngle, true);
//        RaycastHit hit;

//        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
//        {
//            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
//        }
//        else
//        {
//            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
//        }
//    }

//    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
//    {
//        if (!angleIsGlobal)
//        {
//            angleInDegrees += transform.eulerAngles.y;
//        }
//        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
//    }

//    public struct ViewCastInfo
//    {
//        public bool hit;
//        public Vector3 point;
//        public float distance;
//        public float angle;

//        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
//        {
//            hit = _hit;
//            point = _point;
//            distance = _distance;
//            angle = _angle;
//        }
//    }

//    public struct EdgeInfo
//    {
//        public Vector3 pointA;
//        public Vector3 pointB;

//        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
//        {
//            pointA = _pointA;
//            pointB = _pointB;
//        }
//    }

//}
