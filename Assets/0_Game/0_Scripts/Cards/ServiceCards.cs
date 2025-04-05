﻿using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ServiceCards : MonoBehaviour
{
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

    private async UniTask UseCardByType(CardTypes type)
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
                    l.IncrementStep();
                    await TryInteractByStep(ladderStep);
                    var n = Root.Instance.ServiceLadder.GetNextStep();
                    await TryInteractByStep(n);
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
                    l.DecrementStep();
                }
                else
                {
                    Debug.Log("No prev step");
                }

                break;
            case CardTypes.Dream:
                break;
            case CardTypes.Struggle:
                break;
            case CardTypes.Talk:
                Root.Instance.ServiceFight.UseTalk();
                break;
            case CardTypes.Listen:
                Root.Instance.ServiceFight.UseListen();
                break;
            case CardTypes.Flee:
                Root.Instance.ServiceFight.UseFlee();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        IsUsingCard = false;
    }

    private async UniTask TryInteractByStep(LadderStep step)
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
            case LadderStep.StepTypes.EachFive:
                break;
            case LadderStep.StepTypes.BossFight:
                await Root.Instance.ServiceFight.ForceStartFight(step);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}