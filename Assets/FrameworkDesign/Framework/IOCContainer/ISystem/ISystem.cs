namespace FrameworkDesign
{
    //����System�Ĺ���
    public interface ISystem : ICanSetIOC,ICanGetIOC,ICanGetModel,ICanGetUtility,ICanSendEvent,ICanRegisterEvent,ICanGetSystem
    {
        //System���ʼ��
        void Init();
    }

    public abstract class AbstractISystem : ISystem
    {
        private IIOC iOC;

        IIOC ICanGetIOC.GetIOC()
        {
            return iOC;
        }

        void ISystem.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();

        void ICanSetIOC.SetIOC(IIOC iOC)
        {
            this.iOC = iOC;
        }
    }
}
