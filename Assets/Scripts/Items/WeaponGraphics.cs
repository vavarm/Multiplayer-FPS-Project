using UnityEngine;

public class WeaponGraphics : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private GameObject hitEffectPrefab;
    public GameObject HitEffectPrefab => hitEffectPrefab;

    public void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }
}
