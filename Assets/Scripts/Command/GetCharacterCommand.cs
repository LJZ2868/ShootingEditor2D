using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    public class GetCharacterCommand : AbstractICommand
    {
        string name;

        public GetCharacterCommand(string name)
        {
            this.name = name;
        }

        protected override void OnExecute()
        {
            var characters = this.GetSystem<ICharacterSystem>().characterInfos;

            characters[name].IsHave = true;
            characters[name].Effect();

            this.GetModel<IPlayerModel>().CharacterSelectCount.Value++;
            //this.SendEvent(new GetCharacterEvent(name));
        }
    }
}