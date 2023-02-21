using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ContainerComponent Class.
/// </summary>
public class ContainerComponent : MonoBehaviour
{
    // Reference to the speed variable for access in the inspector.
    public float speed = 5f;

    // References to the X, Y, and Z variables respectively.
    // Serialized fields to make them accessible in the inspector.
    [SerializeField]
    private float X, Y, Z = 0;

    // Update is called once per frame
    void Update()
    {
        // Rotate the gameObject this script is attached to.
        // Rotate the gameObject based on X,Y,Z variables times deltaTime times the speed.
        this.transform.Rotate(new Vector3(X, Y, Z) * Time.deltaTime * speed);
    }
}