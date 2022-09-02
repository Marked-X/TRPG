using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateMovement : State
{
    public PlayerMovement playerMovement = null;

    private GameObject SelectedObject = null;
    private TextMeshProUGUI movementPointsText = null;
    private GridCell destination = null;
    private bool isMoving = false;
    private bool isFreeroam = false;

    public override void Enter(GameObject characterGameObj)
    {
        if(!characterGameObj.TryGetComponent(out playerMovement))
        {
            Debug.LogError("Movement State couldn't find movement component");
            return;
        }
        playerMovement.OnMovementEnded += OnPlayerMovementEnded;
        playerMovement.ShowWalkingRadius();
        movementPointsText = GameController.Instance.movementPointsText;
    }

    public override void Leave()
    {
        foreach(GridCell cell in GameController.Instance.gridCells)
        {
            cell.ResetVisuals();
        }
        playerMovement.OnMovementEnded -= OnPlayerMovementEnded;
    }

    private void OnPlayerMovementEnded()
    {
        if (playerMovement.CurrentMovementPoints == 0 && isFreeroam)
        {
            playerMovement.RefreshMovementPoints();
            playerMovement.ShowWalkingRadius();
        }
        movementPointsText.text = "Movement: " + playerMovement.CurrentMovementPoints;
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
                    if ((destination == null || destination != temp) && playerMovement.Trace(temp))
                    {
                        destination = temp;
                    }
                    else if (destination == temp)
                    {
                        playerMovement.MovePlayer();
                    }
                }
            }
        }
    }


}
