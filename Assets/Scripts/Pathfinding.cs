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
            Tile currentTile = frontier.Dequeue();
            if (currentTile == start)
                break;

            foreach (Tile neighbour in _mapGenerator.GetNeighbours(currentTile))
            {
                int newCost = costToReachTile[currentTile] + neighbour.GetCost();
                if (costToReachTile.ContainsKey(neighbour) == false || newCost < costToReachTile[neighbour])
                {
                    if (neighbour.GetTileType() != Tile.TileType.Wall)
                    {
                        costToReachTile[neighbour] = newCost;
                        int priority = newCost + Distance(neighbour, start);
                        frontier.Enqueue(neighbour, priority);
                        nextTileTowardsGoal[neighbour] = currentTile;
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

    public int Distance(Tile t1, Tile t2)
    {
        return Mathf.Abs(t1.GetX() - t2.GetX()) + Mathf.Abs(t1.GetY() - t2.GetY());
    }
}
