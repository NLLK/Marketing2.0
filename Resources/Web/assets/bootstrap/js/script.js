function xyi() {
    var questionnaire = [{ "Question": "Вопрос 1", "Scale": 0, "Answers": [{ "Question": "ответ 1", "Scale": -1, "Answers": null, "QuestionNumber": 0, "NextQuestionIfYes": 0 }, { "Question": "ответ 2", "Scale": -1, "Answers": null, "QuestionNumber": 1, "NextQuestionIfYes": 0 }], "QuestionNumber": 1, "NextQuestionIfYes": 0 }]
    console.log(questionnaire[0].Answers[1].Question)
    var q1 = questionnaire[0].Question
    var a1 = questionnaire[0].Answers[0].Question
    var a2 = questionnaire[0].Answers[1].Question
    document.getElementById("test").innerHTML = "<div class=\"form-group\">"
        + "<label for=\"text1\">" + q1 + "</label>"
        + "<div class=\"form-check\">"
        + "<input class=\"form-check-input\" type=\"radio\" name=\"number\" id=\"exampleRadios1\" value=\"option1\" checked>"
        + "<label class=\"form-check-label\" for=\"exampleRadios1\">"
        + a1
        + "</label>"
        + "</div>"
        + "<div class=\"form-check\">"
        + "<input class=\"form-check-input\" type=\"radio\" name=\"number\" id=\"exampleRadios2\" value=\"option2\">"
        + "<label class=\"form-check-label\" for=\"exampleRadios2\">"
        + a2
        + "</label>"
        + "</div>"
}