using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHPBarMaterial : MonoBehaviour
{
    [SerializeField] private Renderer healthBarRenderer;

    // Assuming that the material has a "_Health" property
    const string healthProperty = "_Value";

    // Function to update the health bar material
    public void UpdateOnHealthChangedEvent(float newHealth,float maxHealth)
    {
        // Get the material of the renderer
        Material healthBarMaterial = healthBarRenderer.material;
        // Clamp the new health value between 0 and 1
        float clampedHealth = Mathf.Clamp01(newHealth/maxHealth);
        // Set the health value in the material
        healthBarMaterial.SetFloat(healthProperty, clampedHealth);
    }
}
