using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Pathfinding pathfinding = new Pathfinding();
    private Character player = null;
    private Stack<GridCell> path = new Stack<GridCell>();

    public void Ready()
    {
        player = gameObject.GetComponent<Character>();
        pathfinding.Ready();
    }

    public void MovePlayer()
    {
        StartCoroutine(Move());
    }

    public IEnumerator Move()
    {
        while(path.Count > 0)
        {
            GridCell cell = path.Pop();
            yield return StartCoroutine(player.Move(cell.transform.position));
            player.SetPosition(cell);
            cell.Reset();
        }
    }

    public void Trace(GridCell finish)
    {
        GridCell start = player.GetPosition();
        ReadyCells();
        pathfinding.Astar(start, finish);
        path.Clear();
        GridCell cell = finish;
        do
        {
            cell.TurnRed();
            path.Push(cell);
            cell = cell.parent;
        } while (cell != start);
    }

    private void ReadyCells()
    {
        foreach(GridCell cell in GameController.Instance.gridCells)
        {
            cell.Reset();
        }
    }
}
