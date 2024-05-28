using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : MonoBehaviour
{
    public Transform plane;
    public Material slicedSurface;
    private LayerMask _noteMask;

    private void Awake()
    {
        _noteMask = LayerMask.GetMask("Note");
    }


    private void FixedUpdate()
    {
        var cols = Physics.OverlapSphere(transform.position, 3f, _noteMask);
        if (cols.Length > 0)
        {
            Slice(cols[0].gameObject);
        }
    }

    private void Slice(GameObject target)
    {
        SlicedHull hull = target.Slice(plane.position, plane.up);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, slicedSurface);
            upperHull.AddComponent<UpperHullBehaviour>();
            GameObject lowerHull = hull.CreateLowerHull(target, slicedSurface);
            lowerHull.AddComponent<LowerHullBehaviour>();
            Destroy(target);
        }
    }
}
