using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent { }
public struct OnPlayerHealthChangedEvent : IEvent
{
    public float newHealth;
    public float maxHealth;
}
public struct OnPlayerDeath : IEvent { }
public struct OnPlayerMovement : IEvent { }
public struct OnPlayerRangeAttack : IEvent { }
