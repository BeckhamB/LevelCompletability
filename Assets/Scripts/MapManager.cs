using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class MapManager : MonoBehaviour
{
    public Pathfinding pathfinding;
    public MapGenerator mapGenerator;
    public GameObject playerPrefab;
    public GameObject itemPrefab;
    public TextMeshProUGUI UnreachableItem;
    public TextMeshProUGUI UnreachableGoal;
    public TextMeshProUGUI NotEnoughMoves;
    public TextMeshProUGUI NoStartSelected;
    public GameObject inputField;

    public int maximumMoves = 0;


    private Tile _startTile;
    private Tile _endTile;
    private Vector3 _newStartTile;
    private PathMovement _pathMovement;

    public List<Tile> cachedTile = new();
    public List<GameObject> cachedGameObjects = new();


    private void Update()
    {
        HandleInput();
        UpdateInputField();
        CalculateItemPath();
        MaxMovesManager();
    }

    private Queue<Tile> CalculatePath(Tile newStartTile, Tile newEndTile, GameObject item)
    {
        Queue<Tile> path = pathfinding.GetPath(newStartTile, newEndTile);
        
        if (path == null)
        {
            if(item != null)
            {
                item.GetComponent<HighlightSwap>().SetUnreachableMaterial();
                UnreachableItem.gameObject.SetActive(true);
            }
            else
            {
                UnreachableGoal.gameObject.SetActive(true);
            }
        }

        else
        {
            if (item != null)
            {
                item.GetComponent<HighlightSwap>().SetReachableMaterial();
                UnreachableItem.gameObject.SetActive(false);
            }
            foreach (Tile t in path)
            {
                t.SetColour(new Color(1, 0.6f, 0));
            }
            
            newEndTile.SetColour(Color.red);
            newEndTile.SetText("End");
            newStartTile.SetColour(Color.cyan);
            newStartTile.SetText("Start");
            UnreachableGoal.gameObject.SetActive(false);
        }
        return path;
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
                selectedTile.SetTileType(Tile.TileType.Ground);
                RepaintMap();
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            Tile selectedTile = GetSelectedTile();
            if (selectedTile != null)
            {
                selectedTile.SetTileType(Tile.TileType.Rocks);
                RepaintMap();
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            Tile selectedTile = GetSelectedTile();
            if (selectedTile != null)
            {
                if(selectedTile != _startTile && selectedTile != _endTile)
                {
                    selectedTile.SetTileType(Tile.TileType.Wall);
                    RepaintMap();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Tile t in mapGenerator.grid)
            {
                t.SetText(t.GetX() + ", " + t.GetY());
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
        if(Input.GetKeyDown(KeyCode.R))
        {
            Tile selectedTile = GetSelectedTile();
            if(selectedTile != null)
            {
                Vector3 newItemTilePos = new Vector3(selectedTile.transform.position.x, selectedTile.transform.position.y + 1f, selectedTile.transform.position.z);
                if (selectedTile != null && _startTile != null && selectedTile != _startTile)
                {
                    cachedGameObjects.Add(Instantiate(itemPrefab, newItemTilePos, Quaternion.identity));
                    RepaintMap();
                    cachedTile.Add(selectedTile);
                    CalculatePath(_startTile, selectedTile, cachedGameObjects.Last());
                }
                if (_startTile == null)
                {
                    Debug.LogError("There is no starting location selected");
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Y))
        {
            RemoveItems();
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            RemoveLastItem();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
            CalculatePath(_startTile, _endTile, null);
        }
    }

    private void RemoveItems()
    {
        cachedTile.Clear();
        foreach(GameObject obj in cachedGameObjects)
        {
            Destroy(obj);
        }
        cachedGameObjects.Clear();
        RepaintMap();
    }
    private void RemoveLastItem()
    {
        if(cachedTile.Count > 0 && cachedGameObjects.Count > 0)
        {
            cachedTile.Remove(cachedTile.Last());
            Destroy(cachedGameObjects.Last());
            cachedGameObjects.Remove(cachedGameObjects.Last());
            RepaintMap();
        }
    }
    private void CalculateItemPath()
    {
        if (cachedTile != null && _startTile != null)
        {
            foreach (Tile t in cachedTile)
            {
                foreach(GameObject obj in cachedGameObjects)
                {
                    CalculatePath(_startTile, t, obj);
                }
            }
        }
    }

    private void MaxMovesManager()
    {
        if (maximumMoves > 0)
        {
            if (_startTile != null && _endTile != null)
            {
                if (pathfinding.Distance(_startTile, _endTile) > maximumMoves)
                {
                    NotEnoughMoves.gameObject.SetActive(true);
                }
                else
                {
                    NotEnoughMoves.gameObject.SetActive(false);
                }
            }
        }
    }
    private void UpdateInputField()
    {
        if (inputField.GetComponent<TMP_InputField>().text != null)
        {
            int.TryParse(inputField.GetComponent<TMP_InputField>().text, out int result);
            maximumMoves = result;
        }
    }
}
