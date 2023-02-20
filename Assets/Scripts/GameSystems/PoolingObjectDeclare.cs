using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems
{
    public class PoolingObjectDeclare : MonoBehaviour
    {

        [SerializeField]
        public List<ObjectPooling> templateList;

        public void Awake()
        {
            templateList.ForEach(x => x.SelfEnqueue());
        }

    }
}