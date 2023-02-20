using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maps
{
    /// <summary>
    ///  Only do this because the deadline. Never write code like this in future. Đm deadline
    /// </summary>
    public class TurretSpawner : MonoBehaviour
    {
        [SerializeField] GunTurret turretTemplate;
        public List<Transform> placeHolder;

        public void SpawnTurret()
        {
            var turret = Instantiate(turretTemplate, placeHolder[Random.Range(0, placeHolder.Count)]);
            turret.transform.localPosition = Vector3.zero;
        }
    }
}