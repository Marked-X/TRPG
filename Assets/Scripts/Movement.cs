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

    public void Ready()
    {
        gridCells = GameController.Instance.gridCells;
        gridWidth = GameController.Instance.gridWidth;
        gridHeight = GameController.Instance.gridHeight;
    }

    public void Astar(GridCell start, GridCell finish)
    {
        bool foundDest = false;
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
            int newG, newH, newF;

            //right
            if (IsValid(x + 1, y))
            {
                successor = gridCells[x + 1, y];
                if (successor == finish)
                {
                    finish.parent = q;
                    foundDest = true;
                    Trace(start, finish);
                    return;
                }
                else if (!closedList.Contains(successor) || !successor.Occupied)
                {
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
            }

            //left
            if (IsValid(x - 1, y))
            {
                successor = gridCells[x - 1, y];
                if (successor == finish)
                {
                    finish.parent = q;
                    foundDest = true;
                    Trace(start, finish);
                    return;
                }
                else if (!closedList.Contains(successor) || !successor.Occupied)
                {
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
            }

            //down
            if (IsValid(x, y - 1))
            {
                successor = gridCells[x, y - 1];
                if (successor == finish)
                {
                    finish.parent = q;
                    foundDest = true;
                    Trace(start, finish);
                    return;
                }
                else if (!closedList.Contains(successor) || !successor.Occupied)
                {
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
            }

            //up
            if (IsValid(x, y + 1))
            {
                successor = gridCells[x, y + 1];
                if (successor == finish)
                {
                    finish.parent = q;
                    foundDest = true;
                    Trace(start, finish);
                    return;
                }
                else if (!closedList.Contains(successor) || !successor.Occupied)
                {
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
            }


            closedList.Add(q);
        }
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
