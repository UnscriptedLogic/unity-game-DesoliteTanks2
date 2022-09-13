using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileData
{
    public string tankID;
    public string team;
    public float damage;
    public float bulletSpeed;

    public ProjectileData(string tankID, string team, float damage, float bulletSpeed)
    {
        this.tankID = tankID;
        this.team = team;
        this.damage = damage;
        this.bulletSpeed = bulletSpeed;
    }
}