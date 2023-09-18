using UnityEngine;
using FishNet.Object;
using VaVarm.Utils;
using System.Collections;
using FishNet.Object.Synchronizing;
using FishNet.Object.Synchronizing.Internal;

public class WeaponManager : NetworkBehaviour
{
    private GameData gameDataInstance;

    [SerializeField]
    private Weapon primaryWeapon;

    private Weapon currentWeapon;
    [SyncVar(OnChange = nameof(EquipWeapon))]
    private int currentWeaponIndex = -1;
    private WeaponGraphics currentGraphics;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private int currentMagazineSize;
    public int CurrentMagazineSize => currentMagazineSize;

    public bool isReloading { get; private set; } = false;

    private void Start()
    {
        gameDataInstance = GameData.Instance;
        if(primaryWeapon == null)
        {
            Debug.Log("No primary weapon assigned to WeaponManager", this);
            return;
        }
        int _index = gameDataInstance.GetWeaponIndex(primaryWeapon.weaponName);
        Debug.Log("Starting Weapon index: " + _index);
        if(_index != -1)
        {
            SetCurrentWeaponIndex(_index);
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    public void SetCurrentWeaponIndex(int _index)
    {
        Debug.Log("Call Equip Weapon with index: " + _index);
        EquipWeapon(currentWeaponIndex, _index, false);
        Debug.Log("Set current weapon index: " + _index);
        currentWeaponIndex = _index;
    }

    public void EquipWeapon(int _oldIndex, int _newIndex, bool asServer)
    {

        Debug.Log("EquipWeapon: " + _oldIndex + " " + _newIndex);

        Weapon _weapon = gameDataInstance.weapons[_newIndex];

        if(_weapon == null) return;

        // update current weapon
        currentWeapon = _weapon;

        // set current magazine size
        currentMagazineSize = currentWeapon.magazineSize;

        // instantiate weapon graphics
        GameObject weaponIns = Instantiate(currentWeapon.prefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);

        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();

        if (currentWeapon == null)
            Debug.LogError("No WeaponGraphics component on the weapon object: " + weaponIns.name);

        // set specific layer to weapon graphics
        if (base.IsOwner)
        {
            LayerUtils.SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }

    public IEnumerator Reload()
    {
        if (isReloading) yield break;
        isReloading = true;
        PlayReloadAnimation();
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentMagazineSize = currentWeapon.magazineSize;
        isReloading = false;
    }

    private void PlayReloadAnimation()
    {
        Animator anim = currentGraphics.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }
}
