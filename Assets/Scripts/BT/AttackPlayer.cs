using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ShootEditor2D
{
    public class AttackPlayer : Action
    {
        public override TaskStatus OnUpdate()
        {

            return TaskStatus.Running;
        }

        void Attack()
        {
            
        }
    }
}