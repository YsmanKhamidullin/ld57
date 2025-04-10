﻿using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class ServiceLadder : MonoBehaviour
{
    public int CurrentHeight => Steps[_currentStepIndex].Height;
    public List<LadderStep> Steps;
    public int _currentStepIndex = 0;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UpdateStepsList();
        }
#endif
    }

    [Button]
    private void UpdateStepsList()
    {
        Steps = GetComponentsInChildren<LadderStep>().ToList();
        Steps.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
    }


    public void EnableLadderVisual()
    {
        gameObject.SetActive(true);
    }

    public void DisableLadderVisual()
    {
        gameObject.SetActive(false);
    }

    public bool IsNextStepAvailable()
    {
        return _currentStepIndex + 1 < Steps.Count;
    }

    public bool IsPreviousStepAvailable()
    {
        return _currentStepIndex - 1 >= 0;
    }

    public void IncrementStep()
    {
        _currentStepIndex++;
    }

    public void DecrementStep()
    {
        _currentStepIndex--;
    }

    public LadderStep GetNextStep()
    {
        return Steps[_currentStepIndex + 1];
    }

    public LadderStep GetPreviousStep()
    {
        return Steps[_currentStepIndex - 1];
    }
}