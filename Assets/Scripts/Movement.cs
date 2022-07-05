using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    protected Pathfinding pathfinding = new();
    protected Character character = null;
    protected List<GridCell> radius = new List<GridCell>();

    protected int maxMovementPoints = 4;
    protected int currentMovementPoints = 4;
    public int CurrentMovementPoints { get => currentMovementPoints; private set => currentMovementPoints = value; }

    public void RefreshMovementPoints()
    {
        currentMovementPoints = maxMovementPoints;
    }

    protected void ReadyCells()
    {
        foreach (GridCell cell in GameController.Instance.gridCells)
        {
            cell.Reset();
        }
    }

    protected void ResetRadiusCells()
    {
        foreach (GridCell cell in GameController.Instance.gridCells)
        {
            cell.IsInRadius = false;
        }
    }

    public void ShowWalkingRadius()
    {
        radius.Clear();

        foreach (GridCell cell in GameController.Instance.gridCells)
        {
            if (pathfinding.ManhattanDistance(transform.position, cell.transform.position) <= currentMovementPoints)
            {
                if (cell != character.GetPosition())
                {
                    radius.Add(cell);
                }
            }
        }

        Stack<GridCell> temp;
        foreach (GridCell cell in radius)
        {
            ReadyCells();
            temp = pathfinding.Astar(character.GetPosition(), cell);
            if (temp != null && temp.Count <= currentMovementPoints)
            {
                cell.IsInRadius = true;
            }
        }
    }
}
