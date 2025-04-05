using System.Collections.Generic;
using UnityEngine;

public class KiddoEnemy : Enemy
{
    public override List<EnemyAttack.AttackPattern> AttackPatterns => defaultPatterns;
    public override Vector2Int AttacksCount => attacksCount;

    [SerializeField]
    private Vector2Int attacksCount = new Vector2Int(1, 1);

    private List<EnemyAttack.AttackPattern> defaultPatterns = new()
    {
        new EnemyAttack.AttackPattern
        {
            patternName = "Third Row Horizontal",
            targetPositions = new List<Vector2Int>
            {
                new Vector2Int(0, 2),
                new Vector2Int(1, 2),
                new Vector2Int(2, 2)
            }
        },
    };
}