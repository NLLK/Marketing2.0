window.onload = function () {
    var questionnaire = [{ "Question": "Вопрос 1", "Scale": 0, "Answers": [{ "Question": "ответ 1", "Scale": -1, "Answers": null, "QuestionNumber": 0, "NextQuestionIfYes": 0 }, { "Question": "ответ 2", "Scale": -1, "Answers": null, "QuestionNumber": 1, "NextQuestionIfYes": 0 }], "QuestionNumber": 1, "NextQuestionIfYes": 0 }, { "Question": "Вопрос 2", "Scale": 0, "Answers": [{ "Question": "ответ 1", "Scale": -1, "Answers": null, "QuestionNumber": 0, "NextQuestionIfYes": 0 }, { "Question": "ответ 2", "Scale": -1, "Answers": null, "QuestionNumber": 1, "NextQuestionIfYes": 0 }], "QuestionNumber": 2, "NextQuestionIfYes": 0 }]
    var mainNode = document.getElementById("test")
    questionnaire.forEach(question => {
        let questionDiv = document.createElement('div')
        questionDiv.id = "question" + question.QuestionNumber
        mainNode.append(questionDiv)
        let questionLabel = document.createElement('label')
        questionLabel.innerText = question.QuestionNumber + ". " + question.Question
        questionDiv.append(questionLabel)
        let answersDiv = document.createElement('div')
        answersDiv.id = "question" + question.QuestionNumber + "answers"
        answersDiv.className = "form-check"
        questionDiv.append(answersDiv)

        question.Answers.forEach(answer => {
            let answersInput = document.createElement('input')
            answersInput.id = answer.QuestionNumber
            answersInput.type = "radio"
            answersInput.name = "radioName" + question.QuestionNumber
            answersInput.className = "form-check-input"
            answersDiv.append(answersInput)
            //answersInput.innerText = answer.QuestionNumber + ". " + answer.Question
            let answersLabel = document.createElement('label')
            answersLabel.className = "form-check-label"

            answersLabel.innerText = answer.Question
            answersDiv.append(answersLabel)

            let br = document.createElement('br')
            answersDiv.append(br)
        });
        CefSharp.PostMessage("windowLoaded")

    });
}
function initForm() { }
