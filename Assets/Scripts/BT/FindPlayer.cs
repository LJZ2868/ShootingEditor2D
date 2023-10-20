using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ShootEditor2D
{
    public class FindPlayer : Conditional
    {
        public SharedFloat distance;

        public SharedLayerMask layer;

        public SharedBool isFind;

        BehaviorTree bt;

        public override void OnAwake()
        {
            bt = GetComponent<BehaviorTree>();
        }

        public override TaskStatus OnUpdate()
        {
            isFind.Value = Physics2D.OverlapCircle(transform.position, distance.Value, layer.Value);
            bt.SetVariable("IsFind", isFind);

            return isFind.Value ? TaskStatus.Success : TaskStatus.Running;
        }


    }
}