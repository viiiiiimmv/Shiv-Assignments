function validate(){
    if (document.myForm.Name.value == "") {
        alert("Please enter your name!");
        document.myForm.Name.focus();
        return false;
    }
    if (document.myForm.Email.value == "") {
        alert("Please enter your email!");
        document.myForm.Email.focus();
        return false;
    }
    if (document.myForm.Zip.value == "" && document.myForm.Zip.value.length != 6) {
        alert("Please enter a valid zip code!");
        document.myForm.Zip.focus();
        return false;
    }
    if (document.myForm.Country.value == "-1") {
        alert("Please select your country!");
        document.myForm.Country.focus();
        return false;
    }
    return true;
}

function validateRegex(){
    var email = document.getElementById("email").value;
    var regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    if(regex.test(email)){
        document.getElementById("result").innerHTML = "Valid Email";
        document.getElementById("result").style.color = "green";
    }else{
        document.getElementById("result").innerHTML = "Invalid Email";
        document.getElementById("result").style.color = "red";
    }
}