using Characters;
using Commons;
using GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    
    public class CoinItem : Item<CoinItem>
    {
        
        [SerializeField] List<SerializedTuple<int, Mesh>> coinTypeDefined;
        [SerializeField] MeshFilter meshFilter;

        SerializedTuple<int, Mesh> holdingValue;

        public override void OnCollect(Character character)
        {
            if (character == GameplayController.player)
            {
                SignalHub.onCoinUpdate.Invoke(holdingValue.Item1);
            }
            Destroy();
        }

        public override void OnSpawn()
        {
            InitData();
            gameObject.SetActive(true);
        }

        void InitData()
        {
            holdingValue = coinTypeDefined[Random.Range(0, coinTypeDefined.Count)];
            meshFilter.mesh = holdingValue.Item2;
        }
    }
}