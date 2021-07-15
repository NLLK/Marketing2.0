
    var questionnaire
    var mainNode = document.getElementById("test")
    questionnaire.forEach(question => {
        addQuestion(mainNode,question)
        question.Answers.forEach(ans => {
            if (ans.Answers.length != 0)
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
        if (answer.Answers.length == 0)
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