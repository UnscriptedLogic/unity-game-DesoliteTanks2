using Core;
using System;
using UnityEngine;

namespace Entities
{
    public class BaseHealthClass : MonoBehaviour, IDamageable
    {
        [SerializeField] protected bool overrideHealth;
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float currentHealth;

        [Header("Base Components")]
        [SerializeField] protected BaseManagerClass baseManager;

        private DamageManager damageManager;

        protected virtual void OnEnable()
        {
            if (overrideHealth)
            {
                currentHealth = maxHealth;
            }

            damageManager = DamageManager.instance;
        }

        public void SetHealth(float newHealth)
        {
            currentHealth = newHealth;
        }

        public virtual void TakeDamage(float amount, string attackerID, Action<float> OnDamageCallback)
        {
            if (attackerID == baseManager.EntityID)
                return;

            float prevHealth = currentHealth;
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                currentHealth = 0f;
                EntityPoolManager.entityPoolInstance.PushToPool(gameObject);
            }

            OnDamageCallback?.Invoke(prevHealth - currentHealth);
            damageManager.DamageTaken(attackerID, baseManager.EntityID, prevHealth, currentHealth);

        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public void OnDamageCallBack(float damageTaken)
        {
            throw new NotImplementedException();
        }
    }
}
