using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Projectile : BaseManagerClass
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private EntityHealth entityHealth;
        
        private string tankID;
        private float damage;
        private float speed;
        private bool initialized;
        [SerializeField] private float lifeTime = 3f;

        [Header("Components")]
        [SerializeField] private TrailRenderer trailRenderer;

        private float _lifeTime;

        private void OnEnable()
        {
            trailRenderer.Clear();
        }

        public void Initialize(ProjectileData data)
        {
            tankID = data.tankID;
            team = data.team;
            damage = data.damage;
            speed = data.bulletSpeed;
            _lifeTime = lifeTime;
            entityHealth.SetHealth(damage);
            initialized = true;
        }

        private void Update()
        {
            if (initialized)
            {
                rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);

                if (_lifeTime <= 0)
                {
                    EntityManager.instance.RemoveEntity(gameObject);
                }
                else
                {
                    _lifeTime -= Time.deltaTime;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageScript = other.GetComponent<IDamageable>();
            if (damageScript != null)
            {
                if (other.GetComponent<BaseManagerClass>().Team == team)
                {
                    return; 
                }

                float hitEntityHealth = damageScript.GetCurrentHealth();
                damageScript.TakeDamage(damage, tankID, damageTaken =>
                {
                    entityHealth.TakeDamage(damageTaken, tankID, OnDamageCallback);
                });
            }
        }

        private void OnDamageCallback(float damageTaken)
        {
            
        }
    }
}