document.addEventListener("DOMContentLoaded", function () {
    const questions = {
        "/Login/Login": [
            { question: "Kako se registrovati?", answer: "Da biste se registrovali, kliknite na 'Registration' link u navigaciji." },
            { question: "Kako se prijaviti?", answer: "Unesite vaš email i lozinku, a zatim kliknite na 'Prijavi se' dugme." }
        ],
        "/Registration/Registration": [
            { question: "Kako se registrovati?", answer: "Da biste se registrovali, morate popuniti sva obavezna polja i ispoštovati sva ograničenja za unos, zatim kliknite na dugme 'Registration'." },
            { question: "Koja su obavezna polja?", answer: "Sva polja su obavezna, jedino polje koje nije obavezno je 'Slika'." }
        ],
        "/Profile/ShowProfile": [
            { question: "Kako izmeniti profil?", answer: "Popunite nove vrednosti za polja koja želite, zatim kliknite na 'Izmeni profil' dugme na vašoj profil stranici, takođe vodite računa o obaveznim poljima." },
            { question: "Kako dodati sliku profila?", answer: "Kliknite na 'Dodaj sliku' dugme i izaberite sliku sa vašeg računara." }
        ],
        "/PostPage/PostPage": [
            { question: "Kako lajkovati post?", answer: "Kliknite na 'Like' dugme ispod posta koji želite da lajkujete." },
            { question: "Kako lajkovati komentar?", answer: "Kliknite na '+' ikonicu da prikažete komentar a zatim na 'Like' dugme ispod komentara." },
            { question: "Kako komentarisati post?", answer: "Kliknite na ikonicu '+' ispod posta i tu unesite u prazno polje odgovarajući komentar." }
        ],
        "/": [
            { question: "Kako kreirati novi post?", answer: "Kliknite na 'Dodavanje novog posta' dugme i popunite potrebne informacije." },
            { question: "Kako obrisati post?", answer: "Kliknite na ikonicu smeće pored posta koji želite da obrišete." },
            { question: "Kako sortirati postove?", answer: "Iz padajuće liste odaberite 'Naziv posta'." },
            { question: "Kako pretražiti postove?", answer: "U polje pretraga unesite odgovarajuće ime." },
            { question: "Kako dodati neki post u favorite?", answer: "Kliknite na ikonicu zvezdice pored posta koji želite da favorizujete." },
            { question: "Gde da lajkujem, komentarišem i vidim detalje o nekom postu?", answer: "Kliknite na odgovarajući post iz liste 'Sve teme'." }
        ]
    };

    const path = window.location.pathname;
    const questionsList = document.getElementById("questions-list");

    if (questions[path]) {
        questions[path].forEach(function (q) {
            const li = document.createElement("li");
            li.textContent = q.question;
            li.onclick = function () {
                showAnswer(q.answer);
            };
            questionsList.appendChild(li);
        });
    } else {
        const li = document.createElement("li");
        li.textContent = "Nema dostupnih pitanja za ovu stranicu.";
        questionsList.appendChild(li);
    }

    function showAnswer(answer) {
        const messages = document.getElementById("chatbot-messages");

        // Prikaz bot odgovora
        const botMessage = document.createElement("div");
        botMessage.className = "message bot-message";
        botMessage.textContent = answer;
        messages.appendChild(botMessage);

        // Skrolovanje na najnoviju poruku
        messages.scrollTop = messages.scrollHeight;
    }

    // Osnovna funkcionalnost za chatbot
    window.sendMessage = function () {
        var input = document.getElementById("chatbot-input").value;
        if (input.trim() === "") return;

        var messages = document.getElementById("chatbot-messages");

        // Prikaz korisničke poruke
        var userMessage = document.createElement("div");
        userMessage.className = "message user-message";
        userMessage.textContent = input;
        messages.appendChild(userMessage);

        // Prikaz bot odgovora
        var botMessage = document.createElement("div");
        botMessage.className = "message bot-message";
        botMessage.textContent = getBotResponse(input);
        messages.appendChild(botMessage);

        // Resetovanje input polja
        document.getElementById("chatbot-input").value = "";

        // Skrolovanje na najnoviju poruku
        messages.scrollTop = messages.scrollHeight;
    };

    // Funkcija koja vraća odgovore bota
    function getBotResponse(input) {
        const responses = {
            "Kako se registrovati?": "Da biste se registrovali, morate popuniti sva obavezna polja i ispoštovati sva ograničenja za unos, zatim kliknite na dugme 'Registration'.",
            "Kako se prijaviti?": "Unesite vaš email i lozinku, a zatim kliknite na 'Prijavi se' dugme.",
            "Koja su obavezna polja?": "Sva polja su obavezna, jedino polje koje nije obavezno je 'Slika'.",
            "Kako izmeniti profil?": "Popunite nove vrednosti za polja koja želite, zatim kliknite na 'Izmeni profil' dugme na vašoj profil stranici, takođe vodite računa o obaveznim poljima.",
            "Kako dodati sliku profila?": "Kliknite na 'Dodaj sliku' dugme i izaberite sliku sa vašeg računara.",
            "Kako lajkovati post?": "Kliknite na 'Like' dugme ispod posta koji želite da lajkujete.",
            "Kako lajkovati komentar?": "Kliknite na '+' ikonicu da prikažete komentar, a zatim na 'Like' dugme ispod komentara.",
            "Kako komentarisati post?": "Kliknite na ikonicu '+' ispod posta i unesite komentar u prazno polje.",
            "Kako kreirati novi post?": "Kliknite na 'Dodavanje novog posta' dugme i popunite potrebne informacije.",
            "Kako obrisati post?": "Kliknite na ikonicu smeće pored posta koji želite da obrišete.",
            "Kako sortirati postove?": "Iz padajuće liste odaberite 'Naziv posta'.",
            "Kako pretražiti postove?": "U polje pretraga unesite odgovarajuće ime.",
            "Kako dodati neki post u favorite?": "Kliknite na ikonicu zvezdice pored posta koji želite da favorizujete.",
            "Gde da lajkujem, komentarišem i vidim detalje o nekom postu?": "Kliknite na odgovarajući post iz liste 'Sve teme'."
        };

        // Normalizacija unosa
        const normalizedInput = input.trim().toLowerCase();

        // Provera da li postoji odgovor za dati unos
        for (const [question, answer] of Object.entries(responses)) {
            if (question.toLowerCase() === normalizedInput) {
                return answer;
            }
        }

        if (normalizedInput === "zdravo") {
            return "Zdravo! Kako vam mogu pomoći?";
        } else {
            return "Izvinite, ne razumem vaše pitanje.";
        }
    }

    // Funkcija za brisanje svih poruka
    window.clearAllMessages = function () {
        document.getElementById("chatbot-messages").innerHTML = "";
    };
});
