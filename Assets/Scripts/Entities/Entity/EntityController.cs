using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class EntityController : BaseManagerClass
    {
        [SerializeField] protected Transform target;
        [SerializeField] protected bool doBruteForcePlayerLocate;

        public Action OnTargetSet;

        protected void OnEnable()
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

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            OnTargetSet?.Invoke();
        }

        public Transform GetTarget(out bool isValid)
        {
            if (target == null)
            {
                isValid = false;
                target = null;
                Debug.Log("Target is null");
                return null;
            }

            isValid = true;
            return target;
        }

        public void DoFindPlayerTag()
        {
            SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
        }
    }
}