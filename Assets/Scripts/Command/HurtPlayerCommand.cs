using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;
using UnityEngine.SceneManagement;


namespace ShootEditor2D
{
    public class HurtPlayerCommand : AbstractICommand
    {
        private float damage;

        public HurtPlayerCommand(float damage)
        {
            this.damage = damage;
        }

        protected override void OnExecute()
        {
            var playerModel = this.GetModel<IPlayerModel>();
            //²»ÊÇÎÞµÐ×´Ì¬
            if (!playerModel.IsInvincible)
            {
                playerModel.HP.Value -= this.damage;
                playerModel.IsInvincible = true;

                this.SendEvent<HurtPlayerEvent>();

                this.GetSystem<ITimeSystem>().AddDelayTask(playerModel.InvincibleTime, () =>
                {
                     playerModel.IsInvincible = false;
                });
            }

            if (playerModel.HP.Value <= 0)
            {
                SceneManager.LoadScene("GamePass");
            }
        }
    }
}