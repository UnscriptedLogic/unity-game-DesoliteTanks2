using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class DamageManager : MonoBehaviour
    {
        public Action<string, string, float, float> OnDamageTaken;
        
        public static DamageManager instance;
        public void Awake()
        {
            instance = this;
        }

        public void DamageTaken(string attackerID, string tankID, float prevHealth, float remainingHealth)
        {
            OnDamageTaken?.Invoke(attackerID, tankID, prevHealth, remainingHealth);
        }
    }
}