using FrameworkDesign;
using System.Xml;

namespace ShootEditor2D
{
    public interface IPlayerModel : IModel
    {
        //���Ѫ��
        BindableProperty<int> MaxHP { get; }
        //��ǰѪ��
        BindableProperty<float> HP { get; }
        //�ۼƻ�ɱ(�����ɾ�)
        BindableProperty<int> TotalKill { get; }
        //��Ѫ��ɱ����
        BindableProperty<int> LifeStealingCount { get; }

        BindableProperty<bool> IsCanDodge { get; set; }

        BindableProperty<float> ATK { get; set; }

        BindableProperty<float> Speed { get; set; }

        //����
        BindableProperty<float> FiringRate { get; set; }
        //�ʱ��
        BindableProperty<float> ReloadSeconds { get; set; }
        //��������
        BindableProperty<int> MaxClip { get; set; }
        //����
        BindableProperty<float> BulletSpeed { get; set; }
        //��������
        BindableProperty<float> Flight { get; set; }
        //����CD
        BindableProperty<float> MaxDodgeCD { get; set; }
        //�������ʱ��
        BindableProperty<float> MaxHoldTime { get; set; }

        //�Ƿ��޵�״̬
        bool IsInvincible { get; set; }
        //�޵�ʱ��
        float InvincibleTime { get; }

        BindableProperty<int> CharacterSelectCount { get; set; }

        SaveDate SaveDate();
        void LoadDate(SaveDate date); 
    }

    [System.Serializable]
    public class SaveDate
    {
        public int MaxHP;
        public float HP;
        public float ATK;
        public float Speed;
        public float FiringRate;
        public float ReloadSeconds;
        public int MaxClip;
        public float BulletSpeed;
        public float Flight;
        public float MaxDodgeCD;
        public float MaxHoldTime;
        public bool IsCanDodge;
        public int CharacterSelectCount;
    }

    public class PlayerModel : AbstractIModel, IPlayerModel
    {
        public BindableProperty<int> MaxHP { get; } = new BindableProperty<int>();
        public BindableProperty<float> HP { get; } = new BindableProperty<float>();
        public BindableProperty<int> TotalKill { get; } = new BindableProperty<int>();

        public BindableProperty<int> LifeStealingCount { get; } = new BindableProperty<int>();
        public bool IsInvincible { get; set; }

        public float InvincibleTime { get { return 1f; } }

        public BindableProperty<bool> IsCanDodge { get; set; } = new BindableProperty<bool>();
        public BindableProperty<float> ATK { get; set; } = new BindableProperty<float>();
        public BindableProperty<float> Speed { get; set; } = new BindableProperty<float>();
        public BindableProperty<float> FiringRate { get; set; } = new BindableProperty<float>();
        public BindableProperty<float> ReloadSeconds { get; set; } = new BindableProperty<float>();
        public BindableProperty<int> MaxClip { get; set; } = new BindableProperty<int>();
        public BindableProperty<float> BulletSpeed { get; set; } = new BindableProperty<float>();
        public BindableProperty<float> Flight { get; set; } = new BindableProperty<float>();
        public BindableProperty<float> MaxDodgeCD { get; set; } = new BindableProperty<float>() ;
        public BindableProperty<float> MaxHoldTime { get; set; } = new BindableProperty<float>() ;

        public BindableProperty<int> CharacterSelectCount { get; set; } = new BindableProperty<int>() { Value = 0 };

        public void LoadDate(SaveDate date)
        {
            MaxHP.Value = date.MaxHP;
            HP.Value = date.HP;
            ATK.Value = date.ATK;
            Speed.Value = date.Speed;
            FiringRate.Value = date.FiringRate;
            ReloadSeconds.Value = date.ReloadSeconds;
            MaxClip.Value = date.MaxClip;
            BulletSpeed.Value = date.BulletSpeed;
            Flight.Value = date.Flight;
            MaxDodgeCD.Value = date.MaxDodgeCD;
            MaxHoldTime.Value = date.MaxHoldTime;
            IsCanDodge.Value = date.IsCanDodge;
            CharacterSelectCount.Value = date.CharacterSelectCount;
        }

        public SaveDate SaveDate()
        {
            var date = new SaveDate
            {
                MaxHP = MaxHP.Value,
                HP = HP.Value,
                ATK = ATK.Value,
                Speed = Speed.Value,
                FiringRate = FiringRate.Value,
                ReloadSeconds = ReloadSeconds.Value,
                MaxClip = MaxClip.Value,
                BulletSpeed = BulletSpeed.Value,
                Flight = Flight.Value,
                MaxDodgeCD = MaxDodgeCD.Value,
                MaxHoldTime = MaxHoldTime.Value,
                IsCanDodge = IsCanDodge.Value,
                CharacterSelectCount = CharacterSelectCount.Value
            };
            return date;
        }



        protected override void OnInit()
        {

            var storage = this.GetUtility<IStorage>();

            //CharacterSelectCount.Value = storage.LoadInt(nameof(CharacterSelectCount), 0);
            //CharacterSelectCount.RegisterOnValueChange(newValue => 
            //{
            //    storage.SaveInt(nameof(CharacterSelectCount), newValue);
            //});

            TotalKill.Value = storage.LoadInt(nameof(TotalKill), 0);
            TotalKill.RegisterOnValueChange(newValue =>
            {
                //��¼��ɱ
                storage.SaveInt(nameof(TotalKill), newValue);
            });


            LifeStealingCount.Value = storage.LoadInt(nameof(LifeStealingCount), 0);
            LifeStealingCount.RegisterOnValueChange(newValue =>
            {
                storage.SaveInt(nameof(LifeStealingCount), newValue);
            });
        }
    }
}