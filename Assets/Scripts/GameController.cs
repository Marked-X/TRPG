using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;
    
    public GameObject grid = null;
    public GameObject player = null;

    public GameObject gridcellPrefab = null;

    public int gridWidth = 14;
    public int gridHeight = 8;

    public GridCell[,] gridCells = null;


    private State currentState = null;
    private static StateMovement moving = new StateMovement();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentState = moving;
        currentState.Enter();
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
        player.GetComponent<PlayerMovement>().Ready();
    }

    void Update()
    {
        currentState.Update();
    }
}
