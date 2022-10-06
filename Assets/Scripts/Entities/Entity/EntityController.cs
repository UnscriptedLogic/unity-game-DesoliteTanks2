using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class EntityController : Entity
    {
        [SerializeField] protected Transform target;

        public Action OnTargetSet;

        protected virtual void OnEnable()
        {
            if (target != null)
            {
                OnTargetSet?.Invoke();
            }
        }

        protected override void Start()
        {
            base.Start();
            entityID += UnityEngine.Random.Range(1000, 9999);
        }

        public void SetTarget(Transform newTarget)
        {
            if (newTarget == null)
            {
                return;
            }

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
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                SetTarget(playerObject.transform);
            }
        }

        protected void OnDisable()
        {
            target = null;
        }
    }
}