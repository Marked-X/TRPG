using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : State
{
    private GameObject selectedObject = null;
    private GridCell target = null;
    private PlayerTargeting playerTargeting = null;
    private Character character = null;
    private bool hasAttacked = false;

    public override void Enter(GameObject characterGameObj)
    {
        if (!characterGameObj.TryGetComponent(out playerTargeting))
        {
            Debug.LogError("Attack State couldn't find targeting component");
            return;
        }
        if (!characterGameObj.TryGetComponent(out character))
        {
            Debug.LogError("Attack State couldn't find character component");
            return;
        }
        playerTargeting.ShowSkillRadius(character.Attack.range);
        hasAttacked = false;
    }

    public override void Leave()
    {
        playerTargeting.ResetRadiusCells();
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
                    if ((target == null || target != temp) && playerTargeting.TargetCell(temp))
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
