namespace FrameworkDesign
{
    //���ֲ�
    //����Controller�Ĺ���
    public interface IController : ICanGetIOC,ICanSendCommand,ICanGetModel,ICanGetSystem,ICanRegisterEvent,ICanSendQuery
    {

    }

    //����ͨ���̳�AbstractIController��GetIOC��ȡIOCContainer����ֱ�ӻ�ȡ�����е�����
    //public abstract class AbstractIController : IController
    //{

    //    public IIOC GetIOC()
    //    {
    //        return IOCManager.Interface;
    //    }

    //    public void SetIOC(IIOC iOC)
    //    {
    //        //GetIOC().GetModel<>
    //    }

        
    //}
}

