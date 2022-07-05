using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    private Stack<GridCell> path = null;
    public delegate void MovementEnded();
    public event MovementEnded OnMovementEnded;

    public void Ready()
    {
        character = gameObject.GetComponent<Character>();
        pathfinding.Ready();
    }

    public void MovePlayer()
    {
        StartCoroutine(Move());
    }

    public IEnumerator Move()
    {
        if (path == null)
        {
            Debug.LogWarning("No path");
            StopCoroutine(Move());
        }

        while(path.Count > 0)
        {
            GridCell cell = path.Pop();
            currentMovementPoints--;
            yield return StartCoroutine(character.Move(cell.transform.position));
            character.SetPosition(cell);
            cell.Reset();
        }

        ResetRadiusCells();
        ShowWalkingRadius();
        OnMovementEnded?.Invoke();
    }

    public bool Trace(GridCell finish)
    {
        if (!radius.Contains(finish))
            return false;

        GridCell start = character.GetPosition();
        ReadyCells();
        if (path != null)
            path.Clear();
        path = pathfinding.Astar(start, finish);

        if (path.Count <= 0 || path == null)
            return false;

        foreach(GridCell cell in path)
        {
            cell.IsPath = true;
        }

        return true;
    } 
}
