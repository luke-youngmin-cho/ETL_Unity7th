using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerHullBehaviour : MonoBehaviour
{
    private float _speed = 1.0f;
    private float _decel = 1.0f;
    private void FixedUpdate()
    {
        transform.position -= transform.up * _speed * Time.fixedDeltaTime;
        _speed -= _decel * Time.fixedDeltaTime;

        if (_speed <= 0f)
            Destroy(this.gameObject);
    }
}
