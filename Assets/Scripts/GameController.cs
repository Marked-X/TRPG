using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;
    
    public Movement movement = null;
    public GameObject grid = null;
    public GameObject player = null;

    public GameObject gridcellPrefab = null;

    public int gridWidth = 14;
    public int gridHeight = 8;

    public GridCell[,] gridCells = null;

    private GameObject selectedObject = null;
    private GridCell destination = null;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gridCells = new GridCell[gridWidth, gridHeight];

        int i = 0, j = 0;

        foreach(Transform cell in grid.transform)
        {
            cell.gameObject.GetComponent<GridCell>().Position = new Vector2(i, j);

            gridCells[i, j] = cell.GetComponent<GridCell>();
            if (++i == gridWidth)
            {
                i = 0;
                j++;
            }
        }

        player.GetComponent<Character>().SetPosition(gridCells[4, 3]);

        movement.Ready();
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject;
                GridCell temp = null;
                if (selectedObject.TryGetComponent<GridCell>(out temp))
                {
                    if (destination == null || destination != temp)
                    {
                        destination = temp;
                        movement.Astar(player.GetComponent<Character>().GetPosition(), destination);
                    }
                    else if (destination == temp)
                    {
                        StartCoroutine(movement.MoveCharacter(player.GetComponent<Character>()));
                    }
                }
            }
        }
    }
}
