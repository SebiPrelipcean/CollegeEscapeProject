using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataCreatorWindow : EditorWindow
{
    /*
   [SerializeField] Data data = new Data();
   SerializedObject serializedObject = null;
   SerializedProperty questionProperty = null;

   private void OnEnable(){
       serializedObject = new SerializedObject(this);
       data.questions = new Question[0];
       questionProperty = serializedObject.FindProperty("data").FindPropertyRelative("Question");
       Debug.Log(questionProperty);
   }

    [MenuItem("Game/Data Creator")]
   public static void OpenWindow(){
       var window = EditorWindow.GetWindow<DataCreatorWindow>("Creator");
       window.minSize = new Vector2(510.0f, 344.0f);
       window.Show();
   }

   private void OnGUI(){

        #region Header Region

       Rect headerRect = new Rect(15, 15, this.position.width - 30, 65);
       GUI.Box(headerRect, GUIContent.none);

       GUIStyle headerStyle = new GUIStyle(EditorStyles.largeLabel){
           fontSize = 26,
           alignment = TextAnchor.UpperLeft
       };

        headerRect.x += 5;
        headerRect.width -= 10;
        headerRect.y += 5;
        headerRect.height -= 10;

       GUI.Label(headerRect, "Data to Xml Creator", headerStyle);

       Rect summaryRect = new Rect(headerRect.x + 25, (headerRect.y + headerRect.height) - 20, headerRect.width - 50, 15);
       GUI.Label(summaryRect, "Create the data that needs to be included into the Xml file");
        #endregion

        #region Body Region

            Rect bodyRect = new Rect(15, (headerRect.y + headerRect.height) + 20, this.position.width -30, this.position.height - (headerRect.y + headerRect.height) -80);
            GUI.Box(bodyRect, GUIContent.none);

            var arraySize = data.questions.Length;

            Rect propertyRect = new Rect(bodyRect.x + 10, bodyRect.y + 10, bodyRect.width - 20, 17);
            //EditorGUI.PropertyField(propertyRect, questionProperty);

        #endregion
   }
   */
}
