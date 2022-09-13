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

        protected virtual void OnEnable()
        {
            if (overrideHealth)
            {
                currentHealth = maxHealth;
            }
        }

        public void SetHealth(float newHealth)
        {
            currentHealth = newHealth;
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public virtual void TakeDamage(float amount, string attackerID, Action<float> OnDamageCallback)
        {
            if (attackerID == baseManager.EntityID)
                return;

            float prevHealth = currentHealth;
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                EntityDeathMethod();
            }

            OnDamageCallback?.Invoke(prevHealth - currentHealth);
            DamageManager.instance.DamageTaken(attackerID, baseManager.EntityID, prevHealth, currentHealth);
        }

        public void OnDamageCallBack(float damageTaken)
        {
            throw new NotImplementedException();
        }

        protected virtual void EntityDeathMethod()
        {
            currentHealth = 0f;
            EntityManager.instance.RemoveEntity(gameObject);
        }
    }
}
