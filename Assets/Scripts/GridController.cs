using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public enum State
    {
        Default,
        Movement,
        Skill
    }

    public Character CurrentCharacter { get; set; }
    public State CurrentState { get; set; }

    private GridCell[,] gridCells = null;
    private Pathfinding pathfinding = null;
    private List<GridCell> radius = new();

    void Start()
    {
        gridCells = GameController.Instance.gridCells;
        pathfinding = GameController.Instance.pathfinding;
    }

    private void ReadyCells()
    {
        foreach (GridCell cell in gridCells)
        {
            cell.Reset();
        }
    }

    public void ResetRadiusCells()
    {
        foreach (GridCell cell in radius)
        {
            cell.ResetVisuals();
        }
    }

    public void ShowRadius(int range = 0)
    {
        ResetRadiusCells();
        switch (CurrentState)
        {
            case State.Movement:
                ShowWalkingRadius();
                break;
            case State.Skill:
                ShowSkillRadius(range);
                break;
            case State.Default:
            default:
                Debug.LogError("Show Radius in grid controller doesn't know current state!");
                break;
        }
    }

    private void ShowWalkingRadius()
    {
        radius.Clear();

        List<GridCell> tempRadius = new();

        foreach (GridCell cell in gridCells)
        {
            if (pathfinding.ManhattanDistance(CurrentCharacter.transform.position, cell.transform.position) <= CurrentCharacter.CurrentMovementPoints)
            {
                if (cell != CurrentCharacter.GetPosition())
                {
                    tempRadius.Add(cell);
                }
            }
        }

        Stack<GridCell> temp; //Stack for checking if there is a A* path to cell 
        foreach (GridCell cell in tempRadius)
        {
            ReadyCells();
            temp = pathfinding.Astar(CurrentCharacter.GetPosition(), cell);
            if (temp != null && temp.Count <= CurrentCharacter.CurrentMovementPoints)
            {
                radius.Add(cell);
                cell.IsInWalkRadius = true;
            }
        }
    }

    private void ShowSkillRadius(int range)
    {
        radius.Clear();

        foreach (GridCell cell in gridCells)
        {
            if (pathfinding.ManhattanDistance(CurrentCharacter.transform.position, cell.transform.position) <= range)
            {
                if (cell != CurrentCharacter.GetPosition())
                {
                    radius.Add(cell);
                    cell.IsInSkillRadius = true;
                }
            }
        }
    }

    public void TracePath(Stack<GridCell> path)
    {
        foreach (GridCell cell in path)
        {
            cell.IsPath = true;
        }
    }

    private void ResetTargetCells()
    {
        foreach (GridCell cell in radius)
        {
            cell.IsTarget = false;
        }
    }

    public bool TargetCell(GridCell cell)
    {
        if (radius.Contains(cell))
        {
            ResetTargetCells();
            cell.IsTarget = true;
            return true;
        }
        return false;
    }
}
