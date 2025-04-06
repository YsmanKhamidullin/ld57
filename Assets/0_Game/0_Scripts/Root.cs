﻿using System;
using UnityEngine;

public class Root : MonoBehaviour
{
    [field: SerializeField]
    public ServiceFight ServiceFight { get; private set; }

    [field: SerializeField]
    public ServiceLadder ServiceLadder { get; private set; }

    [field: SerializeField]
    public BattleCardsContainer BattleCardsContainer { get; private set; }
    [field: SerializeField]
    public DialogueSequenceWrapper DialogueSequenceWrapper { get; private set; }

    [field: SerializeField]
    public ServiceInput ServiceInput { get; private set; }

    [field: SerializeField]
    public ServiceUi ServiceUi { get; private set; }

    [field: SerializeField]
    public ServiceCards ServiceCards { get; private set; }

    [field: SerializeField]
    public Player Player { get; private set; }
    [field: SerializeField]
    public EnemyVisual EnemyVisual { get; private set; }

    [field: SerializeField]
    public PlayerHeart PlayerHeart { get; private set; }

    [field: SerializeField]
    public Mind Mind { get; private set; }

    public static Root Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }
}