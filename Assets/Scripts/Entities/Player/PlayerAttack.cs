using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class PlayerAttack : BaseAttackClass, IListensToGameState, IListensToEntityDeath
    {
        private bool canShoot = true;
        
        [SerializeField] private float shootCooldown = 0.1f;
        private float _shootCooldown;

        private void OnEnable()
        {
            canShoot = true;   
        }

        public void OnEntityDeath(Entity entity)
        {
            if (entity.IsProjectile)
            {
                Projectile projectile = entity.GetComponent<Projectile>();
                if (projectile.OwnerID == baseClass.EntityID)
                {
                    canShoot = true;
                }
            }   
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (canShoot)
                {
                    if (_shootCooldown <= 0f)
                    {
                        CreateBullet();
                        canShoot = false;
                        _shootCooldown = shootCooldown;
                    }
                }
            }

            _shootCooldown -= Time.deltaTime;
        }

        public void OnGameStateChanged(bool won)
        {
            enabled = false;
        }
    }
}