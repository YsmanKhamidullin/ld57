using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool IsNearHeart()
    {
        var heart = Root.Instance.PlayerHeart;
        var heartPos = heart.transform.position;
        var distanceToHeart = Vector2.Distance(heartPos, transform.position);
        if (distanceToHeart < 80f)
        {
            return true;
        }

        return false;
    }
    public bool IsTriedDealDamage { get; set; }
}