using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ObliqueMatrix : MonoBehaviour
{
    public float zScale;
    public Vector3 offsetViewPoint;
    private Camera cam;

    void Awake ()
    {
        cam = this.GetComponent<Camera>();
	}

    private void OnEnable()
    {
        Apply();
    }

    private void OnDisable()
    {
        cam.ResetProjectionMatrix();
    }

    /// <summary>
    /// Camera Oblique Projection
    /// </summary>
    void Apply ()
    {
        var offX = offsetViewPoint.x;
        var offY = offsetViewPoint.y;
        //var offZ = offsetViewPoint.z;

        //cam.fieldOfView = Mathf.Atan(0.5f / offZ) * Mathf.Rad2Deg * zScale;
        var matrix = cam.projectionMatrix;
        matrix.m02 = offX * zScale;
        matrix.m12 = offY * zScale;
        cam.projectionMatrix = matrix;
	}

    private void Update()
    {
        Apply();
    }
}
