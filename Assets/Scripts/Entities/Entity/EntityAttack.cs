using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class EntityAttack : BaseAttackClass, IListensToGameState
    {
        [Header("EntityAttack Extension")]
        [SerializeField] private Vector2 attackInterval;
        public Vector2 AttackInterval { get => attackInterval; set { attackInterval = value; } } 
        private float _attackInterval;

        public void OnGameStateChanged(bool won)
        {
            enabled = false;
        }

        private void OnEnable()
        {
            _attackInterval = MathHelper.RandomInRange(attackInterval);
        }

        private void Update()
        {
            if (_attackInterval <= 0)
            {
                _attackInterval = MathHelper.RandomInRange(attackInterval);
                CreateBullet();
            }
            else
            {
               _attackInterval -= Time.deltaTime;
            }
        }
    }
}