window.onload = function () {
    var questionnaire =
    [
        {
          "Question": "Скажите пожалуйста, Ваша компания осуществляет работы на дорогах I или II категории?",
          "Scale": 0,
          "Answers": [
            {
              "Question": "Нет - А другие виды дорожных работ, требующих высокой точности?",
              "Scale": -1,
              "Answers": [
                {
                  "Question": "Нет - то есть, автоматические системы управления системы, или системы нивелирования в Вашей комании не применяются и не нужны?",
                  "Scale": -1,
                  "Answers": [
                    {
                      "Question": "Не нужны - (Спасибо за уделенное время)",
                      "Scale": -1,
                      "Answers": null,
                      "QuestionNumber": "1.1.1.1",
                      "NextQuestionIfYes": 0,
                      "Value": ""
                    },
                    {
                      "Question": "Нужны (переход ко второму вопросу)",
                      "Scale": -1,
                      "Answers": null,
                      "QuestionNumber": "1.1.1.2",
                      "NextQuestionIfYes": 2,
                      "Value": ""
                    }
                  ],
                  "QuestionNumber": "1.1.1",
                  "NextQuestionIfYes": 0,
                  "Value": ""
                },
                {
                  "Question": "Да - Уточните, пожалуйста, какие?",
                  "Scale": -1,
                  "Answers": null,
                  "QuestionNumber": "1.1.2",
                  "NextQuestionIfYes": 2,
                  "Value": ""
                }
              ],
              "QuestionNumber": "1.1",
              "NextQuestionIfYes": 0,
              "Value": ""
            },
            {
              "Question": "Да (переход ко второму вопросу)",
              "Scale": -1,
              "Answers": null,
              "QuestionNumber": "1.2",
              "NextQuestionIfYes": 2,
              "Value": ""
            }
          ],
          "QuestionNumber": "1",
          "NextQuestionIfYes": 0,
          "Value": ""
        }
      ]
    var mainNode = document.getElementById("test")
    questionnaire.forEach(question => {
        addQuestion(mainNode,question)
        question.Answers.forEach(ans => {
            if (ans.Answers != null)
            {
                disableNode(ans)
            }
        });        
    });

    let questionnaireHiddenDiv = document.createElement('div')
    questionnaireHiddenDiv.id = "questionnaireHidden"
    questionnaireHiddenDiv.hidden = true
    questionnaireHiddenDiv.innerText = JSON.stringify(questionnaire)
    mainNode.append(questionnaireHiddenDiv)
    //CefSharp.PostMessage("windowLoaded")
}
function addQuestion(mainNode, question)
{   
    //MainNode of a question
    let questionDiv = document.createElement('div')
    questionDiv.id = "question" + question.QuestionNumber
    mainNode.append(questionDiv)

    //label of question
    let questionLabel = document.createElement('label')
    questionLabel.innerText = question.QuestionNumber + ". " + question.Question
    questionDiv.append(questionLabel)

    //node for answers
    let answersDiv = document.createElement('div')
    answersDiv.id = "question" + question.QuestionNumber + "answers"
    answersDiv.className = "form-check"
    questionDiv.append(answersDiv)

    //for indication
    var qNumber = 0;
    //for all answers in dedicated array
    question.Answers.forEach(answer => 
    {
        //if question have not any children
        if (answer.Answers == null)
        {
            //radiobutton of answer
            //TODO: textbox if needed
            let answersInput = document.createElement('input')
            answersInput.id = answer.QuestionNumber
            answersInput.type = "radio"
            answersInput.name = "radioName" + question.QuestionNumber
            answersInput.className = "form-check-input"

            answersDiv.append(answersInput)

            //radiobutton label
            let answersLabel = document.createElement('label')
            answersLabel.className = "form-check-label"
            answersLabel.innerText = answer.Question
            answersDiv.append(answersLabel)
            
            //new row
            let br = document.createElement('br')
            answersDiv.append(br)
        }
        else
        {
            //radiobutton of answer
            let answersInput = document.createElement('input')
            answersInput.id = answer.QuestionNumber
            answersInput.type = "radio"
            answersInput.name = "radioName" + question.QuestionNumber
            answersInput.className = "form-check-input"
            answersDiv.append(answersInput)

            // if (qNumber == 0)
            // {
            //     answersInput.disabled = true
            //     qNumber++;
            // }

            addQuestion(answersDiv, answer)
        } 
    });
}
function disableNode(answers)
{
    //input.disable = true для дочерних 
    console.log(answers.QuestionNumber)
}