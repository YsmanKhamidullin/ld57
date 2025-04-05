using System.Collections.Generic;
using UnityEngine;

public class HorizontalAndVerticalEnemy : Enemy
{
    public override List<EnemyAttack.AttackPattern> AttackPatterns => defaultPatterns;
    public override Vector2Int AttacksCount => attacksCount;

    [SerializeField]
    private Vector2Int attacksCount = new Vector2Int(5, 10);

    private List<EnemyAttack.AttackPattern> defaultPatterns = new()
    {
        new EnemyAttack.AttackPattern
        {
            patternName = "First Row Horizontal",
            targetPositions = new List<Vector2Int>
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(2, 0)
            }
        },
        new EnemyAttack.AttackPattern
        {
            patternName = "Second Row Horizontal",
            targetPositions = new List<Vector2Int>
            {
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(2, 1)
            }
        },
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
        new EnemyAttack.AttackPattern
        {
            patternName = "First Row Vertical",
            targetPositions = new List<Vector2Int>
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2)
            }
        },
        new EnemyAttack.AttackPattern
        {
            patternName = "Second Row Vertical",
            targetPositions = new List<Vector2Int>
            {
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(1, 2)
            }
        },
        new EnemyAttack.AttackPattern
        {
            patternName = "Third Row Vertical",
            targetPositions = new List<Vector2Int>
            {
                new Vector2Int(2, 0),
                new Vector2Int(2, 1),
                new Vector2Int(2, 2)
            }
        },
    };
}