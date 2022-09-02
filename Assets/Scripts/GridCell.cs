using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedBorder = null;
    [SerializeField]
    private GameObject walkRadiusBorder = null;
    [SerializeField]
    private GameObject skillRadiusBorder = null;
    [SerializeField]
    private GameObject targetBorder = null;
    [SerializeField]
    private GameObject pathBorder = null;
    
    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }


    public Vector2 Position { get; set; }

    private bool isInWalkRadius;
    public bool IsInWalkRadius
    {
        get
        {
            return isInWalkRadius;
        }
        set
        {
            isInWalkRadius = value;
            walkRadiusBorder.SetActive(value);
        }
    }

    private bool isInSkillRadius;
    public bool IsInSkillRadius
    {
        get
        {
            return isInSkillRadius;
        }
        set
        {
            isInSkillRadius = value;
            skillRadiusBorder.SetActive(value);
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

    private bool isTarget;
    public bool IsTarget
    {
        get
        {
            return isTarget;
        }
        set
        {
            isTarget = value;
            targetBorder.SetActive(value);
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

    public bool IsOccupied 
    {
        get
        {
            return isOccupied;
        }
        set
        {
            isOccupied = value;
        }
    }

    public bool isOccupied = false;

    private Character occupator = null;

    public Character Occupator
    { 
        get 
        {
            return occupator;
        }
        set
        {
            occupator = value;
            if (value)
                IsOccupied = true;
            else
                IsOccupied = false;
        } 
    }

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

    public void ResetVisuals() //can potentially move into Reset function
    {
        IsSelected = false;
        IsInWalkRadius = false;
        IsInSkillRadius = false;
        IsTarget = false;
        IsPath = false;
    }

    private void OnMouseEnter()
    {
        IsSelected = true;
        if (Occupator)
        {
            GameController.Instance.targetInfo.TargetEnter(this);
        }
    }

    private void OnMouseExit()
    {
        IsSelected = false;
        GameController.Instance.targetInfo.TargetLeave();
    }

    public void GetAttacked(SkillSO skill)
    {
        if (Occupator)
        {
            Occupator.GetAttacked(skill);
        }
    }
}
