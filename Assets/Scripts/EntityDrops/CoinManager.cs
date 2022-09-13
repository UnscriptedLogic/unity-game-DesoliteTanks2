using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class CoinManager : BaseManagerClass
    {
        private void OnCollisionEnter(Collision other)
        {
            BaseManagerClass baseEntityClass = other.transform.GetComponent<BaseManagerClass>();
            if (baseEntityClass != null)
            {
                if (baseEntityClass.Team != team)
                {
                    if (baseEntityClass.EntityID.Contains("tank"))
                    {
                        EntityManager.instance.RemoveEntity(gameObject);
                    }
                }
            }
        }
    }
}