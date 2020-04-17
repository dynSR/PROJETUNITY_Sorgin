using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public static class RendererExtensions
    {
        public static bool IsTargetVisible(Camera c, GameObject go)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(c);
            var point = go.transform.position;
            foreach (var plane in planes)
            {
                if (plane.GetDistanceToPoint(point) < 0)
                    return false;
            }
            return true;
        }
    }

