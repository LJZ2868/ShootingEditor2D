using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace FrameworkDesign
{
    public class UIPanelManager 
    {
        private static Transform uiRoot;
        public static Transform UIRoot
        {
            get
            {
                if (uiRoot == null)
                {
                    //uiRoot = GameObject.Instantiate(Resources.Load<Transform>("UI/Canvas"));
                    //��ǩ����
                    uiRoot = GameObject.FindGameObjectWithTag("UIRoot")?.transform;
                    if (uiRoot == null)
                    {
                        //����Resources�ļ����µ�UI��Դ
                        uiRoot = GameObject.Instantiate(Resources.Load<Transform>("UI/Canvas"));

                    }
                    Object.DontDestroyOnLoad(uiRoot);
                    //��EventSystem����Canvas�¾Ϳ��Բ���д����
                    //Object.DontDestroyOnLoad(GameObject.Find("EventSystem"));
                }
                return uiRoot;
            }
        }

        private static UIPanelManager instance;
        public static UIPanelManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIPanelManager();
                }
                return instance;
            }
        }

        UIPanelManager()
        {
            Init(APPConst.uiPrefabXML);
        }

        //�����󻺴�
        Dictionary<string, BaseUIPanel> uiPanels = new Dictionary<string, BaseUIPanel>();

        //��������Դ
        Dictionary<string, BaseUIPanel> uiPanelDic = new Dictionary<string, BaseUIPanel>();

        //��ʼ�� ����xml�ļ�  ��ȡ������Դ
        private void Init(string pathName)
        {
            //��ȡ����ǩ
            XmlDocument doc = new XmlDocument();

            //doc.LoadXml();
            doc.Load(pathName);

            //��ȡXML�ļ��Ľڵ� 
            var root = doc.SelectSingleNode("UIRoot") as XmlElement;

            //��������ǩ ������Դ
            foreach (XmlElement item in root.ChildNodes)
            {
                string name = item.GetAttribute("name");
                string uiPlanelPath = item.GetAttribute("uiPanelPath");
                //Debug.Log(uiPlanelPath);

                var uiPanel = Resources.Load<BaseUIPanel>(uiPlanelPath);

                //Object.Instantiate(uiPanel);

                if (!uiPanelDic.ContainsKey(name))
                {
                    uiPanelDic.Add(name,uiPanel); 
                }
            }
        }

        public void ShowUIPanel(string uiPanelName)
        {
            //����ڻ�������ֱ�Ӽ���
            if (uiPanels.ContainsKey(uiPanelName))
            {
                uiPanels[uiPanelName].Show();
                return;
            }

            //������ڻ��������ж��Ƿ����
            if (uiPanelDic.ContainsKey(uiPanelName))
            {
                //Debug.Log("123");
                //ʵ��������
                var uiPanel = GameObject.Instantiate(uiPanelDic[uiPanelName],UIRoot);
                //���ø�����(λ��)
                //uiPanel.transform.SetParent(UIRoot);
                //������ʼλ��
                uiPanel.Show();
                //���뻺����
                uiPanels.Add(uiPanelName, uiPanel);
                //uiPanels[uiPanelName].Show();
            }
        }

        public void HideUIPanel(string uiPanelName)
        {
            //����ڻ�������ֱ������
            if (uiPanels.ContainsKey(uiPanelName))
            {
                uiPanels[uiPanelName].Hide();
            }
        }



    }
}