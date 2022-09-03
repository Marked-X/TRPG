using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : State
{
    private GridController gridController = null;
    private GameObject selectedObject = null;
    private GridCell target = null;
    private Character character = null;
    private bool hasAttacked = false;

    public StateAttack()
    {
        gridController = GameController.Instance.gridController;
    }

    public override void Enter(GameObject characterGameObj)
    {
        if (!characterGameObj.TryGetComponent(out character))
        {
            Debug.LogError("Attack State couldn't find character component");
            return;
        }
        gridController.CurrentCharacter = character;
        gridController.CurrentState = GridController.State.Skill;
        gridController.ShowRadius(character.Attack.range);
        hasAttacked = false;
    }

    public override void Leave()
    {
        gridController.ResetRadiusCells();
    }

    public override void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject;
                if (selectedObject.TryGetComponent<GridCell>(out GridCell temp))
                {
                    if ((target == null || target != temp) && gridController.TargetCell(temp))
                    {
                        target = temp;
                    }
                    else if (target == temp)
                    {
                        temp.GetAttacked(character.Attack);
                    }
                }
            }
        }
    }

}
