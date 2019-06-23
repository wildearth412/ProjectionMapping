//http://docs.unity3d.com/412/Documentation/ScriptReference/Camera.OnPreCull.html

using UnityEngine;
using System.Collections;

namespace Spout
{
	[RequireComponent (typeof(Camera))]
	[ExecuteInEditMode]
	public class InvertCamera : MonoBehaviour
    {
        public bool invertVorH;

		void OnPreCull ()
        {
			GetComponent<Camera>().ResetWorldToCameraMatrix();
			//GetComponent<Camera>().ResetProjectionMatrix();

            //Quaternion rotation = Quaternion.Euler(0,90.0f,0);
            //GetComponent<Camera>().projectionMatrix = GetComponent<Camera>().projectionMatrix * Matrix4x4.Rotate(rotation);
            float f1 = invertVorH ? -1f : 1f;
            float f2 = invertVorH ? 1f : -1f;
            GetComponent<Camera>().projectionMatrix = GetComponent<Camera>().projectionMatrix * Matrix4x4.Scale(new Vector3(f2, f1, 1));
        }

        void OnPreRender()
        {
            //GL.SetRevertBackfacing(true);
            GL.invertCulling = true;
        }

        void OnPostRender()
        {
            //GL.SetRevertBackfacing(false);
            GL.invertCulling = false;
        }
    }
}