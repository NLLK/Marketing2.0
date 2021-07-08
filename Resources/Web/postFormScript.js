function postForm()
{
    var answers = new Array();
    var inputs = document.getElementsByTagName("input")
    
    for (let item of inputs)
    {
        var answer = 
        {
            QuestionNumber: "", 
            Value: ""
        }
    
        answer.QuestionNumber = item.id
        if (item.checked == true)
            answer.Value = 1
        else
            answer.Value = 0  
        answers.push(answer)
    }

    window.location.href = '#questionnaireName'

    CefSharp.PostMessage(JSON.stringify(answers))    
} 