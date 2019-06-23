using UnityEngine;

public class ChangeRenderSource : MonoBehaviour
{
    public RenderTexture source;

	void OnRenderImage (RenderTexture src,RenderTexture dst)
    {
        Graphics.Blit(source,dst);
	}
}
