﻿using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public static class Onboarding
{
    public static bool IsOnboardingFirstBattleStarted;
    public static bool IsOnboardingSecondBattleStarted;
    public static bool IsInOnboarding { get; set; }

    public static async UniTask<bool> TryBattleTutorial(int index)
    {
        if (IsOnboardingFirstBattleStarted)
        {
            return false;
        }

        IsInOnboarding = true;
        IsOnboardingFirstBattleStarted = true;

        await Root.Instance.DialogueSequenceWrapper.StartSequence(GameResources.VisualNovel.Battle_Tutor_01, null,
            true);
        await UniTask.WaitForSeconds(0.1f);

        IsInOnboarding = false;
        return true;
    }

    public static async UniTask<bool> TryBattleSecondTutorial()
    {
        if (IsOnboardingSecondBattleStarted)
        {
            return false;
        }

        IsInOnboarding = true;
        IsOnboardingSecondBattleStarted = true;

        await Root.Instance.DialogueSequenceWrapper.StartSequence(GameResources.VisualNovel.Battle_Tutor_02, null, true);
        await UniTask.WaitForSeconds(0.1f);

        IsInOnboarding = false;
        return false;
    }
}