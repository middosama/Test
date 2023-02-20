using PathCreation;
using System.Collections;
using UnityEngine;
using Commons;

namespace Maps
{
    public class PathBlock : MonoBehaviour
    {
        [SerializeField] PathCreator pathCreator;
        [SerializeField]
        Vector3 startDirection = Vector3.forward, endDirection = Vector3.forward;
        [SerializeField] Vector3 pivotOffset;
        public int itemInLine = 2;
        float angle = 0;
        public PathBlock NextPath { private set; get; }

        private void Start()
        {
            Debug.Log(EndPosition);
        }
        /// <summary>
        /// End direction rotated according to angle
        /// </summary>
        public Vector3 WorldEndDirection => Quaternion.Euler(0, angle, 0) * endDirection;
        public Vector3 EndPosition => Quaternion.Euler(0, angle, 0) * pathCreator.bezierPath.GetPoint(pathCreator.bezierPath.NumPoints - 1) * transform.localScale.y + transform.localPosition;
        public Vector3 RotatedPivotOffset => Quaternion.Euler(0, angle, 0) * pivotOffset * transform.localScale.y;


        public Vector3 GetPointAtDistance(float distance) => pathCreator.path.GetPointAtDistance(distance);
        public bool IsEndOfPath(float distance) => PathLength - distance < 0.01f;
        public float PathLength => pathCreator.path.length;
        public Vector3 GetNormalAtDistance(float distance) => pathCreator.path.GetNormalAtDistance(distance);

        public Vector3 GetDirectionAtDistance(float distance) => pathCreator.path.GetDirectionAtDistance(distance);

        /// <summary>
        /// Set next path manually
        /// </summary>
        public void SetNextPath(PathBlock nextPath)
        {
            NextPath = nextPath;
        }


        /// <summary>
        /// Rotate pathBlock to match startDirection with required direction
        /// </summary>
        /// <returns>New rotated instance</returns>
        public PathBlock CloneNextPath(PathBlock previousPath, Transform parent = null)
        {
            var clone = Instantiate(this, parent);
            var angle = startDirection.CalculateAngleTo(previousPath.WorldEndDirection);
            clone.angle = angle;
            clone.transform.localRotation = Quaternion.Euler(0, angle, 0);

            //clone.transform.position = previousPath.EndPosition + Vector2.Angle( clone.WorldEndDirection.ToVector2(), ));
            clone.transform.localPosition = previousPath.EndPosition  + clone.RotatedPivotOffset.RotateAroundY(previousPath.WorldEndDirection);
            previousPath.NextPath = clone;
            return clone;
        }

    }
}