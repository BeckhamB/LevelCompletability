using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Ground,
        Wall,
        Rocks
    }

    public GameObject rockGameObj;
    public GameObject wallGameObj;

    public Text text;
    public Color cachedColour;
    private TileType _tileType;

    private Renderer _renderer;
    private int _x;
    private int _y;

    void Awake()
    {
        text = GetComponentInChildren<Text>();
        _renderer = GetComponent<Renderer>();
    }

    public void InitialiseTiles(int x, int y)
    {
        _x = x;
        _y = y;
        name = "Tile_" + x + "_" + y;
    }

    public Color GetColour()
    {
        return _renderer.material.color;
    }
    public void SetColour(Color colour)
    {
        _renderer.material.color = colour;
    }
    public void SetText(string value)
    {
        text.text = value;
    }
    public string GetText()
    {
        return text.ToString();
    }
    public TileType GetTileType()
    {
        return _tileType;
    }
    public void SetTileType(TileType tileType)
    {
        _tileType = tileType;
        switch (_tileType)
        {
            case TileType.Ground:
                rockGameObj.SetActive(false);
                wallGameObj.SetActive(false);
                break;
            case TileType.Wall:
                rockGameObj.SetActive(false);
                wallGameObj.SetActive(true);
                break;
            case TileType.Rocks:
                rockGameObj.SetActive(true);
                wallGameObj.SetActive(false);
                break;
        }
    }
    public int GetCost()
    {
        return _tileType switch
        {
            TileType.Ground => 1,
            TileType.Rocks => 3,
            _ => 0,
        };
    }
    public int GetX()
    {
        return _x;
    }
    public int GetY()
    {
        return _y;
    }
}
