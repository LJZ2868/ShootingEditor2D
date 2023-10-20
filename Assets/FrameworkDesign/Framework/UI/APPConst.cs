using UnityEngine;

namespace FrameworkDesign
{
    public class APPConst : MonoBehaviour
    {
        //配置文件的路径
        public static string uiPrefabXML = Application.dataPath + "/XML/UI/UIRoot.xml";

        public static string LevelXML = Application.dataPath + "/XML/LevelFile/Scene1.xml";

        public static string Level = Application.dataPath + "/XML/LevelFile/";
        public static string Shop1 = Application.dataPath + "/XML/LevelFile/Shop1.xml";

        public static string PlayerInfoXML = Application.dataPath + "/XML/PlayerInfo/PlayerInfo.xml";

        public static string PlayerDate = Application.dataPath + "/PlayerDate/Date.date";
        public static string InitPlayerDate = Application.dataPath + "/PlayerDate/InitDate.date";

        public static string InitLevelDate = Application.dataPath + "/LevelDate/InitDate.date";
        public static string LevelDate = Application.dataPath + "/LevelDate/Date.date";
    }
}