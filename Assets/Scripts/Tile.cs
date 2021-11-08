using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : PositionedObject
{
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material whiteMaterial;

    public override Position Position
    {
        set
        {
            base.Position = value;
            GetComponent<Renderer>().material = ((value.Column + value.Row) % 2 == 0) ? blackMaterial : whiteMaterial;
        }
    }
}
