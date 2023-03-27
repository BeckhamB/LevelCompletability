using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Plains,
        Wall,
        Wood
    }

    public GameObject _woodGO;
    public GameObject _wallGO;

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
            case TileType.Plains:
                _woodGO.SetActive(false);
                _wallGO.SetActive(false);
                break;
            case TileType.Wall:
                _woodGO.SetActive(false);
                _wallGO.SetActive(true);
                break;
            case TileType.Wood:
                _woodGO.SetActive(true);
                _wallGO.SetActive(false);
                break;
        }
    }
    public int GetCost()
    {
        return _tileType switch
        {
            TileType.Plains => 1,
            TileType.Wood => 5,
            _ => 0,
        };
    }
    public int _X => _x; 
    public int _Y => _y;
}
