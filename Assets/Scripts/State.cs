using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract GameObject SelectedObject { get; set; }

    public abstract void Enter();

    public abstract void Update();

    public abstract void Leave();
}
