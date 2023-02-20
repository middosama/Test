using Characters;
using Items;
using System.Collections;
using UnityEngine;

namespace Items
{
    public class Bullet : Item<Bullet>
    {
        [SerializeField] Rigidbody rb;
        public override void OnCollect(Character character)
        {
            // hardcode for deadline
            character.SetTemporitySpeed(-2, 0.2f);
            Destroy();
        }

        public void Fire(float speed,Quaternion direction)
        {
            gameObject.SetActive(true);
            Destroy(5f);
            transform.rotation = direction;
            rb.velocity = transform.forward * speed;
        }

        public override void OnSpawn()
        {
        }

        
    }
}