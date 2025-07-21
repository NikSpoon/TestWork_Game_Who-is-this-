using UnityEngine;

namespace Assets.Scripts.Ai.States
{
    public class ToSiginState : BaseStateAI
    {
        private Transform _target; 
        private float stopDistance = 1f;

        public override void OnEnter()
        {
            if (_muvment == null)
                _muvment = GetComponent<Muvment>();

            FindSigin();
        }

        public override void OnUpdate()
        {
            if (_target == null || _muvment == null)
                return;

            Vector3 direction = _target.position - transform.position;
            direction.y = 0f;

            if (direction.magnitude <= stopDistance)
            {
                _muvment.Direction = Vector3.zero;

                FindSigin();
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

        private void FindSigin()
        {
            var trigers = GameObject.FindGameObjectsWithTag("Zone");
            if (trigers.Length == 0)
            {
                _target = null;
                return;
            }

            System.Random random = new System.Random();
            int index = random.Next(trigers.Length);
            _target = trigers[index].transform;
        }
    }
}