using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(WorldManager))]
public class Pathfinder : MonoBehaviour
{
    private WorldManager worldManager;
    void Start()
    {
        worldManager=GetComponent<WorldManager>();
    }
    public GameObject StartLoc;
    public GameObject StopLoc;
    public int WaitForOffset = 5000;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            worldManager.RenderWorld();
            StartCoroutine(CalcPath(Vector3Int.RoundToInt(StartLoc.transform.position), Vector3Int.RoundToInt(StopLoc.transform.position)));
        }
    }

    public List<Vector3Int> path;
    public IEnumerator CalcPath(Vector3Int from, Vector3Int to)
    {
       // Debug.Log("Working..");
        path =new List<Vector3Int>();
        
        if(from==to)
        {
            yield break; // No need to pathfind, we are already there!
        }


        if(!worldManager.world.isPos(to)|| !worldManager.world.isPos(from))
        {
            Debug.LogError("Cannot pathfind outside of the given world scope!");
            yield break;
        }

        List<World.Tile> live= new List<World.Tile> ();
        List<World.Tile> cleared = new List<World.Tile>();

        World.Tile start=worldManager.world.getTile(from);
        World.Tile tile =start;
        World.Tile target = worldManager.world.getTile(to);
        int waitForOffset = 1;

        live.Add(tile);

        while (tile.location != to)
        {

            World.Tile cheapest=live[0];
            foreach(World.Tile option in live)
            {
                if(cheapest.HCost()>option.HCost())
                {
                    cheapest = option;
                }


                if (waitForOffset % WaitForOffset == 0)
                    yield return new WaitForEndOfFrame();
                waitForOffset++;
            }
            tile = cheapest;


            FetchNeighbors(tile, target, live,cleared);
            live.Remove(tile);
            cleared.Add(tile);
            worldManager.DebugSetColor(tile.location, Color.red);

            if(waitForOffset% WaitForOffset == 0)
                yield return new WaitForEndOfFrame();
            waitForOffset++;
         //   yield return new WaitForSeconds(0.1f);

            if (live.Count==0)
            {
                Debug.LogError("No path found!");
                yield break;
            }
        }
     //   Debug.Log("Backtracking...");
        tile=target;
        while(tile!=start)
        {
            worldManager.DebugSetColor(tile.location, Color.blue);
            path.Add(tile.location);
            tile = worldManager.world.getTile(tile.pathPointer);
            if (waitForOffset % WaitForOffset == 0)
                yield return new WaitForEndOfFrame();
            waitForOffset++;
            //   yield return new WaitForSeconds(0.1f);
        }

        worldManager.DebugSetColor(tile.location, Color.blue);
        path.Reverse();
     //   Debug.Log("Made it!");

    }
    public void FetchNeighbors(World.Tile tile, World.Tile target, List<World.Tile> live,List<World.Tile> cleared)
    {
        Vector3Int loc = tile.location;


        loc = tile.location + new Vector3Int(1, 0, 0);
        if (worldManager.world.isPos(loc))
        {
            AddNeighbor(tile,worldManager.world.getTile(loc),target,live,cleared);
        }
        loc = tile.location + new Vector3Int(-1, 0, 0);
        if (worldManager.world.isPos(loc))
        {
            AddNeighbor(tile, worldManager.world.getTile(loc), target, live, cleared);
        }
        loc = tile.location + new Vector3Int(0, 1, 0);
        if (worldManager.world.isPos(loc))
        {
            AddNeighbor(tile, worldManager.world.getTile(loc), target, live,cleared);
        }
        loc = tile.location + new Vector3Int(0, -1, 0);
        if (worldManager.world.isPos(loc))
        {
            AddNeighbor(tile, worldManager.world.getTile(loc), target, live, cleared);
        }
        loc = tile.location + new Vector3Int(1, 1, 0);
        if (worldManager.world.isPos(loc))
        {
            AddNeighbor(tile, worldManager.world.getTile(loc), target, live,cleared);
        }
        loc = tile.location + new Vector3Int(1, -1, 0);
        if (worldManager.world.isPos(loc))
        {
            AddNeighbor(tile, worldManager.world.getTile(loc), target, live, cleared);
        }
        loc = tile.location + new Vector3Int(-1, 1, 0);
        if (worldManager.world.isPos(loc))
        {
            AddNeighbor(tile, worldManager.world.getTile(loc), target, live,cleared);
        }
        loc = tile.location + new Vector3Int(-1, -1, 0);
        if (worldManager.world.isPos(loc))
        {
            AddNeighbor(tile, worldManager.world.getTile(loc), target, live, cleared);
        }
        if(tile.CanGoUp())
        {
            loc = tile.location + new Vector3Int(0, 0, 1);
            if (worldManager.world.isPos(loc))
            {
                AddNeighbor(tile, worldManager.world.getTile(loc), target, live, cleared);
            }
        }

        if (tile.CanGoDown())
        {
            loc = tile.location + new Vector3Int(0, 0, -1);
            if (worldManager.world.isPos(loc))
            {
                AddNeighbor(tile, worldManager.world.getTile(loc), target, live, cleared);
            }
        }

    }
    public void AddNeighbor(World.Tile tile, World.Tile tile_new,World.Tile target, List<World.Tile> live,List<World.Tile> cleared)
    {
        if(tile_new.pathCost<0)
        {
            return; //This tile is a wall.
        }
        bool alreadyHas = live.Contains(tile_new) || cleared.Contains(tile_new);
        if (!alreadyHas)
        {
            live.Add(tile_new);
            worldManager.DebugSetColor(tile_new.location, Color.yellow);
        }

        tile_new.UpdateCosts(tile,target,!alreadyHas);
    }
}
