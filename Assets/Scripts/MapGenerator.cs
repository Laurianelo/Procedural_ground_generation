using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh}
    public DrawMode drawMode;
    public bool autoUpdate;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octave;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;// seed for random generation
    public Vector2 offset;

    public TerrainType[] regions;

    //generate dot map with perlin noise
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GeneateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octave, persistance, lacunarity, offset);

        Color[] colormap = new Color[mapWidth * mapHeight];
        //assign color
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colormap[y * mapWidth + x] = regions[i].colorTerrain;
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
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colormap, mapWidth, mapHeight));
        }
        else if(drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap), TextureGenerator.TextureFromColorMap(colormap, mapWidth, mapHeight));
        }
       
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

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colorTerrain;

}
