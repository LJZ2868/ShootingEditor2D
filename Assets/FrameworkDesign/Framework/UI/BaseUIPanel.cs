using UnityEngine;
namespace FrameworkDesign
{
    //������ϷUI���� �����������
    public class BaseUIPanel : MonoBehaviour
    {
        //��ȡ���رհ�ť
        private Transform closeBtn;
        public Transform CloseBtn 
        {
            get 
            {
                if (closeBtn == null)
                    closeBtn = transform.Find("CloseBtn");
                return closeBtn;
            }
        }

        private void Awake()
        {
            //���رհ�ť��Ӽ���
            //CloseBtn.GetComponent<Button>()?.onClick.AddListener(Hide);
            //Debug.Log(CloseBtn.name);
        }

        //��ʾUIPanel
        public virtual void Show()
        {
            gameObject.SetActive(true);
            //transform.localPosition = Vector3.zero;
            //transform.localScale = new Vector3(1, 1, 1);
        }

        //����UIPanel
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}