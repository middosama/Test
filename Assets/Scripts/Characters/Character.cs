using Commons;
using Controllers;
using GameSystems;
using Items;
using Maps;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        //public const string ANIM_RUN = "Run";
        //public const string ANIM_LOSE = "Lose";
        //public const string ANIM_WIN = "Win";

        public UnityEvent onReachEnd = new UnityEvent();
        [SerializeField] float _speed = 1.9f;
        private float _fXSpeed = 0;
        public float FXSpeed
        {
            get => _fXSpeed;
            set
            {
                _fXSpeed = value;
                animator.SetFloat("Speed", Speed);
            }
        }

        public float smoothTime = 0.3f;
        public IController controller;

        [SerializeField] Animator animator;


        private int _state = -2;
        public int State
        {
            get => _state;
            set
            {
                _state = value;
                animator.SetInteger("State", _state);
            }
        }

        public PathBlock CurrentPath { get; private set; }
        public float PathDistance { get; private set; }
        public float Speed => _speed + FXSpeed;

        public GameObject defaultModel;
        GameObject currentModel;
        GameObject modelInstance;


        Coroutine moveCoroutine;
        Coroutine temporitySpeedCoroutine;

        /// <summary>
        /// Make the character move along the path
        /// </summary>
        /// <param name="path"> Start path </param>
        /// <param name="startDistance"> Start distance (on start block path) </param>
        public void Depart(float startDistance = 0)
        {
            PathDistance = startDistance;
            moveCoroutine = StartCoroutine(IMove());
            State = (int)CharacterState.Running;
        }

        public void ReadyOnPath(PathBlock path)
        {
            CurrentPath = path;
            transform.position = CurrentPath.GetPointAtDistance(0);
            SetModel(defaultModel);
            State = (int)CharacterState.Idle;
        }

        private void Start()
        {
            
        }


        public void Stop()
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }

        public Character SetController(IController controller)
        {
            this.controller = controller;
            return this;
        }

        public void SetAnimation(string name)
        {
            animator.Play(name);
        }

        public bool SetModel(GameObject template)
        {
            if (currentModel != template)
            {
                if (modelInstance != null)
                {
                    Destroy(modelInstance.gameObject);
                }
                modelInstance = Instantiate(template, transform);
                currentModel = template;
                this.WaitNewFrame(() =>
                {
                    animator.Rebind();
                    animator.SetFloat("Speed", Speed);
                    animator.SetInteger("State", _state);
                });
                return true;
            }
            return false;
        }

        public Character SetTemporitySpeed(float speed, float duration)
        {
            if (temporitySpeedCoroutine != null)
            {
                StopCoroutine(temporitySpeedCoroutine);
            }
            temporitySpeedCoroutine = StartCoroutine(ITemporitySpeedSet(speed, duration));
            return this;
        }

        IEnumerator ITemporitySpeedSet(float speed, float duration)
        {
            FXSpeed = speed;
            yield return new WaitForSeconds(duration);
            FXSpeed = 0;
        }

        IEnumerator IMove()
        {
            while (CurrentPath != null)
            {
                PathDistance += Speed * Time.deltaTime;
                if (CurrentPath.IsEndOfPath(PathDistance))
                {
                    CurrentPath = CurrentPath.NextPath;
                    PathDistance = 0;
                    continue;
                }
                // Smoothly move the character towards the target point.
                transform.rotation = Quaternion.LookRotation(CurrentPath.GetDirectionAtDistance(PathDistance));
                // rotate offset to match transform direction
                var rotatedOffset = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * (controller.OffsetX * Vector3.right);
                transform.position = Vector3.Lerp(transform.position, CurrentPath.GetPointAtDistance(PathDistance) + rotatedOffset, smoothTime);
                yield return null;
            }
            onReachEnd.Invoke();
        }


    }


}
