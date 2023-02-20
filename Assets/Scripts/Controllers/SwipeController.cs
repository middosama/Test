using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class SwipeController : MonoBehaviour , IController
    {
        public float OffsetX => offsetX;
        float offsetX = 0;
        float offsetXBeforeSwipe = 0;
        public float sensitivity = 0.01f;
        public float offsetLimit = 3f; // block width / 2 - character width / 2. Don't do this. Just for dealine
        Vector3 _startPosition;
        bool dragging;
        

        // Detect and calculate drag mouse X delta on screen point
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = Input.mousePosition;
                dragging = true;
                Debug.Log("dragging");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                offsetXBeforeSwipe = offsetX;
            }
            else if (dragging)
            {
                var delta = Input.mousePosition - _startPosition;
                //Debug.Log(delta);
                offsetX = offsetXBeforeSwipe + delta.x * sensitivity;
                offsetX = Mathf.Clamp(offsetX, -offsetLimit, offsetLimit);

            }


        }
    }
}