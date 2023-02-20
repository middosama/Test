using Commons;
using System.Collections;
using UnityEngine;

namespace GameSystems
{
    public class PoolingEnqueuer : MonoBehaviour
    {
        [SerializeField] private ObjectPooling[] poolingObjectTemplates;

        private void Start()
        {
            foreach (var template in poolingObjectTemplates)
            {
                template.SelfEnqueue();
            }
        }
    }
}