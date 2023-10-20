using FrameworkDesign;
namespace ShootEditor2D 
{
    public class BulletInfo : IController
    {
        public string Name { get; set; }
        //飞行速度
        public float originalSpeed;
        //飞行时间
        public float originalFlight { get; set; }
        //伤害
        public float originalDamage;
        public float Damage => originalDamage * this.GetModel<IPlayerModel>().ATK.Value;
        public float Speed => originalSpeed * this.GetModel<IPlayerModel>().BulletSpeed.Value;
        public float Flight => originalFlight * this.GetModel<IPlayerModel>().Flight.Value;

        public IIOC GetIOC()
        {
            return ShootEditor2D.Interface;
        }
    }
}
