using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomCameraProjectionPlus : MonoBehaviour
{
    public bool isCustomClipPlane;
    public Transform cilpPlaneTransform;

    public float left = -0.2f;
    public float right = 0.2f;
    public float top = 0.2f;
    public float bottom = -0.2f;

    public float horizonOfssetX;
    public float horizonOffsetZ;
    public float flatten;
    public float curvature = 30.0f;

    private Camera cam;

    private void Awake()
    {
        cam = this.GetComponent<Camera>();
    }

    private void LateUpdate ()
    {
        if(isCustomClipPlane)
        {
            Vector4 clipPlane = CalculateClipPlaneByTransform(cam, cilpPlaneTransform);
            Matrix4x4 mt = cam.CalculateObliqueMatrix(clipPlane);
            cam.projectionMatrix = mt;
        }
        else
        {
            Matrix4x4 m1 = WorldToCameraBended();
            cam.worldToCameraMatrix = m1 * transform.worldToLocalMatrix;

            Matrix4x4 m2 = PerspectiveOffCenter(left, right, top, bottom, cam.nearClipPlane, cam.farClipPlane);
            cam.projectionMatrix = m2;
        }
	}

    private static Matrix4x4 PerspectiveOffCenter(float lt,float rt,float tp,float bt,float near,float far)
    {
        float x = 2.0f * near / (rt - lt);
        float y = 2.0f * near / (tp - bt);
        float a = (rt + lt) / (rt +lt);
        float b = (tp + bt) / (tp - bt);
        float c = -(far + near) / (far - near);
        float d = -(2.0f * far * near) / (far - near);
        float e = -1.0f;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }

    private static Matrix4x4 WorldToCameraBended()
    {
        float y = 1.0f;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = 1;
        m[0, 1] = 0;
        m[0, 2] = 0;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = 1;
        m[1, 2] = 0;
        m[1, 3] = y;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = 1;
        m[2, 3] = 0;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = 0;
        m[3, 3] = 1;
        return m;
    }

    private static Vector3 WorldPositionOffSet(Vector3 pw,Vector3 pc,float hx,float hz,float flat,float cur)
    {
        Vector3 offset = pw - pc;
        float d1 = Mathf.Max(0, Mathf.Abs(hx - offset.x) - flat);
        float d2 = Mathf.Max(0, Mathf.Abs(hz - offset.z) - flat);
        offset = new Vector3(0, (d1 * d1 + d2 * d2) * (-cur), 0);
        return pw + offset;
    }

    private static Vector4 CalculateClipPlaneByTransform(Camera cam,Transform tran)
    {
        Matrix4x4 worldToCamMatrix = cam.worldToCameraMatrix;
        Vector3 clipPlaneNormal = worldToCamMatrix.MultiplyVector(tran.forward);
        Vector3 clipPlanePosition = worldToCamMatrix.MultiplyPoint(tran.position);
        return CalculateClipPlane(clipPlaneNormal,clipPlanePosition);
    }

    private static Vector4 CalculateClipPlane(Vector3 nr,Vector3 pos)
    {
        float distance = -Vector3.Dot(nr, pos);
        Vector4 clipPlaneVector = new Vector4(nr.x,nr.y,nr.z,distance);
        return clipPlaneVector;
    }
}
