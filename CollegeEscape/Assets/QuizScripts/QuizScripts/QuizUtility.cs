using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class QuizUtility
{
    public const float Resolution_Delay_Time=1;
    public const string Save_Player_Pref_Key="Game_Highscore_Value";
    /*
    public const string xmlFileName = "Questions_Info.xml";
    public static string xmlFilePath{
        get{
            return Application.dataPath + "/" + xmlFileName;
        }
    }
    */
}

/*
[System.Serializable()]
public class Data{
    public Question[] questions = null;
    public Data(){}
    public static void Write(Data data){
        XmlSerializer serializer = new XmlSerializer(typeof(Data));
        using ( Stream stream = new FileStream(QuizUtility.xmlFilePath, FileMode.Create)){
            serializer.Serialize(stream, data);
        }
    }

    public static Data Fetch(){
        XmlSerializer deserializer = new XmlSerializer(typeof(Data));
        using ( Stream stream = new FileStream(QuizUtility.xmlFilePath, FileMode.Open)){
            var data = (Data)deserializer.Deserialize(stream);
            return data;
        }
    }
}
*/
