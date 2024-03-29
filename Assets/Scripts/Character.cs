using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    Animator animator = null;

    [field: SerializeField]
    public SkillSO Attack { get; private set; }

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public Sprite DefaultSprite { get; private set; }

    private int maxMovementPoints = 4;
    [field: SerializeField]
    public int CurrentMovementPoints { get; set; } = 4;

    public int CurrentHealth 
    { 
        get 
        {
            return currentHealth;
        } 
        private set
        {
            currentHealth = value;
        }
    }

    public delegate void MovementEndedEventHandler();

    public event MovementEndedEventHandler OnMovementEnded;
    
    private int maxHealth = 3;
    private int currentHealth = 3;
    private GridCell position = null;
    private Vector3 direction = Vector3.down;

    public void SetPosition(GridCell cell)
    {
        position = cell;
        cell.IsOccupied = true;
        cell.Occupator = this;
        transform.position = new Vector3(cell.gameObject.transform.position.x, cell.gameObject.transform.position.y, -1);
    }

    public GridCell GetPosition()
    {
        return position;
    }

    public void GetAttacked(SkillSO skill)
    {
        currentHealth -= skill.damage;
        if(currentHealth <= 0)
        {
            Death();
        }
    }

    public void RefreshMovementPoints()
    {
        CurrentMovementPoints = maxMovementPoints;
    }

    private void Death()
    {
        //death animation
        position.Occupator = null;
        GameObject.Destroy(gameObject);
    }

    public void BeginMoving(Stack<GridCell> path)
    {
        StartCoroutine(MoveAlongPath(path));
    }

    private IEnumerator MoveAlongPath(Stack<GridCell> path)
    {
        if (path == null)
        {
            Debug.LogWarning("No path");
            StopCoroutine(MoveAlongPath(path));
        }

        while (path.Count > 0)
        {
            GridCell cell = path.Pop();
            CurrentMovementPoints--;
            yield return StartCoroutine(Move(cell));
            cell.Reset();
        }

        OnMovementEnded.Invoke();
    }

    private IEnumerator Move(GridCell targetCell)
    {
        Vector3 targetPos = targetCell.transform.position;
        position.IsOccupied = false;
        position.Occupator = null;

        animator.SetBool("Moving", true);
        direction = targetPos - transform.position;
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 4f * Time.deltaTime);
            
            yield return null;
        }

        SetPosition(targetCell);
        animator.SetBool("Moving", false);
    }
}
