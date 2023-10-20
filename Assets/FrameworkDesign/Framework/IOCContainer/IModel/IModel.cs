namespace FrameworkDesign
{
    //����Model�Ĺ���
    public interface IModel : ICanSetIOC,ICanGetIOC,ICanGetUtility,ICanSendEvent
    {
        //Model���ʼ��
        void Init();
    }


    //дһ�������� ���������ظ�д��������
    public abstract class AbstractIModel : IModel
    {
        private IIOC iOC;

        IIOC ICanGetIOC.GetIOC()
        {
            return iOC;
        }

        void IModel.Init()
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
