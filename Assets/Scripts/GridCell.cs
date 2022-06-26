using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite = null;
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    public Vector2 Position { get; set; }

    public bool Occupied { get; private set; }

    public int f = 0;
    public int g = 0;
    public int h = 0;
    public GridCell parent = null;

    public void Reset()
    {
        f = int.MaxValue;
        g = int.MaxValue;
        h = int.MaxValue;
        parent = null;
        spriteRenderer.color = Color.white;
    }

    public void TurnRed()
    {
        spriteRenderer.color = Color.red;
    }
}
