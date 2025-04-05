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
}