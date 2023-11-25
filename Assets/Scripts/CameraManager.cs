using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    public WorldManager worldManager;
    public int YLevel = 0;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Comma))
        {
            YLevel--;
        }
        else if (Input.GetKeyDown(KeyCode.Period))
        {
            YLevel++;
        }
        BoundYLevel();
        RenderYLevel();
        DebugMousePaint();
    }
    public void DebugMousePaint()
    {
        World world = worldManager.world;
        Vector3Int mPos = Vector3Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0.15f, 0.15f, 0f));
        mPos.z = YLevel;

        if (Input.GetMouseButton(0))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                worldManager.GetComponent<Pathfinder>().StartLoc.transform.position = mPos;
                worldManager.GetComponent<Pathfinder>().StartLoc.GetComponent<SpriteRenderer>().sortingOrder =1+ YLevel*2;
            }
            else if (Input.GetKey(KeyCode.RightShift))
            {
                if (world.isPos(mPos))
                {
                    world.getTile(mPos).tileId = 1;
                    world.getTile(mPos).pathCost = 1;
                    worldManager.RenderTile(mPos);
                }
            }
            else
            {

                if (world.isPos(mPos))
                {
                    world.getTile(mPos).tileId = 0;
                    world.getTile(mPos).pathCost = -1;
                    worldManager.RenderTile(mPos);
                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                worldManager.GetComponent<Pathfinder>().StopLoc.transform.position = mPos;
                worldManager.GetComponent<Pathfinder>().StopLoc.GetComponent<SpriteRenderer>().sortingOrder =1+ YLevel*2;
            }
            else if (Input.GetKey(KeyCode.RightShift))
            {
                if (world.isPos(mPos))
                {
                    world.getTile(mPos).tileId = 3;
                    world.getTile(mPos).pathCost = 1;
                    worldManager.RenderTile(mPos);
                }
            }
            else
            {

                if (world.isPos(mPos))
                {
                    world.getTile(mPos).tileId = 2;
                    world.getTile(mPos).pathCost = 1;
                    worldManager.RenderTile(mPos);
                }
            }
        }
    }
    public void RenderYLevel()
    {
        //Debug Circles:
        worldManager.GetComponent<Pathfinder>().StartLoc.SetActive(worldManager.GetComponent<Pathfinder>().StartLoc.GetComponent<SpriteRenderer>().sortingOrder-1 <= 2 * YLevel);
        worldManager.GetComponent<Pathfinder>().StopLoc.SetActive(worldManager.GetComponent<Pathfinder>().StopLoc.GetComponent<SpriteRenderer>().sortingOrder-1 <= 2 * YLevel);
        //

        Color transparent = new Color(1, 1, 1, 0);
        for (int i = 0; i < worldManager.Slices.Length; i++)
        {
            Tilemap t = worldManager.Slices[i];

            if (i > YLevel)
                t.color = transparent;
            else if (i==YLevel)
            {
                t.color = Color.white;
            }
            else  //i<YLevel
            {
                t.color=new Color(0.9f,0.9f,0.9f,(((float)i+1)/(YLevel+1)));
            }

        }
    }
    public void BoundYLevel()
    {
        if (YLevel >= worldManager.Slices.Length)
        {
            YLevel = worldManager.Slices.Length - 1;
        }
        else if (YLevel < 0)
        {
            YLevel = 0;
        }
    }
}
