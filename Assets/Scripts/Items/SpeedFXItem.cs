using Characters;
using Commons;
using GameSystems;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Items
{
    public class SpeedFXItem : Item<SpeedFXItem>
    {
        [Header("Item1 is speed, Item2 is duration, Item3 is item mesh")]
        [SerializeField] List<SerializedTuple<float, float, GameObject>> itemTypeDefined;
        public Transform container;
        GameObject currentTemplate;

        SerializedTuple<float, float, GameObject> holdingValue;

        public override void OnCollect(Character character)
        {
            character.SetTemporitySpeed(holdingValue.Item1, holdingValue.Item2);
            Destroy();
            
        }

        public override void OnSpawn()
        {
            InitData();
            gameObject.SetActive(true);
        }
        void InitData()
        {
            var newHoldingValue = itemTypeDefined[Random.Range(0, itemTypeDefined.Count)];
            if (newHoldingValue == holdingValue) return;
            holdingValue = newHoldingValue;
            if (currentTemplate != null)
                Destroy(currentTemplate);
            currentTemplate = Instantiate(holdingValue.Item3, container);
            currentTemplate.transform.localPosition = Vector3.zero;
        }
    }
}