using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="QuizEvents", menuName="Quiz/new QuizEvents")]
public class QuizEvents : ScriptableObject
{
    //update new question
    public delegate void UpdateQuestionUICallback(Question question);
    public UpdateQuestionUICallback UpdateQuestionUI;

    public delegate void UpdateQuestionAnswerCallback(AnswersInfo answersInfo);
    public UpdateQuestionAnswerCallback updateQuestionAnswer;

    public delegate void DisplayResplutionScreenCallback(UIManager.ResolutionScreenType resolutionScreenType,int score);
    public DisplayResplutionScreenCallback displayResplutionScreen;

    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback scoreUpdated;

    [HideInInspector]
    public int currentFinalScore;
    [HideInInspector]
    public int startHighscore;

}
