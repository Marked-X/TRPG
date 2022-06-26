using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private GridCell position = null;
    private Vector2 direction = Vector2.down;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(GridCell cell)
    {
        position = cell;
        transform.position = new Vector3(cell.gameObject.transform.position.x, cell.gameObject.transform.position.y, -1);
    }

    public GridCell GetPosition()
    {
        return position;
    }
}
