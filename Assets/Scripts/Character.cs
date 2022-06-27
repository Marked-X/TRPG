using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    Animator animator = null;

    private GridCell position = null;
    private Vector3 direction = Vector3.down;

    public void SetPosition(GridCell cell)
    {
        position = cell;
        transform.position = new Vector3(cell.gameObject.transform.position.x, cell.gameObject.transform.position.y, -1);
    }

    public GridCell GetPosition()
    {
        return position;
    }

    public IEnumerator Move(Vector3 targetPos)
    {
        animator.SetBool("Moving", true);
        direction = targetPos - transform.position;
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 4f * Time.deltaTime);
            
            yield return null;
        }

        transform.position = targetPos;
        animator.SetBool("Moving", false);
    }
}
