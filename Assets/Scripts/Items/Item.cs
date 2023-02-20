using Characters;
using Commons;
using GameSystems;
using Items;
using System.Collections;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Collider))]
    public abstract class Item<T> : DynamicObjectPooling<T>, ICollectable where T : Item<T>
    {
        public override Transform PoolContainer => GameplayController.poolingObjectContainer;
        public abstract void OnCollect(Character character);

        private void OnTriggerEnter(Collider collision)
        {
            var collider = collision.GetComponent<Character>();
            if (collider != null)
            {
                OnCollect(collider);
            }
        }
    }
    
}