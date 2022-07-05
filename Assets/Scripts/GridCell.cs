using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite = null;
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;
    [SerializeField]
    private GameObject selectedBorder = null;
    [SerializeField]
    private GameObject radiusBorder = null;
    [SerializeField]
    private GameObject pathBorder = null;

    public Vector2 Position { get; set; }

    private bool isInRadius;
    public bool IsInRadius
    {
        get
        {
            return isInRadius;
        }
        set
        {
            isInRadius = value;
            radiusBorder.SetActive(value);
        }
    }

    private bool isPath;
    public bool IsPath
    {
        get
        {
            return isPath;
        }
        set
        {
            isPath = value;
            pathBorder.SetActive(value);
        }
    }

    private bool isSelected;
    public bool IsSelected 
    { 
        get 
        {
            return isSelected;
        } 
        set 
        {
            isSelected = value;
            selectedBorder.SetActive(value);
        }
    }

    public bool IsOccupied { get; set; } = false;

    [HideInInspector]
    public int f = int.MaxValue;
    [HideInInspector]
    public int g = int.MaxValue;
    [HideInInspector]
    public int h = int.MaxValue;

    public GridCell parent = null;

    public void Reset()
    {
        f = int.MaxValue;
        g = int.MaxValue;
        h = int.MaxValue;
        parent = null;
        IsPath = false;
    }

    private void OnMouseEnter()
    {
        IsSelected = true;
    }

    private void OnMouseExit()
    {
        IsSelected = false;
    }
}
