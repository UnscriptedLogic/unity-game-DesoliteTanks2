using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileData
{
    public string tankID;
    public float damage;
    public float bulletSpeed;

    public ProjectileData(string tankID, float damage, float bulletSpeed)
    {
        this.tankID = tankID;
        this.damage = damage;
        this.bulletSpeed = bulletSpeed;
    }
}