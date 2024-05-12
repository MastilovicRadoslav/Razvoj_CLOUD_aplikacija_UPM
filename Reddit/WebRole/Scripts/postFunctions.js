﻿// JavaScript funkcija za like/unlike posta
document.addEventListener("DOMContentLoaded", function () {
    const likeCounts = [];
    const unlikeCounts = [];
    const likeButtons = document.querySelectorAll('.like');
    const unlikeButtons = document.querySelectorAll('.unlike');
    const likeCountDisplays = document.querySelectorAll('.like-count');
    const unlikeCountDisplays = document.querySelectorAll('.unlike-count');

    likeButtons.forEach((button, index) => {
        likeCounts[index] = 0;

        button.addEventListener('click', () => {
            likeCounts[index]++;
            likeCountDisplays[index].textContent = likeCounts[index];
        });
    });

    unlikeButtons.forEach((button, index) => {
        unlikeCounts[index] = 0;

        button.addEventListener('click', () => {
            unlikeCounts[index]++;
            unlikeCountDisplays[index].textContent = unlikeCounts[index];
        });
    });
});

// JavaScript funkcija za brisanje posta
function deletePost() {
    alert('Obrisan');
}

// JavaScript funkcija za dodavanje posta u favorite
var isFirstClick = true;
function addToFavorites(element) {
    var title = element.parentElement;
    var originalTitle = title.textContent.trim();
    var printTitle = originalTitle.replace('★', '').trim();

    if (element.style.color === 'black' || isFirstClick) {
        element.style.color = 'yellow';
        alert("Post sa naslovom '" + printTitle + "' ste dodali u favorite");
        isFirstClick = false;
    } else {
        element.style.color = 'black';
        alert("Post sa naslovom '" + printTitle + "' ste uklonili iz favorita");
    }
}

// JavaScript funkcija za prikaz slike kod dodavanja novog posta
function showImage(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var previewImg = document.getElementById('newPostImage');
            previewImg.src = e.target.result;
            previewImg.style.width = '200px';
            previewImg.style.height = '100px';
        };
        reader.readAsDataURL(input.files[0]);
    }
}

// JavaScript funkcija koja obrađuje klik na naslov nekog od postova
function handleTitleClick(title) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/Home/OpenPost", true);
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE) {
            if (xhr.status === 200) {
                var response = JSON.parse(xhr.responseText);
                if (response.success) {
                    window.location.href = '/PostPage/PostPage?postTitle=' + encodeURIComponent(title);
                } else {
                    console.error("Došlo je do greške!");
                }
            } else {
                console.error("Došlo je do greške!");
            }
        }
    };
    xhr.send(JSON.stringify({ postTitle: title }));
}