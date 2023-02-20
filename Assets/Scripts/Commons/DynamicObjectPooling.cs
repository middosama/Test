using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commons
{

    public abstract class DynamicObjectPooling<T> : ObjectPooling<T> where T : DynamicObjectPooling<T>
    {

        Coroutine destroyTimer = null;



        public override void SelfEnqueue()
        {
            Enqueue(1, (T)this);
        }
        public new static T Spawn(Transform parent = null)
        {
            T obj;
            if (Pool.Count > 0)
            {
                obj = Pool.Dequeue();
            }
            else
            {
                obj = Instantiate(Prefab, Prefab.PoolContainer);
            }
            obj.current = obj;
            obj.OnSpawn();

            obj.StopTimingDestroy();
            if (parent != null)
            {
                obj.transform.SetParent(parent, false);
            }
            return obj;

        }

        public override MonoBehaviour BlindSpawn(Transform parent = null) => Spawn(parent);

        //public abstract void OnInstantiate();
        public void Destroy(T obj)
        {
            if (obj == null || PoolContainer == null)
                return;

            Pool.Enqueue(obj);
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(PoolContainer, false);
            //transform.parent = Main.ObjectPool;
        }
        public new void Destroy()
        {
            if (this == null || PoolContainer == null)
                return;

            current.gameObject.SetActive(false);
            Pool.Enqueue(current);
            current.transform.SetParent(PoolContainer, false);
        }

        IEnumerator TimingDestroy(T obj, float delayTime)
        {
            yield return new WaitForSecondsRealtime(delayTime);
            Destroy(obj);
        }

        public void Destroy(T obj, float delayTime = 0)
        {
            StopTimingDestroy();
            if (delayTime > 0)
            {
                if (gameObject.activeInHierarchy)
                {
                    destroyTimer = StartCoroutine(TimingDestroy(obj, delayTime));
                }
            }
            else
            {
                Destroy(obj);
            }
        }

    }
}
