using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public int sizeX = 15;
    public int sizeY = 15;

    public Tile[,] grid;


    private Dictionary<Tile, Tile[]> neighbourDictionary;
    public Tile[] GetNeighbours(Tile tile)
    {
        return neighbourDictionary[tile];
    }

    void Awake()
    {
        grid = new Tile[sizeX, sizeY];
        neighbourDictionary = new Dictionary<Tile, Tile[]>();
        GenerateMap(sizeX, sizeY);
        foreach (Tile t in grid)
        {
            t.cachedColour = t.GetColour();
        }
    }


    void GenerateMap(int sizeX, int sizeY)
    {
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                grid[x, y] = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity).GetComponent<Tile>();
                grid[x, y].InitialiseTiles(x, y);
            }
        }

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                List<Tile> neighbours = new();
                if (y < sizeY-1)
                {
                    neighbours.Add(grid[x, y + 1]);
                }
                if (x < sizeX-1)
                {
                    neighbours.Add(grid[x + 1, y]);
                }
                if (y > 0)
                {
                    neighbours.Add(grid[x, y - 1]);
                }
                if (x > 0)
                {
                    neighbours.Add(grid[x - 1, y]);
                }

                neighbourDictionary.Add(grid[x, y], neighbours.ToArray());
            }
        }
    }

    public void ResetTiles()
    {
        foreach(Tile t in grid)
        {
            t.SetColour(t.cachedColour);
            t.SetText("");
        }
    }
}
