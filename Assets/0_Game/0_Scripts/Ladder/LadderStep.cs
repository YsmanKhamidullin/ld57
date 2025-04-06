using System.Collections.Generic;
using System.Linq;
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

    public Enemy GetEnemy()
    {
        var random = Enemies[0];
        return Root.Instance.EnemyVisual.SpawnEnemy(random);
    }

    public Enemy GetEnemyAndRemove()
    {
        var random = Enemies[0];
        Enemies.Remove(random);
        return Root.Instance.EnemyVisual.SpawnEnemy(random);
    }

    public bool HaveEnemies()
    {
        return Enemies.Count > 0;
    }

    public void TryRemove(Enemy enemy)
    {
        var e = Enemies.FirstOrDefault(e => e.EnemyName == enemy.EnemyName);
        if (e != null)
        {
            Enemies.Remove(e);
        }
    }
}