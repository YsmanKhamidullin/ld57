using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IWill
{
    public string EnemyName;
    
    private int _currentWill;

    [SerializeField]
    private  int _maxWill;

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
}