using Core;
using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Inventory
{
    public class PiercingArmorCollectable : ItemCollectable
    {
        [SerializeField] private int pierceIncrease;

        protected override void OnCollectableTeamInventory(Entity[] entities)
        {
            if (collected) return;

            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].gameObject.activeInHierarchy)
                {
                    EntityInventory entityInventory = entities[i].GetComponent<EntityInventory>();
                    entityInventory.AddItem(entityInventory.ItemFactory.ArmorPiercingAmmo(item, pierceIncrease), 1);
                    EntityManager.emInstance.RemoveEntity(gameObject);
                    collectVFX.PlayVFX(transform.position, transform.rotation);
                    collected = true;
                    return;
                }
            }
        }
    }
}