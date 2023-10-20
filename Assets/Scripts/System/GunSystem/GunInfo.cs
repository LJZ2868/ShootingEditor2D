using FrameworkDesign;

namespace ShootEditor2D
{
    public enum GunState
    {
        Idle,
        Shooting,
        Reloading,
        Hold
    }

    public enum GunType
    {
        Bullet,
        Laser,
        Penetrate
    }

    public class GunInfo 
    {
        //枪的名称
        public BindableProperty<string> Name;

        //枪的状态
        public BindableProperty<GunState> GunState;

        //子弹
        public BulletInfo Bullet;

        public GunType GunType;
        
    }
}