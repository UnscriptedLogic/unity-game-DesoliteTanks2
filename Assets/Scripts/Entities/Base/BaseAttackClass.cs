using Core;
using System.Collections;
using UnityEngine;

namespace Entities
{
    public class BaseAttackClass : MonoBehaviour, IAttackable
    {
        [Header("Stats")]
        [SerializeField] protected float damage;
        [SerializeField] protected float bulletSpeed;
        [SerializeField] protected int piercing = 1;
        [SerializeField] protected float lifetime;

        [Header("Components")]
        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected Transform bulletSpawn;
        [SerializeField] protected VFXSettings shootVFX; 
        [SerializeField] protected Entity baseClass;
        
        public float Damage { get => damage; set { damage = value; } }
        public float BulletSpeed { get => bulletSpeed; set { bulletSpeed = value; } }
        public int Piercing { get => piercing; set { piercing = value; } }
        public float LifeTime { get => lifetime; set { lifetime = value; } }

        public virtual void Attack(IDamageable target)
        {
            
        }

        protected Projectile CreateBullet()
        {
            GameObject bullet = EntityManager.emInstance.CreateEntity(bulletPrefab, baseClass.Team);
            ProjectileData projectileData = new ProjectileData(baseClass.EntityID, baseClass.Team, damage, bulletSpeed, piercing, lifetime);
            bullet.GetComponent<Projectile>().Initialize(projectileData);
            bullet.transform.SetPositionAndRotation(bulletSpawn.position, bulletSpawn.rotation);
            bullet.transform.SetParent(null);
            bullet.SetActive(true);

            shootVFX.PlayVFX(bulletSpawn.position, bulletSpawn.rotation);
            return bullet.GetComponent<Projectile>();
        }
    }
}