using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapWidth">map' width</param>
    /// <param name="mapHeight">map's height</param>
    /// <param name="scaleNoise">Noisemap's scale</param>
    /// <param name="octave">nb of noisemap superposée</param>
    /// <param name="persistance">controle la diminution de l'amplitude des octaves</param>
    /// <param name="lacunarity">nb de frequence pour chaque octave</param>
    /// <returns></returns>
    public static float[,] GeneateNoiseMap(int mapWidth, int mapHeight, int seed,  float scaleNoise, int octave, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapWidth];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // to begin generation in middle of map
        float halfwidth = mapWidth/2;
        float halfHeight = mapHeight / 2;

        //pseudo random next generator
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octave];

        //random generation of offsets
        for (int i = 0; i < octave; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

        }

        // we check scaleNoise if not equal to zero 
        //if it is we assign a value
        if (scaleNoise <= 0)
        {
            scaleNoise = 0.0001f;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octave; i++) //generate noise map for each octaves
                {
                    float sampleX = (x- halfwidth) / scaleNoise * frequency + octaveOffsets[i].x;
                    float sampleY = (y- halfHeight) / scaleNoise * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance; // change amplitude value for next value

                    frequency *= lacunarity;// augmente la frequence d'octave en octave 
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        //normalize values
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
