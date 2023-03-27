using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private MapGenerator _mapGenerator;

    private void Awake()
    {
        _mapGenerator = FindObjectOfType<MapGenerator>();
    }

    Queue<Tile> AStar(Tile start, Tile goal)
    {
        Dictionary<Tile, Tile> nextTileTowardsGoal = new();
        Dictionary<Tile, int> costToReachTile = new();

        PriorityQueue<Tile> frontier = new();
        frontier.Enqueue(goal, 0);
        costToReachTile[goal] = 0;

        while (frontier.GetElementCount() > 0)
        {
            Tile curTile = frontier.Dequeue();
            if (curTile == start)
                break;

            foreach (Tile neighbour in _mapGenerator.GetNeighbours(curTile))
            {
                int newCost = costToReachTile[curTile] + neighbour.GetCost();
                if (costToReachTile.ContainsKey(neighbour) == false || newCost < costToReachTile[neighbour])
                {
                    if (neighbour.GetTileType() != Tile.TileType.Wall)
                    {
                        costToReachTile[neighbour] = newCost;
                        Debug.Log(Distance(neighbour, start));
                        int priority = newCost + Distance(neighbour, start);
                        frontier.Enqueue(neighbour, priority);
                        nextTileTowardsGoal[neighbour] = curTile;
                        neighbour.SetText(costToReachTile[neighbour].ToString());
                    }
                }
            }
        }

        if (nextTileTowardsGoal.ContainsKey(start) == false)
        {
            return null;
        }

        Queue<Tile> path = new();
        Tile pathTile = start;
        while (goal != pathTile)
        {
            pathTile = nextTileTowardsGoal[pathTile];
            path.Enqueue(pathTile);
        }
        return path;
    }
    public Queue<Tile> GetPath(Tile start, Tile end)
    {
        return AStar(start, end);
    }

    int Distance(Tile t1, Tile t2)
    {
        return Mathf.Abs(t1._X - t2._X) + Mathf.Abs(t1._Y - t2._Y);
    }
}
