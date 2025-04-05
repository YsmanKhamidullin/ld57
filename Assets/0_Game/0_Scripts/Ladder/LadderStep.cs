using System.Collections.Generic;
using UnityEngine;

public class LadderStep : MonoBehaviour
{
    public enum StepTypes
    {
        Basic,
        GameStart,
        NPC,
        EachFive,
        BossFight,
    }

    public StepTypes StepType;
    public List<Enemy> Enemies;
    public NPC Npc;

    public Enemy GetEnemyAndRemove()
    {
        var random = Enemies[0];
        Enemies.Remove(random);
        return Instantiate(random);
    }

    public bool HaveEnemies()
    {
        return Enemies.Count > 0;
    }
}