using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSwap : MonoBehaviour
{
    public Material ReachableMaterial;
    public Material UnreachableMaterial;

    public void SetUnreachableMaterial()
    {
        this.GetComponent<Renderer>().material = UnreachableMaterial;
    }
    public void SetReachableMaterial()
    {
        this.GetComponent<Renderer>().material = ReachableMaterial;
    }
}
