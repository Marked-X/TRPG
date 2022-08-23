using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter(GameObject characterGameObj);

    public abstract void Update();

    public abstract void Leave();
}
