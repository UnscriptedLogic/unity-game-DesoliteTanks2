using System;

namespace Entities
{
    public interface IDamageable
    {
        void TakeDamage(float amount, string attackerID, Action<float> OnDamageCallback);

        float GetCurrentHealth();

        void OnDamageCallBack(float damageTaken);
    }
}