using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    void FixedUpdate()
    {
        this.transform.Rotate(0f, 0.0f, 90.0f, Space.Self);
    }
}
