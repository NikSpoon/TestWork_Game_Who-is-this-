using UnityEngine;
namespace Assets.Scripts.Ai.States
{
    public class IdleState : BaseStateAI
    {
        public override void OnEnter()
        {
            _muvment.Direction = Vector3.zero;
        }

        public override void OnUpdate()
        {

        }

        public override void OnExit()
        {

        }

    }
}
