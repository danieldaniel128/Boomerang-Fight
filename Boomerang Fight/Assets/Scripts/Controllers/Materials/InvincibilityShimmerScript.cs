using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InvincibilityShimmerScript : MonoBehaviour
{
    const string EFFECTBOOL = "_EFFECTBOOL";
    [SerializeField] Material shimmerMaterial;
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
