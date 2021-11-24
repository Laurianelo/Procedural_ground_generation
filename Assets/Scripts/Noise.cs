using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{
    public static float[,] GeneateNoiseMap(int mapWidth, int mapHeight, float scaleNoise)
    {
        float[,] noiseMap = new float[mapWidth, mapWidth];

        // we check scaleNoise if not equal to zero 
        //if it is we assign a value
        if(scaleNoise <= 0)
        {
            scaleNoise = 0.0001f;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float sampleX = x / scaleNoise;
                float sampleY = y / scaleNoise;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);

                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }
}
