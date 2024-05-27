// JavaScript za validaciju i dodavanje posta 
function ValidatePost() {
    const title = document.getElementById("postTitleModal").value.trim();
    const description = document.getElementById("postDescriptionModal").value.trim();

    if (title.length === 0) {
        alert("Tema posta mora biti popunjena!");
        return false;
    }
    else if (description.length === 0) {
        alert("Opis posta mora biti popunjena!");
        return false;
    }
    else {
        alert("Dodali ste novi post!");
        return true;
    }
}

function ValidateAndSubmitPost() {
    if (ValidatePost()) {
        document.querySelector("form").submit();
    }
}