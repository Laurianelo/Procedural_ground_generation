using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    //create a button in edtor to generate map faster for testing
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        //re-generate map if the width or height are updated
        if(DrawDefaultInspector())
        {
            if(mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }

        if(GUILayout.Button("Generate Map"))
        {
            mapGen.GenerateMap();
        }
    }
}
