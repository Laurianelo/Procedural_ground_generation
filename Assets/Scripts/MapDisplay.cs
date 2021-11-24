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
    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);//x
        int height = noiseMap.GetLength(1);//y

        Texture2D texture = new Texture2D(width, height);

        //get colors of noisemap 
        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // get line and column of each pixel
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        //apply color of all pixels
        texture.SetPixels(colorMap);
        texture.Apply();

        //strat and not to runtime
        textureRenderer.sharedMaterial.mainTexture = texture;

        //change scale of plane 
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}
