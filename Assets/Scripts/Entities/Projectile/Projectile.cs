using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Projectile : Entity
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private EntityHealth myHealthScript;
        [SerializeField] private float lifeTime = 2f;
        
        private string tankID;
        private float damage;
        private float speed;
        private bool initialized;
        private int piercing;

        public string OwnerID => tankID;

        [Header("Components")]
        [SerializeField] private TrailRenderer trailRenderer;

        private float _lifeTime;

        public Action<float> OnDamageTaken;

        private void OnEnable()
        {
            trailRenderer.Clear();
        }

        private void OnDisable()
        {
            trailRenderer.Clear();
        }

        public void Initialize(ProjectileData data)
        {
            tankID = data.tankID;
            team = data.team;
            damage = data.damage;
            speed = data.bulletSpeed;
            piercing = data.piercing;
            lifeTime = data.lifeTime;
            _lifeTime = lifeTime;
            myHealthScript.SetHealth(damage);
            myHealthScript.SetMaxHealth(damage);
            initialized = true;
        }

        private void FixedUpdate()
        {
            if (initialized)
            {
                rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
            }    
        }

        private void Update()
        {
            if (initialized)
            {

                if (_lifeTime <= 0)
                {
                    EntityManager.emInstance.RemoveEntity(gameObject);
                }
                else
                {
                    _lifeTime -= Time.deltaTime;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable targetScript = other.GetComponent<IDamageable>();
            if (targetScript != null)
            {
                if (other.GetComponent<Entity>().Team == team)
                {
                    return; 
                }

                float hitEntityHealth = targetScript.GetCurrentHealth();
                float damage = myHealthScript.CurrentHealth;
                if (piercing > 1) 
                {
                    damage = myHealthScript.MaxHealth / piercing;
                }

                targetScript.TakeDamage(damage, tankID, damageTaken =>
                {
                    myHealthScript.TakeDamage(damageTaken, tankID, OnDamageTaken);
                });
            }
        }
    }
}