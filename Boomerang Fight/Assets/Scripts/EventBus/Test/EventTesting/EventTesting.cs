using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTesting : MonoBehaviour
{
    EventBinding<PlayerEvent> playerEventBinding;
    private void OnEnable()
    {
        EventBus<PlayerEvent>.Register(playerEventBinding);
    }
    private void OnDisable()
    {
        EventBus<PlayerEvent>.Deregister(playerEventBinding);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
            EventBus<PlayerEvent>.Raise(new PlayerEvent { health = 43,mana = 34} );
    }
    void HandlePlayerEvent(PlayerEvent playerEvent)
    {
        Debug.Log("handled Event");
    }
}
