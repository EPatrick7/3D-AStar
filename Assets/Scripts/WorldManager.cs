using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    public int WorldSize=5;
    public Tilemap[] Slices;
    public Tile[] Tiles;

    [HideInInspector]
    public World world;

    private void Start()
    {
        GenerateWorld();
        RenderWorld();
    }
    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3Int mPos= Vector3Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition)- new Vector3(0.15f,0.15f,0f));
            mPos.z = 0;
            if(world.isPos(mPos))
            {
                world.getTile(mPos).tileId=0;
                world.getTile(mPos).pathCost=-1;
                RenderTile(mPos);
            }
        }
    }
    public void GenerateWorld()
    {
        world = new World(Slices.Length, WorldSize);
    }
    public void RenderTile(Vector3Int targ)
    {
        Slices[targ.z].SetTile(new Vector3Int(targ.x, targ.y, 0), Tiles[world.getTile(targ).tileId]);
    }
    public void RenderWorld()
    {
        for(int z=0;z<Slices.Length;z++)
        {
            for(int x=0;x<WorldSize;x++)
            {
                for (int y = 0; y < WorldSize; y++)
                {
                    RenderTile(new Vector3Int(x, y, z));
                }
            }
        }
    }
    public void DebugSetColor(Vector3Int targ,Color col)
    {
        if(world.isPos(targ))
        {
            Slices[targ.z].SetTile(new Vector3Int(targ.x, targ.y, 0), null);
            Tile t = Tiles[world.getTile(targ).tileId];
            if (t == null)
                return;
            Color old = t.color;
            Tiles[world.getTile(targ).tileId].color = col;
            Slices[targ.z].SetTile(new Vector3Int(targ.x, targ.y, 0), t);
            t.color = old;
        }
    }
}
