function postForm() {
    var answers = new Array();
    var inputs = document.getElementById("test").getElementsByTagName("input")

    var questionnaire = JSON.parse(document.getElementById("questionnaireHidden").innerText)

    for (let item of inputs) {
        var answerNumber = item.id
        var value = checkedToInt(item.checked)
        var splitted = answerNumber.split(".")
        var questionNumber = parseInt(splitted[0]) - 1;
        splitted = cutArrayByOne(splitted)
        writeInObject(splitted, value, questionnaire[questionNumber])

    }
    window.location.href = '#questionnaireName'

    CefSharp.PostMessage(JSON.stringify(questionnaire))
}
function cutArrayByOne(array) {
    var cuttedArray = new Array();
    var i = 0
    array.forEach(element => {
        if (i != 0) cuttedArray.push(element)
        i++
    });
    return cuttedArray;
}
function writeInObject(splitted, value, question) {
    if (splitted.length == 1) {
        var answerNumber = parseInt(splitted[0]) - 1
        question.Answers[answerNumber].Value = value
    }
    else {
        var questionNumber = parseInt(splitted[0]) - 1;
        splitted = cutArrayByOne(splitted)
        writeInObject(splitted, value, question.Answers[questionNumber])
    }
}
function checkedToInt(checked) {
    if (checked) return "1";
    else return "0";
}