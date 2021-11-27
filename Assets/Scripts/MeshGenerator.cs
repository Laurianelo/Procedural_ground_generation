using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{

    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCruve, int LOD)
    {
        //get length and height
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        //for center the mesh
        float topLeftX = (width - 1) / -2f; 
        float topLeftZ = (height - 1) / 2f;//height


        int meshSimplificationIncrement = (LOD== 0)?1:LOD *2 ;
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;// nb de vertices for each line

        // create vertices
        MeshData mshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;
        for (int y = 0; y < height; y+= meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x+= meshSimplificationIncrement)
            {
                mshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCruve.Evaluate(heightMap[x, y]) *  heightMultiplier, topLeftZ - y);
                mshData.uvsMap[vertexIndex] = new Vector2(x / (float)width, y / (float)height); // send % for uv knows in which vertex are we
                //create triangles, ignore all vertex at the right and at the bitton of the map
                if (x < width - 1 && y < height - 1)
                {
                    mshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    mshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }
        return mshData;
    }
}

/// <summary>
/// informations of mesh
/// </summary>
public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    int triangleIndex;
    public Vector2[] uvsMap;// texture on mesh

    //lenght of vertices and triangles
    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];// how many vertices in map
        uvsMap = new Vector2[meshWidth * meshHeight]; //one uv per vertex 
        triangles = new int[((meshWidth - 1) * (meshHeight - 1)) * 6];
    }

    // add triangle (3 vertices -> a,b,c) to array of triangles
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;// next triangle 
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
