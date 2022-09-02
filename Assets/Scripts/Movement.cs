using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    protected Pathfinding pathfinding;
    protected Character character = null;
    protected List<GridCell> radius = new List<GridCell>();

    protected int maxMovementPoints = 4;
    protected int currentMovementPoints = 4;
    public int CurrentMovementPoints { get => currentMovementPoints; private set => currentMovementPoints = value; }

    protected void Start()
    {
        pathfinding =  GameController.Instance.pathfinding;
        character = gameObject.GetComponent<Character>();
    }

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
            cell.IsInWalkRadius = false;
        }
    }

    public void ShowWalkingRadius()
    {
        radius.Clear();

        List<GridCell> tempRadius = new();

        foreach (GridCell cell in GameController.Instance.gridCells)
        {
            if (pathfinding.ManhattanDistance(transform.position, cell.transform.position) <= currentMovementPoints)
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
            if (temp != null && temp.Count <= currentMovementPoints)
            {
                radius.Add(cell);
                cell.IsInWalkRadius = true;
            }
        }
    }
}
