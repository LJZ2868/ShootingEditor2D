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
        //ǹ������
        public BindableProperty<string> Name;

        //ǹ��״̬
        public BindableProperty<GunState> GunState;

        //�ӵ�
        public BulletInfo Bullet;

        public GunType GunType;
        
    }
}