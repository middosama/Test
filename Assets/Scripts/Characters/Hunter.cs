using GameSystems;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Commons;

namespace Characters
{
    public class Hunter : Character
    {
        public const string ANIM_ATTACK = "Attack";
        public Character Target { get; private set; }
        public float attackRange = 0.3f;
        public UnityEvent<Character> onCatch = new UnityEvent<Character>();

        private void FixedUpdate()
        {
            CheckDistanceToTarget();
        }

        public Hunter SetTarget(Character target)
        {
            Target = target;
            return this;
        }

        void CheckDistanceToTarget()
        {
            if (Target != null && Target.Speed < Speed) // Hunter never catch up with the target if the target is faster
            {
                if (Target.CurrentPath == CurrentPath && Target.PathDistance - PathDistance < attackRange)
                {
                    
                    transform.DOMove(Target.transform.position, 0.2f).OnComplete(() =>
                    {
                        onCatch.Invoke(Target);
                        State = (int)CharacterState.HunterWin;
                    });
                }
            }
        }
    }
}