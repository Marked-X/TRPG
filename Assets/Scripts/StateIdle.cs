using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : State
{
    public override void Enter(GameObject characterGameObj)
    {
        PlayerMovement playerMovement = null;
        if (characterGameObj.TryGetComponent(out playerMovement))
        {
            playerMovement.RefreshMovementPoints();
        }
    }

    public override void Leave()
    {
    }

    public override void Update()
    {
    }
}
