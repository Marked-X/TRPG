using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMovement : State
{
    public override GameObject SelectedObject { get; set; }

    public PlayerMovement playerMovement = null;
    private GridCell destination = null;
    private bool isMoving = false;
    private bool isFreeroam = true;

    public override void Enter()
    {
        if (playerMovement == null)
            playerMovement = GameController.Instance.player.GetComponent<PlayerMovement>();
        playerMovement.OnMovementEnded += OnPlayerMovementEnded;
        playerMovement.ShowWalkingRadius();
    }

    public override void Leave()
    {
        playerMovement.OnMovementEnded -= OnPlayerMovementEnded;
    }

    private void OnPlayerMovementEnded()
    {
        if (playerMovement.CurrentMovementPoints == 0 && isFreeroam)
        {
            playerMovement.RefreshMovementPoints();
            playerMovement.ShowWalkingRadius();
        }
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
