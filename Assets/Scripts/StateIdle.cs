using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : State
{
    private Character character = null;

    public override void Enter(GameObject characterGameObj)
    {
        if (characterGameObj.TryGetComponent(out character))
        {
            character.RefreshMovementPoints();
        }
    }

    public override void Leave()
    {
    }

    public override void Update()
    {
    }
}
