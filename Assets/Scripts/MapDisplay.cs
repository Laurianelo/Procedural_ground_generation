using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;


    /// <summary>
    /// Draw Map
    /// </summary>
    /// <param name="noiseMap">Heigth and width</param>
    public void DrawTexture(Texture2D texture)
    {
        //strat and not to runtime
        textureRenderer.sharedMaterial.mainTexture = texture;

        //change scale of plane 
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
}
