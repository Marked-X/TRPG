using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    private Pathfinding pathfinding;
    private Character character = null;
    private GridCell[,] gridCells = null;
    private List<GridCell> radius = new();
    private Stack<GridCell> path = new();

    void Start()
    {
        gridCells = GameController.Instance.gridCells;
        pathfinding = new(gridCells, GameController.Instance.gridWidth, GameController.Instance.gridHeight);
    }

    private void ReadyCells()
    {
        foreach (GridCell cell in gridCells)
        {
            cell.Reset();
        }
    }

    protected void ResetRadiusCells()
    {
        foreach (GridCell cell in radius)
        {
            cell.ResetVisuals();
        }
    }

    public void ShowWalkingRadius()
    {
        radius.Clear();

        List<GridCell> tempRadius = new();

        foreach (GridCell cell in gridCells)
        {
            if (pathfinding.ManhattanDistance(character.transform.position, cell.transform.position) <= character.CurrentMovementPoints)
            {
                if (cell != character.GetPosition())
                {
                    tempRadius.Add(cell);
                }
            }
        }

        Stack<GridCell> temp; //Stack for checking if there is a A* path to cell 
        foreach (GridCell cell in tempRadius)
        {
            ReadyCells();
            temp = pathfinding.Astar(character.GetPosition(), cell);
            if (temp != null && temp.Count <= character.CurrentMovementPoints)
            {
                radius.Add(cell);
                cell.IsInWalkRadius = true;
            }
        }
    }

    public void ShowSkillRadius(int range)
    {
        radius.Clear();

        foreach (GridCell cell in gridCells)
        {
            if (pathfinding.ManhattanDistance(character.transform.position, cell.transform.position) <= range)
            {
                if (cell != character.GetPosition())
                {
                    radius.Add(cell);
                    cell.IsInSkillRadius = true;
                }
            }
        }
    }

    public bool Trace(GridCell finish)
    {
        if (!radius.Contains(finish) || finish.IsOccupied)
            return false;

        GridCell start = character.GetPosition();
        ReadyCells();
        if (path != null)
            path.Clear();
        path = pathfinding.Astar(start, finish);

        if (path.Count <= 0 || path == null)
            return false;

        foreach (GridCell cell in path)
        {
            cell.IsPath = true;
        }

        return true;
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
