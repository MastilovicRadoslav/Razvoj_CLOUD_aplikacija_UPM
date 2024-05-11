// JavaScript za validaciju polja i potvrdu prijave
function ValidateLogin() {
    const email = document.getElementById("email").value.trim();
    const password = document.getElementById("password").value.trim();

    if (email.length === 0) {
        alert("Polje za email mora biti popunjeno!");
        return false;
    }
    else if (!/^[a-zA-Z0-9@.]*$/.test(email) || !email.includes('@') || !email.includes('.')) {
        alert("Email adresa nije validna!");
        return false;
    }
    else if (password.length === 0) {
        alert("Polje za lozinku mora biti popunjeno!");
        return false;
    }
    else if (password.length < 6) {
        alert("Lozinka nije validna! Lozinka mora imati minimalno 6 karaktera.");
        return false;
    }
    else {
        alert("Prijavili ste se!");
        return true;
    }
}

function ValidateAndSubmit() {
    if (ValidateLogin()) {
        document.querySelector("form").submit();
    }
}