using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonScale : MonoBehaviour
{
    [Range(0, 50)]
    public int segments = 50;
    public float range;
    public SphereCollider SphereCol;

    LineRenderer line;

    Color Fade;

    void Start()
    {

        SphereCol.radius = range;
        line = gameObject.GetComponent<LineRenderer>();
        Fade = Color.white;

        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        CreatePoints();
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1,1,1), 1f * Time.deltaTime);

        Fade.a -= Time.deltaTime;
        line.endColor = Fade;
        line.startColor = Fade;

        if (Fade.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * range;

            line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ennemy"))
        {
            other.transform.GetComponentInChildren<EnnemyView>().Detection += range / 40;
        }
    }
}
