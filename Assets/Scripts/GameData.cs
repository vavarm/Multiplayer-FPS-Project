using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    [SerializeField]
    public Weapon[] weapons;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one GameData in scene!");
            return;
        }

        Instance = this;
    }

    public Weapon GetWeapon(string _name)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.name == _name)
                return weapon;
        }

        return null;
    }

    public int GetWeaponIndex(string _name)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].name == _name)
                return i;
        }

        return -1;
    }
}
