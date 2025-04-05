using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimationsUtil
{
    public static async UniTask FadeIn(Image image, float time)
    {
        image.Alpha(0f);
        await image.DOFade(1f, Timings.Get(time)).ToUniTask();
    }


    public static async UniTask FadeOut(Image image, float time)
    {
        image.Alpha(1f);
        await image.DOFade(0f, Timings.Get(time)).ToUniTask();
    }
}