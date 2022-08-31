using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class EntityAttack : BaseAttackClass
    {
        [Header("EntityAttack Extension")]
        [SerializeField] private float attackInterval = 1f;

        private float _attackInterval;

        private void OnEnable()
        {
            _attackInterval = attackInterval;
        }

        private void Update()
        {
            if (_attackInterval <= 0)
            {
                _attackInterval = attackInterval;
                CreateBullet();
            }
            else
            {
               _attackInterval -= Time.deltaTime;
            }
        }
    }
}