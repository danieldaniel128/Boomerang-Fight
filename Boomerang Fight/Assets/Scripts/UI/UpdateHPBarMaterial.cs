using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHPBarMaterial : MonoBehaviourPun
{
    [SerializeField] private Renderer healthBarRenderer;
    private EventBinding<OnPlayerHealthChangedEvent> HealthChangedEventBinding;
    // Assuming that the material has a "_Health" property
    const string healthProperty = "_Value";

    private void OnEnable()
    {
        HealthChangedEventBinding = new EventBinding<OnPlayerHealthChangedEvent>(UpdateOnHealthChangedEvent);
        if(photonView.IsMine)
        EventBus<OnPlayerHealthChangedEvent>.Register(HealthChangedEventBinding);
    }
    private void OnDisable()
    {
        if (photonView.IsMine)
            EventBus<OnPlayerHealthChangedEvent>.Deregister(HealthChangedEventBinding);
    }

    // Function to update the health bar material
    public void UpdateOnHealthChangedEvent(OnPlayerHealthChangedEvent healthChangedEvent)
    {
        // Get the material of the renderer
        Material healthBarMaterial = healthBarRenderer.material;
        // Clamp the new health value between 0 and 1
        float clampedHealth = Mathf.Clamp01(healthChangedEvent.newHealth/healthChangedEvent.maxHealth);
        // Set the health value in the material
        healthBarMaterial.SetFloat(healthProperty, clampedHealth);
    }
}
