using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    public List<BattleCell> Cells;

    private void OnValidate()
    {
        Cells = GetComponentsInChildren<BattleCell>().ToList();
    }
}