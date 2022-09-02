using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private GridCell[,] gridCells = null;

    private int gridWidth;
    private int gridHeight;
    private List<GridCell> openList;
    private List<GridCell> closedList;
    private Vector3[] directions = new Vector3[4] { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

    private GridCell start = null;
    private GridCell finish = null;

    public Pathfinding() { }

    public Pathfinding(GridCell[,] gridCells, int gridWidth, int gridHeight)
    {
        this.gridCells = gridCells;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
    }

    public void Ready()
    {
        gridCells = GameController.Instance.gridCells;
        gridWidth = GameController.Instance.gridWidth;
        gridHeight = GameController.Instance.gridHeight;
    }

    public Stack<GridCell> Astar(GridCell a, GridCell b)
    {
        start = a;
        finish = b;

        openList = new List<GridCell>();
        closedList = new List<GridCell>();

        start.f = 0;
        start.g = 0;
        start.h = 0;
        start.parent = start;
        openList.Add(start);

        while (openList.Count > 0)
        {
            GridCell q = null;
            foreach (GridCell cell in openList)
            {
                if (q == null || q.f > cell.f)
                {
                    q = cell;
                }
            }

            openList.Remove(q);

            int x = (int)q.Position.x;
            int y = (int)q.Position.y;

            GridCell successor;

            foreach(Vector3 direction in directions)
            {
                int tempX = x + (int)direction.x, tempY = y + (int)direction.y;
                if (IsValid(tempX, tempY))
                {
                    if(!gridCells[tempX, tempY].IsOccupied)
                    {
                        successor = gridCells[tempX, tempY];

                        if (CheckSuccessor(successor, q))
                        {
                            return MakePath();
                        }
                    }                    
                }
            }

            closedList.Add(q);
        }

        return null;
    }

    private Stack<GridCell> MakePath()
    {
        Stack<GridCell> path = new Stack<GridCell>();
        GridCell temp = finish;
        do
        {
            path.Push(temp);
            temp = temp.parent;
        } while (temp != start);

        return path;
    }

    private bool CheckSuccessor(GridCell successor, GridCell q)
    {

        if (successor == finish)
        {
            finish.parent = q;
            return true;
        }
        else if (!closedList.Contains(successor) && !successor.IsOccupied)
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

    private bool IsValid(float x, float y)
    {
        if (x < gridWidth && x >= 0 && y >= 0 && y < gridHeight)
            return true;
        else
            return false;
    }

    public int ManhattanDistance(Vector3 current, Vector3 target)
    {
        return (int)(Mathf.Abs(current.x - target.x) + Mathf.Abs(current.y - target.y));
    }

}
