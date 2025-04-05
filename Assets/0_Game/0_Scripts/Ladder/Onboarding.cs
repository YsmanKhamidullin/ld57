using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public static class Onboarding
{
    public static bool isOnboardingFirstBattleStarted;
    public static bool IsInOnboarding { get; set; }

    public static async UniTask<bool> TryBattleTutorial(int index)
    {
        if (isOnboardingFirstBattleStarted)
        {
            return false;
        }

        IsInOnboarding = true;

        isOnboardingFirstBattleStarted = true;

        await Root.Instance.DialogueSequenceWrapper.StartSequence(GameResources.VisualNovel.Battle_Tutor_01, null,
            true);
        await UniTask.WaitForSeconds(0.1f);
        IsInOnboarding = false;
        return true;
    }
}