using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Spawner : EditorWindow
{
    string posX = "0";
    string posY = "0";
    Object objectToSpawn;
    Encounter encounter;

    [MenuItem("Window/Spawner")]
    public static void ShowWindow()
    {
        GetWindow<Spawner>("Spawner");
    }

    private void OnGUI()
    {
        posX = EditorGUILayout.TextField("posX", posX);
        posY = EditorGUILayout.TextField("posY", posY);

        objectToSpawn = EditorGUILayout.ObjectField(objectToSpawn, typeof(GameObject), true);


        if (GUILayout.Button("AddToEncounter"))
        {
            AddToEncounterFunction();
        }
        if (GUILayout.Button("StartEncounter"))
        {
            StartEncounterFunction();
        }

    }

    private void StartEncounterFunction()
    {
        GameController.Instance.StartAnEncounter(encounter);
    }

    private void AddToEncounterFunction()
    {
        if (encounter.encounterSize == 0)
        {
            encounter = new();
            encounter.enemies = new();
        }
        GameObject newEnemy = (GameObject)Instantiate(objectToSpawn);
        newEnemy.GetComponent<Character>().SetPosition(GameController.Instance.gridCells[int.Parse(posX), int.Parse(posY)]);
        encounter.encounterSize++;
        encounter.enemies.Add(newEnemy);
    }
}
