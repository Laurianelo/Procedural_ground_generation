using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGround : MonoBehaviour
{
    public const float maxViewDist = 500;// player's max view
    public Transform viewer;

    public static Vector2 viewerPosition;
    int chunkSize; 
    int chunkVisibleViewDst;// chunk in view distance zone;

    Dictionary<Vector2, GroundChunk> GroundChunkDictionary = new Dictionary<Vector2, GroundChunk>();
    List<GroundChunk> GroundChunksVisibleLastUpdate = new List<GroundChunk>();
    private void Start()
    {
        chunkSize = MapGenerator.mapChunkSize -1;
        chunkVisibleViewDst = Mathf.RoundToInt(maxViewDist / chunkSize);
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }




    void UpdateVisibleChunks()
    {

        //disable all chunk
        for (int i = 0; i < GroundChunksVisibleLastUpdate.Count; i++)
        {
            GroundChunksVisibleLastUpdate[i].SetVisible(false);
        }
        GroundChunksVisibleLastUpdate.Clear();


        //chunk where the viewer is 
        int currentChunkCordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        //get chunk around player
        for (int yOffset = -chunkVisibleViewDst; yOffset <= chunkVisibleViewDst; yOffset++)
        {
            for (int xOffset = -chunkVisibleViewDst; xOffset <= chunkVisibleViewDst; xOffset++)
            {
                Vector2 viewerChunkCoord = new Vector2(currentChunkCordX + xOffset, currentChunkCordY + yOffset);
                
                //vérifie si le joueur a accé a certains chunck, ont ils déja ete genere ? 
                if(GroundChunkDictionary.ContainsKey(viewerChunkCoord))
                {
                    GroundChunkDictionary[viewerChunkCoord].UpdateGroundChunk();
                    if(GroundChunkDictionary[viewerChunkCoord].IsVisible())
                    {
                        GroundChunksVisibleLastUpdate.Add(GroundChunkDictionary[viewerChunkCoord]);
                    }
                }
                else
                {
                    GroundChunkDictionary.Add(viewerChunkCoord, new GroundChunk(viewerChunkCoord, chunkSize, transform));
                }
            }

        }

    }



    public class GroundChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        public GroundChunk(Vector2 coord, int size, Transform parent)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionv3 = new Vector3(position.x, 0, position.y);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionv3;
            meshObject.transform.localScale = Vector3.one * size / 10f;
            meshObject.transform.parent = parent;
            SetVisible(false);
        }


        /// <summary>
        /// Activate or desactivate chunk if player's distance is ok
        /// </summary>
        public void UpdateGroundChunk()
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDstFromNearestEdge <= maxViewDist;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        //return state of object
        public bool  IsVisible()
        {
            return meshObject.activeSelf;
        }
    }
}



