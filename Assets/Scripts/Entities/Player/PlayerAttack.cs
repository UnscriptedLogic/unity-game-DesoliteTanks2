using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class PlayerAttack : BaseAttackClass
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CreateBullet();
            }
        }
    }
}