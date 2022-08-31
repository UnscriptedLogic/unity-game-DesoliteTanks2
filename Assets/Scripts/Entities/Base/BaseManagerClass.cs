using System;
using UnityEngine;

namespace Entities
{
    public class BaseManagerClass : MonoBehaviour
    {
        [SerializeField] protected string entityID;

        public string EntityID => entityID;
    }
}
