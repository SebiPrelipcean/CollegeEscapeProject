using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable()]
public struct UIManagerParameters{
    [Header("Answers Options")]
    [SerializeField] float margins;
    public float GetMargins{ get { return margins; }}

    [Header("Resolution Screen Options")]
    [SerializeField] Color correctBGColor;
    public Color GetCorrectBGColor{get{return correctBGColor;}}
    [SerializeField] Color incorrectBGColor;
    public Color GetIncorrectBGColor{get{return incorrectBGColor;}}
    [SerializeField] Color finalBGColor;
    public Color GetFinalBGColor{get{return finalBGColor;}}
}

[Serializable()]
//this struct will contain UI elements
public struct UIElements{
    [SerializeField] RectTransform answersContentArea;
    public RectTransform GetAnswersContentArea{ get {return answersContentArea;}}

    [SerializeField] TextMeshProUGUI questionInfoTextObj;
    public TextMeshProUGUI GetQuestionInfoTextObj{get {return questionInfoTextObj;}}

    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI GetScoreText{get{return scoreText;}}

    [Space]
    //animator for resolution screen
    [SerializeField] Animator resolutionScreenAnimator;
    public Animator GetResolutionScreenAnimator{get{return resolutionScreenAnimator;}}

    //for changing the background color depending on resolution
    [SerializeField] Image resolutionBackground;
    public Image GetResolutionBackground{get{return resolutionBackground;}}

    [SerializeField] TextMeshProUGUI resolutionStateInfoText;
    public TextMeshProUGUI GetResolutionStateInfoText{get{return resolutionStateInfoText;}}

    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI GetResolutionScoreText{get{return resolutionScoreText;}}

    [Space]
    [SerializeField] TextMeshProUGUI highscoreText;
    public TextMeshProUGUI GetHighscoreText{get{return highscoreText;}}

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup GetMainCanvasGroup{get{return mainCanvasGroup;}}

    //elements that will apear only when game is over
    [SerializeField] RectTransform finishUIElements;
    public RectTransform GetFinishUIElements{get{return finishUIElements;}}
}
public class UIManager : MonoBehaviour
{
    public enum ResolutionScreenType{
        CORRECT,
        INCORRECT,
        FINISHED
    }

    //header for communication between GameManager and UIManager throught QuizEvents
    [Header("References")] 
    [SerializeField] QuizEvents quizEvents;

    [Header("UI Elements (Prefabs)")]
    [SerializeField] AnswersInfo answersPrefab;
    [SerializeField] UIElements uIElements;

    [Space]
    [SerializeField] UIManagerParameters parameters;

    //answers avaible
    List<AnswersInfo> currentAvaibleAnswers=new List<AnswersInfo>();
    //we will use this integer to store animator parameter hash
    private int resolutionParameterHash=0;

    private IEnumerator IE_Display_Time_Resolution;

    void Start(){
        UpdateScoreUI();
        resolutionParameterHash=Animator.StringToHash("ScreenState");
    }

    public void OnEnable(){
        quizEvents.UpdateQuestionUI += UpdateQuestionUI;
        
        quizEvents.displayResplutionScreen += DisplayResolutionScreen;

        quizEvents.scoreUpdated += UpdateScoreUI;
    }

    public void OnDisable(){
        quizEvents.UpdateQuestionUI -= UpdateQuestionUI;

        quizEvents.displayResplutionScreen -= DisplayResolutionScreen;

        quizEvents.scoreUpdated -= UpdateScoreUI;
    }

    public void UpdateQuestionUI(Question question){
        //in this method we will display question information text and we will prepare new answers

        uIElements.GetQuestionInfoTextObj.text=question.GetInfo;
        PrepareAnswers(question);
    }

    public void UpdateScoreUI(){
         uIElements.GetScoreText.text = "Score: " + quizEvents.currentFinalScore;
    }

    private void PrepareAnswers(Question question){
        DeleteAnswers();

        float offset = 0 - parameters.GetMargins;
        for(int i=0;i<question.GetAnswers.Length;i++){
            AnswersInfo newAnswer=(AnswersInfo)Instantiate(answersPrefab,uIElements.GetAnswersContentArea);
            newAnswer.UpdateInfo(question.GetAnswers[i].Info,i);

            newAnswer.GetRect.anchoredPosition=new Vector2(0,offset);

            offset-=(newAnswer.GetRect.sizeDelta.y + parameters.GetMargins);
            uIElements.GetAnswersContentArea.sizeDelta=new Vector2(uIElements.GetAnswersContentArea.sizeDelta.x,offset * -1);

            currentAvaibleAnswers.Add(newAnswer);
        }
    }

    private void DeleteAnswers(){
        foreach(var answer in currentAvaibleAnswers){
            Destroy(answer.gameObject);
        }
        currentAvaibleAnswers.Clear();
    }

    //method that will display the resolution screen
    public void DisplayResolutionScreen(ResolutionScreenType type,int score){
        UpdateResolutionUI(type,score);
        uIElements.GetResolutionScreenAnimator.SetInteger(resolutionParameterHash,2);
        //disable the main game functionality
        uIElements.GetMainCanvasGroup.blocksRaycasts=false;

        //determine if our type is not equal to finish, we will fade out the resolution screen in a certain amount of time
        if(type!=ResolutionScreenType.FINISHED){
            if(IE_Display_Time_Resolution!=null){
                StopCoroutine(IE_Display_Time_Resolution);
            }
            IE_Display_Time_Resolution = DisplayTimeResolution();
            StartCoroutine(IE_Display_Time_Resolution);
            
        }
        
    }

    IEnumerator DisplayTimeResolution(){
        yield return new WaitForSeconds(QuizUtility.Resolution_Delay_Time);
        uIElements.GetResolutionScreenAnimator.SetInteger(resolutionParameterHash,1);
        uIElements.GetMainCanvasGroup.blocksRaycasts=true;
    }

    private void UpdateResolutionUI(ResolutionScreenType type,int score){

        var highscoreUpdate=PlayerPrefs.GetInt(QuizUtility.Save_Player_Pref_Key);

        switch(type){
            case ResolutionScreenType.CORRECT:
                uIElements.GetResolutionBackground.color=parameters.GetCorrectBGColor;
                uIElements.GetResolutionStateInfoText.text="CORRECT";
                uIElements.GetResolutionScoreText.text="+" + score;
                break;
            case ResolutionScreenType.INCORRECT:
                uIElements.GetResolutionBackground.color=parameters.GetIncorrectBGColor;
                uIElements.GetResolutionStateInfoText.text="WRONG";
                uIElements.GetResolutionScoreText.text="-" + score;
                break;
            case ResolutionScreenType.FINISHED:
                uIElements.GetResolutionBackground.color=parameters.GetFinalBGColor;
                uIElements.GetResolutionStateInfoText.text="FINAL SCORE";
                StartCoroutine(CalculateScore());
                uIElements.GetFinishUIElements.gameObject.SetActive(true);
                uIElements.GetHighscoreText.gameObject.SetActive(true);
                //display highscore using player preps (store data)
                uIElements.GetHighscoreText.text=((highscoreUpdate>quizEvents.startHighscore)?"<color=yellow>new </color>" : string.Empty)
                                                    + "Highscore: " + highscoreUpdate;
                break;
        }
    }

    private IEnumerator CalculateScore(){
        var valueS=0;
        while(valueS<quizEvents.currentFinalScore){
            valueS++;
            uIElements.GetResolutionScoreText.text=valueS.ToString();
            
            yield return null;
        }
    }
}
