using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator { 

    //genere map with colors
    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

    //generete map with height(perlin noise)
    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);//x
        int height = heightMap.GetLength(1);//y

        //get colors of noisemap 
        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // get line and column of each pixel
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        //apply color of all pixels
        return TextureFromColorMap(colorMap, width, height);
    }

}
