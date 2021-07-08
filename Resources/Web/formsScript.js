    var questionnaire
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
