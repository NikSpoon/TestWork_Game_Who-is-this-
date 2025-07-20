using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Ai.States
{
    public class PatrolState : BaseStateAI
    {
        [SerializeField] private float stopDistance = 1f;
        [SerializeField] private float waitTime = 2f;

        private List<Transform> patrolPoints = new List<Transform>();
        private Transform currentTarget;
        private float waitTimer;
        private bool waiting;
        private Transform _zoneRoot;

        public void SetZone(Collider zone)
        {
            _zoneRoot = zone.transform;
        }

        public override void OnEnter()
        {
            waiting = false;
            waitTimer = 0f;

            patrolPoints.Clear();

            if (_zoneRoot != null)
            {
                foreach (Transform child in _zoneRoot)
                {
                    patrolPoints.Add(child);
                }

                PickNewTarget();
            }
        }

        public override void OnUpdate()
        {
            if (patrolPoints.Count == 0 || _muvment == null)
                return;

            if (waiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    waiting = false;
                    PickNewTarget();
                }
                return;
            }

            Vector3 direction = currentTarget.position - transform.position;
            direction.y = 0f;

            if (direction.magnitude <= stopDistance)
            {
                _muvment.Direction = Vector3.zero;
                waiting = true;
                waitTimer = waitTime;
            }
            else
            {
                _muvment.Direction = direction.normalized;
            }
        }

        public override void OnExit()
        {
            if (_muvment != null)
                _muvment.Direction = Vector3.zero;
        }

        private void PickNewTarget()
        {
            if (patrolPoints.Count == 0)
            {
                currentTarget = null;
                return;
            }

            int index = Random.Range(0, patrolPoints.Count);
            currentTarget = patrolPoints[index];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_zoneRoot != null) return;

            if (other.CompareTag("Zone"))
            {
                SetZone(other);
            }
        }
    }
}