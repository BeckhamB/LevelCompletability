using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Pathfinding pathfinding;
    public MapGenerator mapGenerator;
    public GameObject playerPrefab;
    public GameObject itemPrefab;

    private Tile _startTile;
    private Tile _endTile;
    private Vector3 _newStartTile;
    private PathMovement _pathMovement;


    private void Update()
    {
        HandleInput();
    }

    private void CalculatePath(Tile newStartTile, Tile newEndTile)
    {
        Queue<Tile> path = pathfinding.GetPath(newStartTile, newEndTile);

        if (path == null)
        {
            Debug.LogWarning("Goal not reachable");
        }

        else
        {
            foreach (Tile t in path)
            {
                t.SetColour(new Color(1, 0.6f, 0));
            }

            newEndTile.SetColour(Color.red);
            newEndTile.SetText("End");
            newStartTile.SetColour(Color.cyan);
            newStartTile.SetText("Start");
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Tile selectedTile = GetSelectedTile();
            if (selectedTile != null && selectedTile.GetTileType() == Tile.TileType.Wall)
            {
                Debug.LogWarning("Can't start or end on Walls!");
                return;
            }

            if (selectedTile != null)
            {
                _startTile = selectedTile;
            }

            RepaintMap();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Tile tileUnderMouse = GetSelectedTile();
            if (tileUnderMouse != null && tileUnderMouse.GetTileType() == Tile.TileType.Wall)
            {
                Debug.LogWarning("Can't start or end on Walls!");
                return;
            }


            if (tileUnderMouse != null)
            {
                _endTile = tileUnderMouse;
            }
            RepaintMap();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Tile selectedTile = GetSelectedTile();
            if (selectedTile != null)
            {
                selectedTile.SetTileType(Tile.TileType.Plains);
                RepaintMap();
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            Tile selectedTile = GetSelectedTile();
            if (selectedTile != null)
            {
                selectedTile.SetTileType(Tile.TileType.Wood);
                RepaintMap();
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            Tile selectedTile = GetSelectedTile();
            if (selectedTile != null)
            {
                selectedTile.SetTileType(Tile.TileType.Wall);
                RepaintMap();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Tile t in mapGenerator.grid)
            {
                t.SetText(t._X + ", " + t._Y);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            RepaintMap();
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            _newStartTile = new Vector3(_startTile.transform.position.x, _startTile.transform.position.y + 2, _startTile.transform.position.z);
            if (_pathMovement == null)
            {
                
                _pathMovement = Instantiate(playerPrefab, _newStartTile, Quaternion.identity).GetComponent<PathMovement>();
            }
            else
                _pathMovement.transform.position = _newStartTile;

            Queue<Tile> generatedPath = pathfinding.GetPath(_startTile, _endTile);
            _pathMovement.SetPath(generatedPath);
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            Tile selectedTile = GetSelectedTile();
            Vector3 newItemTilePos = new Vector3(selectedTile.transform.position.x, selectedTile.transform.position.y + 2f, selectedTile.transform.position.z);
            if (selectedTile != null && _startTile != null)
            {
                GameObject spawnedItem = Instantiate(itemPrefab, newItemTilePos, Quaternion.identity);
                RepaintMap();
                CalculatePath(_startTile, selectedTile);
            }
            if(_startTile == null)
            {
                Debug.LogError("There is no starting location selected");
            }
        }
    }

    private Tile GetSelectedTile()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        bool hitTarget = Physics.Raycast(cameraRay, out rayHit, int.MaxValue, LayerMask.GetMask("Tiles"));
        if (hitTarget)
        {
            return rayHit.transform.GetComponent<Tile>();
        }
        else
        {
            return null;
        }
    }

    public void RepaintMap()
    {
        mapGenerator.ResetTiles();

        if (_endTile != null)
        {
            _endTile.SetColour(Color.red);
            _endTile.SetText("End");
        }
        if (_startTile != null)
        {
            _startTile.SetColour(Color.green);
            _startTile.SetText("Start");
        }

        if (_startTile != null && _endTile != null)
        {
            CalculatePath(_startTile, _endTile);
        }
    }
}
