// JavaScript za validaciju polja i potvrdu ažuriranja profila
function ValidateEditProfile() {
    const fname = document.getElementById("fname").value.trim();
    const sname = document.getElementById("lname").value.trim();
    const adress = document.getElementById("adress").value.trim();
    const city = document.getElementById("city").value.trim();
    const state = document.getElementById("country").value.trim();
    const phoneNumber = document.getElementById("phoneNumber").value.trim();
    const email = document.getElementById("email").value.trim();
    const password = document.getElementById("password").value.trim();
    const image = document.getElementById("image").value.trim();

    if (fname.length === 0) {
        alert("Polje za ime mora biti popunjeno!");
        return false;
    }
    else if (!/^[a-zA-Z]*$/.test(fname)) {
        alert("Za polje ime se moraju uneti samo slova!");
        return false;
    }
    else if (sname.length === 0) {
        alert("Polje za prezime mora biti popunjeno!");
        return false;
    }
    else if (!/^[a-zA-Z]*$/.test(sname)) {
        alert("Za polje prezime se moraju uneti samo slova!");
        return false;
    }
    else if (adress.length === 0) {
        alert("Polje za adresu mora biti popunjeno!");
        return false;
    }
    else if (city.length === 0) {
        alert("Polje za grad mora biti popunjeno!");
        return false;
    }
    else if (!/^[a-zA-Z\s]*$/.test(city)) {
        alert("Za polje grad se moraju uneti samo slova!");
        return false;
    }
    else if (state.length === 0) {
        alert("Polje za državu mora biti popunjeno!");
        return false;
    }
    else if (!/^[a-zA-Z\s]*$/.test(state)) {
        alert("Za polje država se moraju uneti samo slova!");
        return false;
    }
    else if (phoneNumber.length === 0) {
        alert("Polje za broj telefona mora biti popunjeno!");
        return false;
    }
    else if (!/^\d+$/.test(phoneNumber)) {
        alert("Broj telefona nije validan! Molimo unesite samo brojeve.");
        return false;
    }
    else if (email.length === 0) {
        alert("Polje za email mora biti popunjeno!");
        return false;
    }
    else if (!/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(email)) {
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
        alert("Izmenili ste profil!");
        return true;
    }
}

function ValidateAndSubmitEditProfile() {
    console.log('tu sam')
    if (ValidateEditProfile()) {
        document.querySelector("form").submit();
    }
}

/* Dinamièko prikazivanje odabrane slike */
function previewImage(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            var previewImg = document.querySelector('img');
            previewImg.src = e.target.result;
            previewImg.style.width = '200px';
            previewImg.style.height = '100px';
        };

        reader.readAsDataURL(input.files[0]);
    }
}