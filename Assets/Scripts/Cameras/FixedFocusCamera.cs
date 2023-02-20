using Characters;
using System;
using System.Collections;
using UnityEngine;

namespace Cameras
{
    [Obsolete]
    public class FixedFocusCamera : MonoBehaviour
    {
        Transform _focusTarget;
        public float smoothTime = 0.3f;
        public float distanceToTarget = 5f;
        public float cameraHeight = 2f;
        

        public void SetFocusTarget(Transform target)
        {
            _focusTarget = target;
        }
        

        private void LateUpdate()
        {
            
            if (_focusTarget != null)
            {
                // make camera always at target back (include target rotation)
                Vector3 targetPosition = _focusTarget.position - _focusTarget.forward * distanceToTarget;
                targetPosition.y = cameraHeight;
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime * Time.deltaTime);
                transform.LookAt(_focusTarget);
                

            }
        }
    }
}