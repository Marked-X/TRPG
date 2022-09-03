using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateMovement : State
{
    private Stack<GridCell> path = new();

    private Character character = null;

    private GridController gridController = null;
    private Pathfinding pathfinding = null;
    private GameObject SelectedObject = null;
    private TextMeshProUGUI movementPointsText = null;
    private GridCell destination = null;
    private bool isMoving = false;

    public StateMovement()
    {
        gridController = GameController.Instance.gridController;
        pathfinding = GameController.Instance.pathfinding;
    }

    public override void Enter(GameObject characterGameObj)
    {
        if (!characterGameObj.TryGetComponent(out character))
        {
            Debug.LogError("Movement State couldn't find movement component");
            return;
        }
        gridController.CurrentCharacter = character;
        gridController.CurrentState = GridController.State.Movement;
        gridController.ShowRadius();
        character.OnMovementEnded += MovementEnded;
        movementPointsText = GameController.Instance.movementPointsText;
    }

    public override void Leave()
    {
        gridController.ResetRadiusCells();
        character.OnMovementEnded -= MovementEnded;
        gridController.CurrentState = GridController.State.Default;
    }


    public override void Update()
    {
        if (isMoving == true)
            return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                SelectedObject = targetObject.transform.gameObject;
                if (SelectedObject.TryGetComponent<GridCell>(out GridCell temp))
                {
                    if ((destination == null || destination != temp))
                    {
                        path = pathfinding.Astar(character.GetPosition(), temp);
                        if (path != null)
                        {
                            destination = temp;
                            gridController.TracePath(path);
                        }                        
                    }
                    else if (destination == temp)
                    {
                        character.BeginMoving(path);
                    }
                }
            }
        }
    }

    private void MovementEnded()
    {
        gridController.ShowRadius();
        movementPointsText.text = "Movement: " + character.CurrentMovementPoints;
    }
}
