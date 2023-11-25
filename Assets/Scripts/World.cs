using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

[System.Serializable]
public class World
{
    public int WorldSize;
    public int WorldHeight;
    public Tile[][][] Grid;
    public World(int height,int size)
    {
        WorldSize = size;
        WorldHeight = height;

        Grid = new Tile[WorldHeight][][];
        for (int z = 0; z< Grid.Length; z++)
        {
            Grid[z]= new Tile[WorldSize][];
            for (int x = 0; x < Grid[z].Length; x++)
            {
                Grid[z][x]= new Tile[WorldSize];


                for (int y = 0; y < Grid[z][x].Length; y++)
                {
                    Grid[z][x][y] = new Tile(new Vector3Int(x,y,z),1);
                }
            }
        }
    }
    public Tile getTile(Vector3Int pos)
    {
        if(!isPos(pos))
        {
            Debug.LogError("Position "+pos+" is out of bounds!");
        }
        return Grid[pos.z][pos.x][pos.y]; //z is the vertical slice, x and y are the grid coordinates.
    }
    public bool isPos(Vector3Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.z >= 0 && pos.x < WorldSize && pos.y < WorldSize && pos.z < WorldHeight;
    }
    [System.Serializable]
    public class Tile
    {
        public Tile(Vector3Int location, int id)
        {
            tileId = id;
            pathCost = 1;
            this.location = location;
        }
        public Tile(Vector3Int location) //Empty Tile = 0
        {
            tileId = 0;
            pathCost = -1;
            this.location = location;
        }
        public Vector3Int location;
        public int tileId=0;
        public int pathCost=1;

        
        //Pathfinding Data:
        public Vector2Int pathTemp;
        public Vector3Int pathPointer;


        public void UpdateCosts(Tile neighbor,Tile target,bool forced)
        {
            int g_distance = pathCost+ neighbor.GCost()+ Mathf.RoundToInt(Vector3.Distance(location, neighbor.location)*10);
            int h_distance = Mathf.RoundToInt(Vector3.Distance(location, target.location) * 10)+(1000*Mathf.Abs(location.z-target.location.z));
            if (forced||(HCost()>(g_distance+h_distance)))
            {
                pathPointer = neighbor.location;
                SetGCost(g_distance);
                SetHCost(h_distance);
            }
        }
        public bool CanGoUp()
        {
            if (tileId == 2)
                return true;

            return false;
        }
        public bool CanGoDown()
        {
            if (tileId == 3)
                return true;

            return false;
        }
        private void SetGCost(int cost)
        {
            pathTemp.x = cost;
        }
        private void SetHCost(int cost)
        {
            pathTemp.y = cost;
        }
        public int GCost()
        {
            return pathTemp.x;
        }
        public int FCost()
        {
            return pathTemp.y;
        }
        public int HCost()
        {
            return pathTemp.x+ pathTemp.y;
        }
    }
}
