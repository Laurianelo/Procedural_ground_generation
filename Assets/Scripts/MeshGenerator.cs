using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{

    //generate mesh of map
    public static MeshData GenerateTerrainMesh(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData mshData = new MeshData(width, height);
        int vertexIndex = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                mshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x, y], topLeftZ - y);
                mshData.uvsMap[vertexIndex] = new Vector2(x / (float)width, y / (float)height); // send % for uv knows in which vertex are we
                //create triangles
                if (x < width - 1 && y < height - 1)
                {
                    mshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    mshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }


        }

        return mshData;

    }
}


public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    int triangleIndex;
    public Vector2[] uvsMap;

    //get lenght of vertices and triangles
    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvsMap = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    // add vertices to triangle
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }


    //create mesh
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvsMap;
        mesh.RecalculateNormals();
        return mesh;
    }

}
