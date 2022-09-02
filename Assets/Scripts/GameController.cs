using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;
    
    public GameObject grid = null;
    public GameObject player = null;
    public TextMeshProUGUI movementPointsText = null;
    public TextMeshProUGUI turnNumberText = null;
    public TargetInfo targetInfo = null;

    public GameObject gridcellPrefab = null;

    public int gridWidth = 14;
    public int gridHeight = 8;

    public GridCell[,] gridCells = null;

    public Pathfinding pathfinding = new();

    private State currentState = null;
    private static StateMovement moving = new StateMovement();
    private static StateAttack attack = new StateAttack();
    private static StateIdle idle = new StateIdle();
    private GameObject[] currentCharacters = null;

    private int partySize = 1;

    private int currentCharacterIndex = 0;
    private int turnCount = 1;

    
    private void Awake()
    {
        Instance = this;
        currentState = idle;
        gridCells = new GridCell[gridWidth, gridHeight];

        int i = 0, j = 0;

        foreach (Transform cell in grid.transform)
        {
            cell.GetComponent<GridCell>().Position = new Vector2(i, j);

            gridCells[i, j] = cell.GetComponent<GridCell>();
            if (++i == gridWidth)
            {
                i = 0;
                j++;
            }
        }
    }

    void Start()
    {
        
        pathfinding.Ready();

        player.GetComponent<Character>().SetPosition(gridCells[4, 3]);

        currentCharacters = new GameObject[partySize];
        currentCharacters[0] = player;

        currentState.Enter(player);
    }

    void Update()
    {
        currentState.Update();
    }

    private void NextTurn()
    {
        currentCharacterIndex++;
        if (currentCharacters.Length <= currentCharacterIndex)
        {
            turnCount++;
            turnNumberText.text = "Turn: " + turnCount;
            currentCharacterIndex = 0;
        }
        currentState.Leave();
        currentState = idle;
        currentState.Enter(currentCharacters[currentCharacterIndex]);
    }

    public void MoveActionButtonPressed()
    {
        currentState.Leave();
        currentState = moving;
        currentState.Enter(currentCharacters[currentCharacterIndex]);
    }

    public void AttackButtonActionPressed()
    {
        currentState.Leave();
        currentState = attack;
        currentState.Enter(currentCharacters[currentCharacterIndex]);
    }

    public void NextTurnButton()
    {
        NextTurn();
    }

    public void StartAnEncounter(Encounter encounter)
    {
        currentCharacters = new GameObject[partySize + encounter.encounterSize];
        currentCharacters[0] = player;
        int i = 1;
        foreach(GameObject enemy in encounter.enemies)
        {
            currentCharacters[i++] = enemy;
        }
    }
}
