using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootEditor2D
{
    //����
    public interface ICharacterSystem : ISystem
    {
        Dictionary<string, ICharacterInfo> characterInfos { get; }

        
    }

    public interface ICharacterInfo : IController
    {
        //���Ե�����
        public string Name { get; }
        public int ID { get; }
        public bool IsHave { get; set; }
        public void Effect();


        IIOC ICanGetIOC.GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }


    //���ݵ�ǰѪ����ȡ���⹥����(1Ѫ10%����������)��һ��Ѫ����
    public class HPToDamage : ICharacterInfo
    {
        public string Name { get { return nameof(HPToDamage); } }

        public bool IsHave { get; set; } 

        public int ID { get { return 0; } }

        public void Effect()
        {
            if (IsHave)
            {
                Debug.Log(Name);
                var mPlayerModel = this.GetModel<IPlayerModel>();

                //��ʼ������һ�¹�����
                UpdateAttack();
                mPlayerModel.MaxHP.Value++;

                mPlayerModel.HP.RegisterOnValueChange(newValue =>
                {
                    UpdateAttack();
                });
            }
        }

        private void UpdateAttack()
        {
            var mPlayerModel = this.GetModel<IPlayerModel>();
            //var mBulletItems = this.GetSystem<IBulletSystem>().BulletInfos;
            //foreach (var item in mBulletItems)
            //{
            //    item.Value.Damage = ((float)((int)mPlayerModel.HP.Value) / 10) * item.Value.OriginalDamage + item.Value.OriginalDamage;
            //    //Debug.Log("Damage:"+item.Value.Damage);
            //}

            mPlayerModel.ATK.Value = (int)mPlayerModel.HP.Value / 10f + 1;
            Debug.Log(mPlayerModel.ATK.Value);
            Debug.Log("atk"+this.GetSystem<IBulletSystem>().BulletInfos["pistol"].Damage);

        }

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }

    //��ɱ���˻ظ�����(2����1Ѫ)
    public class LifeStealing : ICharacterInfo
    {
        public string Name { get { return nameof(LifeStealing); } }

        public bool IsHave { get; set; } 

        public int ID { get { return 1; } }

        public void Effect()
        {
            if (IsHave)
            {
                Debug.Log(Name);
                var mPlayerModel = this.GetModel<IPlayerModel>();

                mPlayerModel.LifeStealingCount.RegisterOnValueChange(newValue =>
                {
                    if (newValue == 2)
                    {
                        mPlayerModel.HP.Value += 1;
                        mPlayerModel.LifeStealingCount.Value = 0;
                        //Debug.Log("LifeStealingCount:"+mPlayerModel.LifeStealingCount.Value);
                    }
                });
            }
        }

    }

    //�������
    //public class CanDodge : ICharacterInfo
    //{
    //    public string Name { get { return nameof(CanDodge); } }

    //    public bool IsHave { get; set; } = new bool();

    //    public int ID { get { return 2; } }

    //    public void Effect()
    //    {
    //        if (IsHave)
    //        {
    //            Debug.Log(Name);
    //            this.GetModel<IPlayerModel>().IsCanDodge.Value = true;
    //        }
    //    }
    //}

    //

    //������ٺ�����cd����
    public class GetMotility : ICharacterInfo
    {
        public string Name => nameof(GetMotility);

        public int ID => 2;

        public bool IsHave { get; set ; }

        public void Effect()
        {
            if (IsHave)
            {
                Debug.Log(nameof(GetMotility));
                this.GetModel<IPlayerModel>().Speed.Value += 30f;
                this.GetModel<IPlayerModel>().MaxDodgeCD.Value -= 1f;
            }
        }
    }

    public class CharacterSystem : AbstractISystem, ICharacterSystem
    {
        public Dictionary<string, ICharacterInfo> characterInfos { get; } = new Dictionary<string, ICharacterInfo>();

        protected override void OnInit()
        {
            characterInfos.Add(nameof(HPToDamage),new HPToDamage());
            characterInfos.Add(nameof(LifeStealing),new LifeStealing());
            characterInfos.Add(nameof(GetMotility), new GetMotility());
        }
    }
}
