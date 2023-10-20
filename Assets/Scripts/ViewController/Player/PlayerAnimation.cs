using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{

    public class PlayerAnimation : MonoBehaviour,IController
    {
        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();

            // ‹…À∂Øª≠
            this.RegisterEvent<HurtPlayerEvent>(e =>
            {
                animator.SetTrigger(AnimationStrings.IsGetHurt);
            });

            this.RegisterEvent<DodgeEvent>(e =>
            {
                animator.SetTrigger(AnimationStrings.IsDodge);
            });
            
        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }   
}
