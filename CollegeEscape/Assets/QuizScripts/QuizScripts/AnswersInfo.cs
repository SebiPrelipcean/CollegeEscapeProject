using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswersInfo : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI infoTextObj;
    [SerializeField] Image toggle;

    [Header("Textures")]
    [SerializeField] Sprite uncheckedToggle;
    [SerializeField] Sprite checkedToggle;

    [Header("References")]
    [SerializeField] QuizEvents quizEvents;
    private RectTransform rect;
    public RectTransform GetRect{
        get{
            if(rect==null){
                rect=GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            } 
            return rect;
        }
    }

    private int answerIdx = -1;
    public int GetAnswerIndex { get { return answerIdx;}}

    private bool checkedInfo = false;

    //for update the answers in answers area
    public void UpdateInfo(string info, int idx){
        infoTextObj.text=info;
        answerIdx=idx;
    }

    public void Reset(){
        checkedInfo=false;
        UpdateUI();
    }

    public void ChangeState(){
        checkedInfo=!checkedInfo;
        UpdateUI();

        if(quizEvents.updateQuestionAnswer !=null){
            quizEvents.updateQuestionAnswer(this);
        }
    }

    private void UpdateUI(){
        toggle.sprite=(checkedInfo==true)?checkedToggle : uncheckedToggle;
    }
}
