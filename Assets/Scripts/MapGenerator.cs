using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh}
    public DrawMode drawMode;
    public bool autoUpdate;

    public const int mapChunkSize = 241; //255 is limited by unity 
    [Range(0,6)]
    public int LOD;// 1 : max of detail; 2: less details, ...
    public float noiseScale;
    public int octave;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public int seed;// seed for random generation
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCruve;
    public Vector2 offset;

    public TerrainType[] regions;

    //generate dot map with perlin noise
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GeneateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octave, persistance, lacunarity, offset);

        Color[] colormap = new Color[mapChunkSize * mapChunkSize];
        //assign color
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colormap[y * mapChunkSize + x] = regions[i].colorTerrain;
                        break;
                    }
                }
            }
        }


        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if(drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colormap, mapChunkSize, mapChunkSize));
        }
        else if(drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCruve, LOD), TextureGenerator.TextureFromColorMap(colormap, mapChunkSize, mapChunkSize));
        }
       
    }


    //forbiden some values
    private void OnValidate()
    {
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

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colorTerrain;

}
