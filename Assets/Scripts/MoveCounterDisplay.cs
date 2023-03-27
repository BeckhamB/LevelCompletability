using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MoveCounterDisplay : MonoBehaviour
{
    public TextMeshProUGUI moveCounterText;
    public GameObject player;
    private TileSwap tileSwapScript;
    // Start is called before the first frame update
    private void Start()
    {
        tileSwapScript = player.GetComponent<TileSwap>(); 
    }

    // Update is called once per frame
    private void Update()
    {
        moveCounterText.gameObject.GetComponent<TextMeshProUGUI>().text = "Number of move remaining: " + tileSwapScript.moveCounter;
    }
}
