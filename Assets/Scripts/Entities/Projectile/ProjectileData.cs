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
    public int piercing;
    public float lifeTime;

    public ProjectileData(string tankID, string team, float damage, float bulletSpeed, int piercing, float lifeTime)
    {
        this.tankID = tankID;
        this.team = team;
        this.damage = damage;
        this.bulletSpeed = bulletSpeed;
        this.piercing = piercing;
        this.lifeTime = lifeTime;
    }
}