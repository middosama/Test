using Commons;
using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class BotController : MonoBehaviour,IController
    {
        public float OffsetX => offsetX;
        public float offsetX = 0;


        public float offsetLimit = 3f; // block width / 2 - character width / 2. Don't do this. Just for dealine

        private void Start()
        {
            StartCoroutine(IRandomControl());
        }
        float direction = 0;
        IEnumerator IRandomControl()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(0.2f, 0.8f));
                direction = Random.Range(-0.1f, 0.1f);
            }
        }

        private void FixedUpdate()
        {
            offsetX = Mathf.Clamp(offsetX + direction, -offsetLimit, offsetLimit);
        }



    }
}