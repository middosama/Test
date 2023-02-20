using Items;
using System.Collections;
using UnityEngine;

namespace Characters
{
    public class GunTurret : MonoBehaviour
    {
        public Bullet itemTemplate;
        [SerializeField] float fireRate = 1f;
        [SerializeField] float bulletSpeed = 10f;
        [SerializeField] Transform gun;


        void Awake()
        {
            if (Bullet.Prefab == null)
            {
                itemTemplate.SelfEnqueue();
            }
        }


        void Start()
        {
            StartCoroutine(IFire());
        }

        IEnumerator IFire()
        {
            while (true)
            {
                yield return new WaitForSeconds(fireRate);
                var bullet = Bullet.Spawn();
                bullet.transform.position = gun.position;
                bullet.Fire(bulletSpeed, Quaternion.LookRotation( gun.transform.up));
            }
        }

    }
}