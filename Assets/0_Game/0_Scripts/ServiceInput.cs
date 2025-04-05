using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ServiceInput : MonoBehaviour
{

    public static async UniTask AwaitClick()
    {
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
    }
}