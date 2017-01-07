﻿$(document).ready(function () {
    $("input[type=radio][name=radioA]").change(function () {
        if (this.value == 'choice') {
            document.getElementById("nrCharactersAnswerDiv").style.display = 'none';
            document.getElementById("answers").style.display = 'block';
            document.getElementById("buttonAddAnswer").style.display = 'block';
        }
        else if (this.value == 'text') {
            document.getElementById("nrCharactersAnswerDiv").style.display = 'block';
            document.getElementById("answers").style.display = 'none';
            document.getElementById("buttonAddAnswer").style.display = 'none';
        }
    });
});

function addAnswer() {
    var answers = document.getElementById('answers');
    var div = document.createElement('div');
    div.setAttribute('class', "form-group row");

    var label = document.createElement('label');
    label.setAttribute('class', "col-sm-2 col-form-label");
    label.appendChild(document.createTextNode("Answer option " + (answers.childElementCount+1)+":"));

    var div2 = document.createElement('div');
    div2.setAttribute('class', "col-sm-10");

    var input = document.createElement('input');
    input.setAttribute('class', "form-control");
    input.setAttribute('type', "text");
    input.setAttribute('placeholder', "Enter answer");

    div2.appendChild(input);
    div.appendChild(label);
    div.appendChild(div2);
    answers.appendChild(div);
}

function validateQ() {
    var obj1 = document.getElementById('fastQuestion');
    if (obj1.value == '') {
        document.getElementById('err_q').appendChild(document.createTextNode("The Question field is required"));
        obj1.focus();
        return false;
    } else {
        if (document.getElementById('err_q').firstChild)
        document.getElementById('err_q').removeChild(document.getElementById('err_q').firstChild);
        submitQ()
        return true;
    }
}

function submitQ() {
    var quesion = document.getElementById('fastQuestion');
    var types = document.getElementsByName('radioA');
    var sel_type;
    for (var i = 0; i < types.length; i++) {
        if (types[i].checked) {
            if (types[i].value == 'text')
                sel_type = 0;
            else {
                sel_type = 1
            }
        }
    }
    var nr;
    if (sel_type==0) {
        nr = document.getElementById('nrCharactersAnswer').value;
    } else {
        nr = document.getElementById('answers').childElementCount;
    }

    
    var data = {
        QuestionText: quesion.value,
        Type: sel_type,
        Number : nr,
        Active : 1
    }

    $.ajax({
        type: 'POST',
        url: '/FastQuestion/Create',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (result) {
            console.log('Data received: ');
            console.log(result);
        }
    });
    

}