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
        for (int y = 0; y< Grid.Length; y++)
        {
            Grid[y]= new Tile[WorldSize][];
            for (int x = 0; x < Grid[y].Length; x++)
            {
                Grid[y][x]= new Tile[WorldSize];


                for (int z = 0; z < Grid[y][x].Length; z++)
                {
                    Grid[y][x][z] = new Tile();
                }
            }
        }
    }
    public Tile getTile(int x,int y, int z)
    {
        return Grid[y][x][z]; //y is the vertical slice, x and z are the grid coordinates.
    }
    [System.Serializable]
    public class Tile
    {
        public Tile(int id)
        {
            tileId = id;
            pathCost = 1;
        }
        public Tile() //Empty Tile = 0
        {
            tileId = 0;
            pathCost = 1;
        }
        public int tileId=0;
        public int pathCost=1;
    }
}
