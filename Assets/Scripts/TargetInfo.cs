using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TargetInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject targetInfoObject = null;
    [SerializeField]
    private GameObject targetSprite = null;
    [SerializeField]
    private Sprite defaultTargetSprite = null;
    [SerializeField]
    private TextMeshProUGUI targetName = null;
    [SerializeField]
    private TextMeshProUGUI targetHealth = null;

    public void TargetEnter(GridCell cell)
    {
        if (!cell.Occupator.TryGetComponent<Character>(out Character character))
        {
            Debug.LogError("Target info can't get character component in this cell!");
        }

        targetSprite.GetComponent<Image>().sprite = character.DefaultSprite;
        targetName.text = character.Name;
        targetHealth.text = "Health:\n" + character.CurrentHealth;
        targetInfoObject.SetActive(true);
    }

    public void TargetLeave()
    {
        targetInfoObject.SetActive(false);
    }
}
