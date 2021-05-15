using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] QuizEvents events = null;

    private Question[] questions = null;

    private List<AnswersInfo> chosenAnswers=new List<AnswersInfo>();
    private List<int> finishedQuestions = new List<int>();
    private int currentQuestion=0;

    private IEnumerator IE_wait_round = null;
    private IEnumerator IE_Start_Timer;

    private bool IsFinished{
        get{
            return (finishedQuestions.Count<questions.Length) ? false : true;
        }
    }

    [SerializeField] Animator timerAnimator = null;
    [SerializeField] TextMeshProUGUI timerText = null;
    private Color timerDefaultColor;
    [SerializeField] Color timerHalfColor = Color.yellow;
    [SerializeField] Color timerAlmostDoneColor = Color.red;

    private int timerStateHash = 0;
    
    void Start(){

        //cache the highscore
        events.startHighscore = PlayerPrefs.GetInt(QuizUtility.Save_Player_Pref_Key);

        timerDefaultColor = timerText.color;
        LoadQuestions();

        timerStateHash = Animator.StringToHash("TimerState");

        //random questions
        var randQuestion=UnityEngine.Random.Range(int.MinValue,int.MaxValue);
        UnityEngine.Random.InitState(randQuestion);
        
        DisplayQuestion();
    }

    void Awake(){
        events.currentFinalScore = 0;
    }

    public void DisplayQuestion(){
        DeleteAnswers();
        var question=GenerateRandomQuestion();

        if(events.UpdateQuestionUI != null){
            events.UpdateQuestionUI(question);
        }else{
            Debug.Log("Something went wrong during updating the question UI.");
        }

        if(question.GetUseTimer){
            UpdateTimer(question.GetUseTimer);
        }
    }

    private void DeleteAnswers(){
        chosenAnswers=new List<AnswersInfo>();
    }

    private Question GenerateRandomQuestion(){
        var random=GenerateRandomQuestionIdx();
        currentQuestion=random;
        return questions[currentQuestion];
    }

    private int GenerateRandomQuestionIdx(){
        var random=0;

        //if we have more questions to be desplayed
        if(finishedQuestions.Count < questions.Length){
            do{
                random = UnityEngine.Random.Range(0,questions.Length);
            }while(finishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    public void LoadQuestions(){
        //data = Data.Fetch();

        Object[] objects=Resources.LoadAll("QuizQuestions",typeof(Question));
        questions=new Question[objects.Length];
        for(int i=0;i<objects.Length;i++){
            questions[i]=(Question)objects[i];
        }
    }

    void OnEnable(){
        events.updateQuestionAnswer += UpdateAnswers;
    }

    void OnDisable(){
        events.updateQuestionAnswer= UpdateAnswers;
    }

    public void Accept(){
        UpdateTimer(false);
        bool isCorrect=CheckAnswers();
        finishedQuestions.Add(currentQuestion);

        UpdateScore((isCorrect) ? questions[currentQuestion].GetAddScore : -questions[currentQuestion].GetAddScore);

        if(IsFinished){
            SetHighscore();
        }

        //determine type of resolution screen, then display the resolution screen
        var type = (IsFinished)? UIManager.ResolutionScreenType.FINISHED : 
                (isCorrect)? UIManager.ResolutionScreenType.CORRECT : UIManager.ResolutionScreenType.INCORRECT;

        if(events.displayResplutionScreen != null){
            //we will display that score only it is not a finished screen (correct / incorrect)
            events.displayResplutionScreen(type,questions[currentQuestion].GetAddScore);
        }

        AudioManager.audioInstance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");

        if(type != UIManager.ResolutionScreenType.FINISHED){

            //--
            if(IE_wait_round !=null){
                StopCoroutine(IE_wait_round);
            }
            IE_wait_round=WaitUntilNextRound();
            StartCoroutine(IE_wait_round);

        }

        
    }

    private bool CheckAnswers(){
        if(!CompareAnswers()){
            return false;
        }
        return true;
    }

    private bool CompareAnswers(){
        if(chosenAnswers.Count>0){
            List<int> correctAnswersList=questions[currentQuestion].GetCorrectAnswers();
            //Select method from System.Linq
            List<int> pickedAnswers=chosenAnswers.Select(x => x.GetAnswerIndex).ToList();

            //Except (from System.Linq) removes elements except of something
            var first=correctAnswersList.Except(pickedAnswers).ToList();
            var second=pickedAnswers.Except(correctAnswersList).ToList();

            //Any checks if a list contains any element
            return !first.Any() && !second.Any();
        }
        return false;
    }

    public void UpdateAnswers(AnswersInfo newAnswer){
        if(questions[currentQuestion].GetAnswerType == Question.AnswerType.SINGLE){
            foreach (var answer in chosenAnswers)
            {
                if(answer != newAnswer){
                    answer.Reset();
                }
            }

            chosenAnswers.Clear();
            chosenAnswers.Add(newAnswer);
        }else{
            bool alreadyChose=chosenAnswers.Exists(x=>x==newAnswer);
            if(alreadyChose){
                chosenAnswers.Remove(newAnswer);
            }else{
                chosenAnswers.Add(newAnswer);
            }
        }
    }

    private void UpdateScore(int addScore){
        events.currentFinalScore += addScore;

        if(events.scoreUpdated!=null){
            events.scoreUpdated();//delegate
        }
    }

    private void SetHighscore(){
        var high = PlayerPrefs.GetInt(QuizUtility.Save_Player_Pref_Key);
        if(high < events.currentFinalScore){
            PlayerPrefs.SetInt(QuizUtility.Save_Player_Pref_Key, events.currentFinalScore);
        }
    }

    private void UpdateTimer(bool phase){
        switch(phase){
            case true:
                IE_Start_Timer = StartTimer();
                StartCoroutine(IE_Start_Timer);

                timerAnimator.SetInteger(timerStateHash,2);//pop up
                break;
            case false:
                if(IE_Start_Timer != null){
                    StopCoroutine(IE_Start_Timer);
                }
                timerAnimator.SetInteger(timerStateHash,1);//hidden
                break;
        }
    }

    IEnumerator StartTimer(){
        var totalTime = questions[currentQuestion].GetTimer;
        var timeLeft = totalTime;
        timerText.color = timerDefaultColor;

        while(timeLeft > 0){
            timeLeft--;

            AudioManager.audioInstance.PlaySound("CountdownSFX");

            if(timeLeft < totalTime/2 && timeLeft > totalTime/4){
                timerText.color = timerHalfColor;
            }else if(timeLeft < totalTime/4){
                timerText.color = timerAlmostDoneColor;
            }

            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }

        Accept();
    }

    public void RestartQuiz(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitQuiz(){
        Application.Quit();
    }

    //iEnumerator that waits a certain amount of time to change question
    IEnumerator WaitUntilNextRound(){
        yield return new WaitForSeconds(QuizUtility.Resolution_Delay_Time);
        DisplayQuestion();
    }

}
