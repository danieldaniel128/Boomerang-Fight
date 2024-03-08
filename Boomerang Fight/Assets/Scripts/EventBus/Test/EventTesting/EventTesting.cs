using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTesting : MonoBehaviour
{
    EventBinding<OnPlayerHealthChangedEvent> playerEventBinding;
    private void OnEnable()
    {
        playerEventBinding = new EventBinding<OnPlayerHealthChangedEvent>(HandlePlayerEvent);
        playerEventBinding.Add(DanielIsGodPlayerEvent);
        EventBus<OnPlayerHealthChangedEvent>.Register(playerEventBinding);
    }
    private void OnDisable()
    {
        EventBus<OnPlayerHealthChangedEvent>.Deregister(playerEventBinding);
    }

    private void Update()
    {
    }
    void HandlePlayerEvent(OnPlayerHealthChangedEvent playerEvent)
    {
        Debug.Log("handled Event");
    }
    void DanielIsGodPlayerEvent(OnPlayerHealthChangedEvent playerEvent)
    {
        Debug.Log("daniel is god");
    }
}
