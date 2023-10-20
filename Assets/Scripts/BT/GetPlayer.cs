using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ShootEditor2D
{
    public class GetPlayer : Action
    {
        //Íæ¼Ò
        public SharedGameObject target;

        public override void OnAwake()
        {
            target = Player.Instance.gameObject;

            var bt = GetComponent<BehaviorTree>();
            bt.SetVariable("Player",target);
        }

        public override TaskStatus OnUpdate()
        {
            //Debug.Log(target.Value.transform.position);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target.Value = null;
        }
    }
}