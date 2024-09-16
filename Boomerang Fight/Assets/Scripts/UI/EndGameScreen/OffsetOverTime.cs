using UnityEngine;

public class OffsetOverTime : MonoBehaviour
{
    public Material material;   // Assign your material here
    public float speedX = 0.1f; // Speed of X offset
    public float speedY = 0.1f; // Speed of Y offset

    private Vector2 offset = Vector2.zero; // Store the offset value

    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        // Calculate the new offset based on time
        offset.x += Time.deltaTime * speedX;
        offset.y += Time.deltaTime * speedY;

        // Apply the new offset to the material
        material.SetTextureOffset("_MainTex", offset);
    }
}