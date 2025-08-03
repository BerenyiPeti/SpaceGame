using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PixelateEffect : MonoBehaviour
{
    public Material pixelateMaterial;
    [Range(1, 512)]
    public int pixelSize = 64;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (pixelateMaterial != null)
        {
            pixelateMaterial.SetFloat("_PixelSize", pixelSize);
            Graphics.Blit(src, dest, pixelateMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
