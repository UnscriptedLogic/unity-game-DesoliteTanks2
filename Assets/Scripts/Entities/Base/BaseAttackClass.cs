using Core;
using System.Collections;
using UnityEngine;

namespace Entities
{
    public class BaseAttackClass : MonoBehaviour, IAttackable
    {
        [SerializeField] protected float damage;
        [SerializeField] protected float bulletSpeed;
        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected Transform bulletSpawn;

        [Header("Components")]
        [SerializeField] protected BaseManagerClass baseManager;

        public virtual void Attack(IDamageable target)
        {
            
        }

        protected void CreateBullet()
        {
            EntityPoolManager.entityPoolInstance.PullFromPool(bulletPrefab, bullet =>
            {
                ProjectileData projectileData = new ProjectileData(baseManager.EntityID, damage, bulletSpeed);
                bullet.GetComponent<Projectile>().Initialize(projectileData);
                bullet.transform.SetPositionAndRotation(bulletSpawn.position, bulletSpawn.rotation);
                bullet.transform.SetParent(null);
                bullet.SetActive(true);
            });
        }
    }
}