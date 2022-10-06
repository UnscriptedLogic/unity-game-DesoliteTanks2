using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Inventory
{
    public class DamageCollectable : ItemCollectable
    {
        [SerializeField] private float damageIncrease;

        protected override void OnCollectableTeamInventory(Entity[] entities)
        {
            if (collected) return;

            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].gameObject.activeInHierarchy)
                {
                    EntityInventory entityInventory = entities[i].GetComponent<EntityInventory>();
                    entityInventory.AddItem(entityInventory.ItemFactory.DamageUpgrade(item, damageIncrease), 1);
                    EntityManager.emInstance.RemoveEntity(gameObject);
                    collectVFX.PlayVFX(transform.position, transform.rotation);
                    collected = true;
                }
            }
        }
    }
}