using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class CoinCollectable : ItemCollectable, IRequiresScoreManager
    {
        [Header("Coin Extension")]
        [SerializeField] private int scoreAmount;

        private EntityScoreManager scoreManager;

        public void InitScoreManager(EntityScoreManager scoreManager)
        {
            this.scoreManager = scoreManager;
        }

        protected override void OnCollectableTeamCollision(Entity[] entities)
        {
            scoreManager.AddScore(entities[0].Team, scoreAmount);
            EntityManager.emInstance.RemoveEntity(gameObject);
            collectVFX.PlayVFX(transform.position, transform.rotation);
        }
    }
}