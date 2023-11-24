using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    public int WorldSize=5;
    public Tilemap[] Slices;
    public Tile[] Tiles;

    private World world;

    private void Start()
    {
        GenerateWorld();
        RenderWorld();
    }
    public void GenerateWorld()
    {
        world = new World(Slices.Length, WorldSize);
    }

    public void RenderWorld()
    {
        for(int y=0;y<Slices.Length;y++)
        {
            for(int x=0;x<WorldSize;x++)
            {
                for (int z = 0; z < WorldSize; z++)
                {
                    Slices[y].SetTile(new Vector3Int(x,z,0),Tiles[world.getTile(x,y,z).tileId]);
                }
            }
        }
    }
}
