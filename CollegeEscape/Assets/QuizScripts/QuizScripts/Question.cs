/*using System;
using System.Collections.Generic;
using UnityEngine;

public enum AnswerType{
        MULTIPLE,
        SINGLE
    }

[Serializable()]
public class Answer{
    public string Info = string.Empty; 
    public bool IsCorrect = false;

    public Answer(){}
}
[Serializable()]
public class Question
{
    public String GetInfo = null;

    public Answer[] GetAnswers = null;

    //Parameters
    public Boolean GetUseTimer = false;

    public Int32 GetTimer = 0;

    public AnswerType GetAnswerType = AnswerType.SINGLE;

    public Int32 GetAddScore = 0;

    public Question(){}

    public List<int> GetCorrectAnswers(){
        List<int> correctAnswers=new List<int>();
        for(int i=0;i<GetAnswers.Length;i++){
            if(GetAnswers[i].IsCorrect){
                correctAnswers.Add(i);
            }
        }
        return correctAnswers;
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Answer{
    [SerializeField] private string info;
    public string Info{get {return info;}}

    [SerializeField] private bool isCorrect;
    public bool IsCorrect{get{return isCorrect;}}
}
[CreateAssetMenu(fileName="New Question", menuName="Quiz/new Question")]
public class Question : ScriptableObject
{

    public enum AnswerType{
        MULTIPLE,
        SINGLE
    }

    [SerializeField] private string info=string.Empty;
    public string GetInfo{get {return info;}}

    [SerializeField] private Answer[] answers=null;
    public Answer[] GetAnswers{get {return answers;}}

    //Parameters
    [SerializeField] private bool useTimer = false;
    public bool GetUseTimer { get {return useTimer;}}

    [SerializeField] private int timer=0;
    public int GetTimer { get {return timer;}}

    [SerializeField] private AnswerType answerType=AnswerType.MULTIPLE;
    public AnswerType GetAnswerType { get {return answerType;}}

    [SerializeField] private int addScore = 10;
    public int GetAddScore { get {return addScore;}}

    public List<int> GetCorrectAnswers(){
        List<int> correctAnswers=new List<int>();
        for(int i=0;i<answers.Length;i++){
            if(answers[i].IsCorrect){
                correctAnswers.Add(i);
            }
        }
        return correctAnswers;
    }
}
