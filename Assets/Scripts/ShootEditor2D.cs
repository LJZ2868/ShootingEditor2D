using FrameworkDesign;

namespace ShootEditor2D
{
    //Ä£¿é¹ÜÀí
    public class ShootEditor2D : IOCContainer<ShootEditor2D>
    {
        protected override void Init()
        {
            
            RegisterUtility<IStorage>(new MyStorage());

            RegisterModel<IGunConfigModel>(new GunConfigModel());
            RegisterModel<IPlayerModel>(new PlayerModel());
            RegisterModel<IEnemyModel>(new EnemyModel());
            RegisterModel<ILevelModel>(new LevelModel());

            RegisterSystem<ISaveLoadSystem>(new SaveLoadSystem());
            RegisterSystem<ITimeSystem>(new TimeSyetem());
            RegisterSystem<IPauseSystem>(new PauseSystem());
            RegisterSystem<IObjectUIFollowSystem>(new ObjectUIFollowSystem());
            RegisterSystem<IBulletSystem>(new BulletSystem());
            RegisterSystem<IGunSystem>(new GunSystem());
            RegisterSystem<ICharacterSystem>(new CharacterSystem());
            RegisterSystem<IEnemyBulletSystem>(new EnemyBulletSystem());
            RegisterSystem<IPropSystem>(new PropSystem());
            RegisterSystem<ILevelManagerSystem>(new LevelManagerSystem());
        }
    }
}