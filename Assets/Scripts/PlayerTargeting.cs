using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    protected Pathfinding pathfinding = new();
    protected Character character = null;
    protected List<GridCell> radius = new List<GridCell>();
    protected int attackRange = 1;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    protected void ReadyCells() //duplicates in movement
    {
        foreach (GridCell cell in GameController.Instance.gridCells)
        {
            cell.Reset();
        }
    }

    public void ResetRadiusCells()
    {
        foreach (GridCell cell in radius)
        {
            cell.IsInSkillRadius = false;
            cell.IsTarget = false;
        }
    }

    private void ResetTargetCells()
    {
        foreach (GridCell cell in radius)
        {
            cell.IsTarget = false;
        }
    }

    public void ShowSkillRadius(int range)
    {
        radius.Clear();

        foreach (GridCell cell in GameController.Instance.gridCells)
        {
            if (pathfinding.ManhattanDistance(transform.position, cell.transform.position) <= range)
            {
                if (cell != character.GetPosition())
                {
                    radius.Add(cell);
                    cell.IsInSkillRadius = true;
                }
            }
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
