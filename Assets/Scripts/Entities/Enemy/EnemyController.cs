using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class EnemyController : EntityController, IListensToEntityCreated
    {
        [SerializeField] protected bool doBruteForcePlayerLocate;

        protected override void OnEnable()
        {
            if (target != null)
            {
                OnTargetSet?.Invoke();
            }
            else
            {
                if (doBruteForcePlayerLocate)
                {
                    DoFindPlayerTag();
                }
            }
        }

        public void OnEntityCreated(GameObject entity)
        {
            //if (entity.GetComponent<BaseManagerClass>().EntityID.Contains("player"))
            //{
            //    SetTarget(entity.transform);
            //    Debug.Log("Recalculating");
            //}
        }
    }
}