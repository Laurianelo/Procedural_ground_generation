using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public bool autoUpdate;
    public int octave;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public int seed;// seed for random generation
    public Vector2 offset;


    //generate dot map with perlin noise
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GeneateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octave, persistance, lacunarity, offset);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }


    //forbiden some values
    private void OnValidate()
    {
        if(mapWidth <1)
        {
            mapWidth = 1;
        }

        if(mapHeight < 1)
        {
            mapHeight = 1;
        }

        if(lacunarity < 1)
        {
            lacunarity = 1;
        }

        if(octave < 0)
        {
            octave = 0;
        }


    }

}
