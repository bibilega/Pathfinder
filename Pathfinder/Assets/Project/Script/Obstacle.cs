using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector3 Position => Transform.position;

    private Transform _transform;

    public Transform Transform
    {
        get
        {
            if (_transform == null)
                _transform = transform;

            return _transform;
        }
    }
}
