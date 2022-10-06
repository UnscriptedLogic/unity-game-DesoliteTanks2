using Core;
using System;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace Entities
{
    public class BaseHealthClass : MonoBehaviour, IDamageable
    {
        [SerializeField] protected bool overrideHealth;
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float currentHealth;

        [SerializeField] protected float armor;

        [SerializeField] protected VFXSettings armorVFX;
        [SerializeField] protected VFXSettings hurtVFX;
        [SerializeField] protected VFXSettings deathVFX;

        [SerializeField] public event Action<float> OnHealthChanged;

        [Header("Base Components")]
        [SerializeField] protected Entity baseManager;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        protected virtual void OnEnable()
        {
            if (overrideHealth)
            {
                currentHealth = maxHealth;
            }
        }

        public void SetHealth(float newHealth) => currentHealth = newHealth;
        public void SetMaxHealth(float newMaxHealth) => maxHealth = newMaxHealth;
        public float GetCurrentHealth() => currentHealth;

        public virtual void TakeDamage(float amount, string attackerID, Action<float> OnDamageCallback)
        {
            if (attackerID == baseManager.EntityID)
                return;

            float damage = amount - armor;
            damage = Mathf.Min(damage, CurrentHealth);

            float prevHealth = currentHealth;

            if (damage <= 0f)
            {
                damage = 0f;
                armorVFX.PlayVFX(transform.position, transform.rotation);
                OnDamageCallback?.Invoke(amount);
                return;
            }

            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                EntityDeathMethod();
                currentHealth = 0f;
            }

            //Debug.Log($"{baseManager.EntityID}: Took {damage} damage, Absorbed {armor} damage, Has {currentHealth}/{maxHealth} health left, Returned {prevHealth - currentHealth + armor} damage");
            
            hurtVFX.PlayVFX(transform.position, transform.rotation);
            OnDamageCallback?.Invoke(prevHealth - currentHealth + armor);
            OnHealthChanged?.Invoke(prevHealth - currentHealth + armor);
            DamageManager.instance.EntityTookDamage(attackerID, baseManager.EntityID, prevHealth, currentHealth);
        }

        public void OnDamageCallBack(float damageTaken)
        {
            throw new NotImplementedException();
        }

        protected virtual void EntityDeathMethod()
        {
            currentHealth = 0f;
            EntityManager.emInstance.RemoveEntity(gameObject);

            deathVFX.PlayVFX(transform.position, transform.rotation);
        }
    }
}
