using UnityEngine;

public class VFXTransition : MonoBehaviour
{
    public VFXTypeEnum VFXType;
    public bool IsTriggerVFX;
}
public enum VFXTypeEnum
{
    Walking,
    Slap,
    Recall,
    Impact,
    HittingEnemy,
    BoomerangTrail,
    BoomerangSpon,
    BoomerangBack,
    Arrow
}
