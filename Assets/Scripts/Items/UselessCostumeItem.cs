using Characters;
using Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class UselessCostumeItem : Item<UselessCostumeItem>
    {
        [SerializeField] SerializedTuple<Mesh,Material,GameObject>[] modelTemplates;
        [SerializeField] MeshFilter meshFilter;
        [SerializeField] MeshRenderer meshRenderer;
        SerializedTuple<Mesh, Material, GameObject> holdingValue;
        
        public override void OnCollect(Character character)
        {
            if (!character.SetModel(holdingValue.Item3))
                character.SetTemporitySpeed(1, 1f);
            Destroy();
        }

        public override void OnSpawn()
        {
            InitData();
            gameObject.SetActive(true);
        }

        void InitData()
        {
            holdingValue = modelTemplates[Random.Range(0, modelTemplates.Length)];
            meshFilter.mesh = holdingValue.Item1;
            meshRenderer.material = holdingValue.Item2;
        }
    }
}