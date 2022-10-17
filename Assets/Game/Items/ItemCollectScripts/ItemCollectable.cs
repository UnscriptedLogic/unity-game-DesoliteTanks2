using Core;
using Entities.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class ItemCollectable : Entity
    {
        [Header("Collectable Settings")]
        [SerializeField] protected ItemSO item;
        [SerializeField] protected float pickupDelay = 0.5f;
        [SerializeField] protected float lifeTime;
        [SerializeField] protected float checkRadius;
        [SerializeField] protected LayerMask entityLayer;
        [SerializeField] protected string[] collectableTeams;
        [SerializeField] protected bool drawGizmos;
        [Space(10)]
        [SerializeField] protected Vector2 blinkSpeedRange;
        [SerializeField] protected float startBlinkTime;
        [SerializeField] protected MeshRenderer meshRenderer;
        [Space(10)]
        [SerializeField] protected float rotationSpeed;
        [Space(10)]
        [SerializeField] protected VFXSettings collectVFX;

        protected float _lifetime, _blinkInterval, _pickupDelay;
        protected bool collected = false;
        public string[] CollectableTeams => collectableTeams;

        private void OnEnable()
        {
            _lifetime = lifeTime;
            meshRenderer.enabled = true;
            _pickupDelay = pickupDelay;
            collected = false;
        }

        protected virtual void Update()
        {
            if (_lifetime <= 0f)
            {
                EntityManager.emInstance.RemoveEntity(gameObject);
                return;
            }

            if (_pickupDelay >= 0)
            {
                _pickupDelay -= Time.deltaTime;
            }

            if (_lifetime <= startBlinkTime)
            {
                if (_blinkInterval <= 0f)
                {
                    meshRenderer.enabled = !meshRenderer.enabled;
                    float lifetimePercent = _lifetime / lifeTime * 100f;
                    _blinkInterval = lifetimePercent / 100 * blinkSpeedRange.y;
                    _blinkInterval = Mathf.Clamp(_blinkInterval, blinkSpeedRange.x, blinkSpeedRange.y);
                }
            }

            _lifetime -= Time.deltaTime;
            _blinkInterval -= Time.deltaTime;

            AnimateItem();
        }

        protected virtual void AnimateItem()
        {
            meshRenderer.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        }

        protected virtual void FixedUpdate()
        {
            if (_pickupDelay > 0f)
                return;

            Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius, entityLayer);
            if (colliders.Length > 0)
            {
                Entity[] entities = new Entity[colliders.Length];
                for (int i = 0; i < entities.Length; i++)
                {
                    entities[i] = colliders[i].GetComponent<Entity>();
                }
                OnAnyEntityCollision(entities);

                List<Entity> collectableTeamsEntities = new List<Entity>();
                for (int i = 0; i < entities.Length; i++)
                {
                    for (int j = 0; j < collectableTeams.Length; j++)
                    {
                        if (entities[i].Team.Contains(collectableTeams[j]))
                        {
                            collectableTeamsEntities.Add(entities[i]);
                        }
                    }
                }

                if (collectableTeamsEntities.Count > 0)
                {
                    OnCollectableTeamCollision(collectableTeamsEntities.ToArray());
                }

                List<Entity> collectableInventoryEntities = new List<Entity>();
                for (int i = 0; i < collectableTeamsEntities.Count; i++)
                {
                    if (collectableTeamsEntities[i].GetComponent<EntityInventory>())
                    {
                        collectableInventoryEntities.Add(collectableTeamsEntities[i]);
                    }
                }

                if (collectableInventoryEntities.Count > 0)
                {
                    OnCollectableTeamInventory(collectableInventoryEntities.ToArray());
                }
            }
        }

        protected virtual void OnAnyEntityCollision(Entity[] entities)
        {

        }

        protected virtual void OnCollectableTeamCollision(Entity[] entities)
        {

        }

        protected virtual void OnCollectableTeamInventory(Entity[] entities)
        {
            
        }

        protected void OnDrawGizmos()
        {
            if (!drawGizmos)
                return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, checkRadius);
        }
    }
}