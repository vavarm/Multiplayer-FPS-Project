using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{

    private bool showSoundSettings = false;

    public override void OnInspectorGUI()
    {
        Weapon weapon = (Weapon)target;

        EditorUtility.SetDirty(weapon);

        weapon.weaponName = EditorGUILayout.TextField("Weapon Name", weapon.weaponName);

        weapon.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", weapon.prefab, typeof(GameObject), false);

        weapon.damage = EditorGUILayout.FloatField("Damage", weapon.damage);

        weapon.range = EditorGUILayout.FloatField("Range", weapon.range);

        weapon.fireMode = (FireMode)EditorGUILayout.EnumPopup("Fire Mode", weapon.fireMode);

        EditorGUI.BeginDisabledGroup(weapon.fireMode != FireMode.Auto);
        weapon.fireRate = EditorGUILayout.FloatField("Fire Rate", weapon.fireRate);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.HelpBox("Fire Rate: number of shoots per second", MessageType.Info);

        if (weapon.fireMode == FireMode.Auto && weapon.fireRate <= 0f)
        {
            EditorGUILayout.HelpBox("Fire Rate must be greater than 0 for Auto Fire Mode", MessageType.Warning);
        }

        if (weapon.fireMode == FireMode.SemiAuto)
        {
            weapon.fireRate = 0f;
        }

        weapon.magazineSize = EditorGUILayout.IntField("Magazine Size", weapon.magazineSize);

        weapon.reloadTime = EditorGUILayout.FloatField("Reload Time", weapon.reloadTime);

        showSoundSettings = EditorGUILayout.Foldout(showSoundSettings, "Sound Settings");
        if (showSoundSettings)
        {
            weapon.shootSound = (AudioClip)EditorGUILayout.ObjectField("Shoot Sound", weapon.shootSound, typeof(AudioClip), false);
            weapon.reloadSound = (AudioClip)EditorGUILayout.ObjectField("Reload Sound", weapon.reloadSound, typeof(AudioClip), false);
        }


    }
}
