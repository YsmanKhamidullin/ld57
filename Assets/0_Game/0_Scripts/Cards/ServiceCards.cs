using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceCards : MonoBehaviour
{
    public Card ForwardCard;
    public Card BackWardCard;
    public Card DreamCard;
    public Card TalkCard;
    public Card ListenCard;
    public Card FleeCard;
    
    public bool IsUsingCard;

    public void TryUseCard(Card card)
    {
        var dropZone = ZoneUtil.Check();
        switch (dropZone)
        {
            case ZoneUtil.ZoneType.Hand:
                card.ToDefaultPos();
                break;
            case ZoneUtil.ZoneType.Center:
                IsUsingCard = true;
                card.ReAppear(() => _ = UseCardByType(card.CardType));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async UniTask UseCardByType(CardTypes type)
    {
        var l = Root.Instance.ServiceLadder;
        Debug.Log($"Using: {type}");
        switch (type)
        {
            case CardTypes.None:
                break;
            case CardTypes.Forward:
                if (l.IsNextStepAvailable())
                {
                    LadderStep ladderStep = Root.Instance.ServiceLadder.GetNextStep();
                    await Root.Instance.Player.Move(ladderStep);
                    ladderStep.CallOnStep();
                    l.IncrementStep();
                    await TryInteractByCurrentStep(ladderStep);
                    if (Root.Instance.ServiceFight.InFight == false)
                    {
                        var n = Root.Instance.ServiceLadder.GetNextStep();
                        await TryInteractByNextStep(n);
                    }
                }
                else
                {
                    Debug.Log("No next step");
                }

                break;
            case CardTypes.Backward:
                if (l.IsPreviousStepAvailable())
                {
                    LadderStep ladderStep = Root.Instance.ServiceLadder.GetPreviousStep();
                    await Root.Instance.Player.Move(ladderStep);
                    ladderStep.CallOnStep();
                    l.DecrementStep();
                    await TryInteractByCurrentStep(ladderStep);
                    if (Root.Instance.ServiceFight.InFight == false)
                    {
                        var n = Root.Instance.ServiceLadder.GetNextStep();
                        await TryInteractByNextStep(n);
                    }
                }
                else
                {
                    Debug.Log("No prev step");
                }

                break;
            case CardTypes.Dream:
                await UseDreamCard();
                break;
            case CardTypes.Struggle:
                break;
            case CardTypes.Talk:
                await Root.Instance.ServiceFight.UseTalk();
                break;
            case CardTypes.Listen:
                await Root.Instance.ServiceFight.UseListen();
                break;
            case CardTypes.Flee:
                await Root.Instance.ServiceFight.UseFlee();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        IsUsingCard = false;
    }

    private async UniTask UseDreamCard()
    {
        await TryHealIfHaveMind();
    }

    private async UniTask TryHealIfHaveMind()
    {
        bool isPositive = Mind.MindValue > 0;
        if (isPositive)
        {
            Root.Instance.Player.CurrentWill++;
            await Root.Instance.Mind.Decrement();
        }
        else if (Mind.MindValue < 0)
        {
            Root.Instance.Player.CurrentWill++;
            await Root.Instance.Mind.Increment();
        }
        else
        {
            await Root.Instance.Mind.ImpactZero();
        }
    }

    public async UniTask TryInteractByNextStep(LadderStep step)
    {
        switch (step.StepType)
        {
            case LadderStep.StepTypes.Basic:
                break;
            case LadderStep.StepTypes.GameStart:
                break;
            case LadderStep.StepTypes.NPC:
                await step.Npc.Interact();
                break;
            case LadderStep.StepTypes.EnterSand:
                break;
            case LadderStep.StepTypes.ExitSand:
                break;
            case LadderStep.StepTypes.BossFight:

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async UniTask TryInteractByCurrentStep(LadderStep step)
    {
        switch (step.StepType)
        {
            case LadderStep.StepTypes.Basic:
                break;
            case LadderStep.StepTypes.GameStart:
                break;
            case LadderStep.StepTypes.NPC:
                break;
            case LadderStep.StepTypes.EnterSand:
                Root.TryLoadSand();
                SceneManager.LoadScene(2);
                break;
            case LadderStep.StepTypes.ExitSand:
                Root.TryLoadGame();
                SceneManager.LoadScene(1);
                break;
            case LadderStep.StepTypes.BossFight:
                if (step.HaveEnemies())
                {
                    await Root.Instance.ServiceFight.ForceStartFight(step);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}