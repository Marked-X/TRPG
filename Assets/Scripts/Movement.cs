using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    GridCell[,] gridCells = null;
    List<GridCell> openList;
    List<GridCell> closedList;

    int gridWidth;
    int gridHeight;

    GridCell start = null;
    GridCell finish = null;

    public void Ready()
    {
        gridCells = GameController.Instance.gridCells;
        gridWidth = GameController.Instance.gridWidth;
        gridHeight = GameController.Instance.gridHeight;
    }

    public void Astar(GridCell a, GridCell b)
    {
        start = a;
        finish = b;

        openList = new List<GridCell>();
        closedList = new List<GridCell>();
        ReadyCells();

        start.f = 0;
        start.g = 0;
        start.h = 0;
        start.parent = start;
        openList.Add(start);

        while (openList.Count > 0)
        {
            GridCell q = null;
            GridCell successor = null;

            foreach(GridCell cell in openList)
            {
                if (q == null || q.f > cell.f)
                {
                    q = cell;
                }
            }

            openList.Remove(q);

            int x = (int)q.Position.x;
            int y = (int)q.Position.y;

            //right
            if (IsValid(x + 1, y))
            {
                successor = gridCells[x + 1, y];

                if (CheckSuccessor(successor, q))
                {
                    return;
                }
            }

            //left
            if (IsValid(x - 1, y))
            {
                successor = gridCells[x - 1, y];
                if (CheckSuccessor(successor, q))
                {
                    return;
                }
            }

            //down
            if (IsValid(x, y - 1))
            {
                successor = gridCells[x, y - 1];
                if (CheckSuccessor(successor, q))
                {
                    return;
                }
            }

            //up
            if (IsValid(x, y + 1))
            {
                successor = gridCells[x, y + 1];
                if (CheckSuccessor(successor, q))
                {
                    return;
                }
            }


            closedList.Add(q);
        }
    }

    private bool CheckSuccessor(GridCell successor, GridCell q)
    {

        if (successor == finish)
        {
            finish.parent = q;
            Trace(start, finish);
            return true;
        }
        else if (!closedList.Contains(successor) && !successor.occupied)
        {
            int newG, newH, newF;

            newG = q.g + 1;
            newH = ManhattanDistance(successor.Position, finish.Position);
            newF = newG + newH;

            if (successor.f == int.MaxValue || successor.f > newF)
            {
                successor.f = newF;
                successor.g = newG;
                successor.h = newH;
                successor.parent = q;
                openList.Add(successor);
            }
        }
        return false;
    }

    private void Trace(GridCell start, GridCell finish)
    {
        GridCell cell = finish;
        do
        {
            Debug.Log(cell.parent.Position);
            cell.TurnRed();
            cell = cell.parent;
        } while (cell != start);
    }

    private bool IsValid(float x, float y)
    {
        if (x < gridWidth && x >= 0 && y >= 0 & y < gridHeight)
            return true;
        else
            return false;
    }

    private void ReadyCells()
    {
        foreach(GridCell cell in gridCells)
        {
            cell.Reset();
        }
    }

    private int ManhattanDistance(Vector3 current, Vector3 target)
    {
        return (int)(Mathf.Abs(current.x - target.x) + Mathf.Abs(current.y - target.y));
    }
}
