﻿using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour, IWill
{
    [SerializeField]
    private int _maxWill;

    private int _currentWill;

    public int CurrentWill
    {
        get => _currentWill;
        set => _currentWill = Math.Clamp(value, 0, _maxWill);
    }

    public int MaxWill
    {
        get => _maxWill;
        set => _maxWill = value;
    }

    private void Start()
    {
        _currentWill = _maxWill;
    }

    public async UniTask Move(LadderStep ladderStep)
    {
        var stepPos = ladderStep.transform.position;
        stepPos.y += 1f;
        transform.DOMove(stepPos, 0.25f);
        await UniTask.WaitForSeconds(0.25f);
    }
}