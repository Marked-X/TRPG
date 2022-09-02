using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSkill", menuName = "Skill")]
public class SkillSO : ScriptableObject
{
    public int damage = 0;
    public int range = 1;
    public Target target = Target.Enemy;
    public int cost = 0;

    public enum Target
    {
        Default,
        Self,
        Allay,
        Enemy,
        AOE
    }
}
