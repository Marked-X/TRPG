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

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gridCells = new GridCell[gridWidth, gridHeight];

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                GameObject temp = Instantiate(gridcellPrefab, grid.transform);
                temp.transform.localPosition = new Vector2(i, j);
                temp.GetComponent<GridCell>().Position = new Vector2(i, j);

                gridCells[i, j] = temp.GetComponent<GridCell>();
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
                GridCell destination = null;
                if(selectedObject.TryGetComponent<GridCell>(out destination))
                {
                    movement.Astar(player.GetComponent<Character>().GetPosition(), destination);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject;
                GridCell cell = null;
                if (selectedObject.TryGetComponent<GridCell>(out cell))
                {
                    cell.occupied = true;
                    cell.TurnBlack();
                }
            }
        }
    }
}
