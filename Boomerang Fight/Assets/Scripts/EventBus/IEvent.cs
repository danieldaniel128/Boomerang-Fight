using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent { }
public struct PlayerEvent : IEvent
{
    public int health;
    public int mana;
}
