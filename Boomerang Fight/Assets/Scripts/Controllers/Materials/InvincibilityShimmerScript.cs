using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InvincibilityShimmerScript : MonoBehaviour
{

    const string EFFECTBOOL = "_EFFECTBOOL";
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] Material shimmerMaterial;
    private void Start()
    {
        //making a new material so that activating the material properties will only paply on the new clone material and not everyone
        shimmerMaterial = new Material(shimmerMaterial);
        skinnedMeshRenderer.SetMaterials(new List<Material> { skinnedMeshRenderer.materials[0], shimmerMaterial } );
    }
    public void ActivateShimmer()
    {
        print("activate shimmer");
        shimmerMaterial.EnableKeyword(EFFECTBOOL);
    }

    public void DeactivateShimmer()
    {
        print("deactivate shimmer");
        shimmerMaterial.DisableKeyword(EFFECTBOOL);
    }
}
