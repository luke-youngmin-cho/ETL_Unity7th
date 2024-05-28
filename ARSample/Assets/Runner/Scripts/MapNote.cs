using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNote : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.position += Vector3.back * 3f * Time.fixedDeltaTime;
    }
}
