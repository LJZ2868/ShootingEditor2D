using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{

    public interface IPropSystem : ISystem
    {
        Dictionary<string, AbstractPropItem> PropItems { get; }
    }


    [RequireComponent(typeof(ShowSelfInfo))]
    public abstract class AbstractPropItem : MonoBehaviour ,IController
    {
        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }

        protected virtual void Awake()
        {
            gameObject.AddComponent<LevelDateItem>().Date = new LevelDate()
            {
                Name = Name,
                Path = "Game/Prop/" + Name,
                Position = transform.position,
                Tag = "Prop"
            };
        }

        //等级
        public abstract int Level { get; }
        //道具名
        public abstract string Name { get; }
        //效果
        public abstract void Effect();

        //是否拥有
        public abstract bool IsHave { get; set; }

        public abstract string Info { get; }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                GetComponent<LevelDateItem>().Date.Tag = "IsHave";
                //this.SendCommand(new GetPropCommand(nameof(Name)));
                Effect();
                IsHave = true;
                GetComponent<ShowSelfInfo>().isTrigger.Value = false;
                gameObject.SetActive(false);

                //角色数据
                var date = this.GetModel<IPlayerModel>().SaveDate();
                this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.PlayerDate, date);
                this.GetModel<IPlayerModel>().LoadDate(date);

                //关卡数据
                var levelDates = this.GetModel<ILevelModel>().LevelDates;
                this.SendCommand(new SaveLevelDateCommand(null));
                this.GetSystem<ISaveLoadSystem>().SaveByJson(APPConst.LevelDate, levelDates);
            }
        }

    }

    public class PropSystem : AbstractISystem, IPropSystem
    {
        public Dictionary<string, AbstractPropItem> PropItems { get; } = new Dictionary<string, AbstractPropItem>();

        protected override void OnInit()
        {
            PropItems.Add(nameof(AddMaxHP),new AddATK());
            PropItems.Add(nameof(AddATK), new AddATK());
            PropItems.Add(nameof(AddMoveSpeed), new AddMoveSpeed());
        }
    }
}
