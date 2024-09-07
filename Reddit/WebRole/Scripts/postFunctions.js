document.addEventListener("DOMContentLoaded", function () {
    const likeCounts = [];
    const unlikeCounts = [];
    const likeButtons = document.querySelectorAll('.like');
    const unlikeButtons = document.querySelectorAll('.unlike');
    const likeCountDisplays = document.querySelectorAll('.like-count');
    const unlikeCountDisplays = document.querySelectorAll('.unlike-count');

    likeButtons.forEach((button, index) => {
        likeCounts[index] = parseInt(likeCountDisplays[index].textContent);

        button.addEventListener('click', () => {
            const postId = button.getAttribute('data-post-id');
            if (button.disabled) return;

            likeCounts[index]++;
            likeCountDisplays[index].textContent = likeCounts[index];
            button.disabled = true;
            unlikeButtons[index].disabled = true;

            fetch('/PostPage/LikePost', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ postId: postId })
            }).then(response => response.json()).then(data => {
                if (data.success) {
                    likeCountDisplays[index].textContent = data.newLikeCount;
                }
            });
        });
    });

    unlikeButtons.forEach((button, index) => {
        unlikeCounts[index] = parseInt(unlikeCountDisplays[index].textContent);

        button.addEventListener('click', () => {
            const postId = button.getAttribute('data-post-id');
            if (button.disabled) return;

            unlikeCounts[index]++;
            unlikeCountDisplays[index].textContent = unlikeCounts[index];
            button.disabled = true;
            likeButtons[index].disabled = true;

            fetch('/PostPage/UnlikePost', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ postId: postId })
            }).then(response => response.json()).then(data => {
                if (data.success) {
                    unlikeCountDisplays[index].textContent = data.newUnlikeCount;
                }
            });
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const likeCountsComments = [];
    const unlikeCountsComments = [];
    const likeCommentButtons = document.querySelectorAll('.like-comment');
    const unlikeCommentButtons = document.querySelectorAll('.unlike-comment');
    const likeCountDisplays = document.querySelectorAll('.like-count-comment');
    const unlikeCountDisplays = document.querySelectorAll('.unlike-count-comment');

    likeCommentButtons.forEach((button, index) => {
        likeCountsComments[index] = parseInt(likeCountDisplays[index].textContent);

        button.addEventListener('click', () => {
            const commentId = button.getAttribute('data-comment-id');
            if (button.disabled) return;

            likeCountsComments[index]++;
            likeCountDisplays[index].textContent = likeCountsComments[index];
            button.disabled = true;
            unlikeCommentButtons[index].disabled = true;

            fetch('/PostPage/LikeComment', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ commentId: commentId })
            }).then(response => response.json()).then(data => {
                if (data.success) {
                    likeCountDisplays[index].textContent = data.newLikeCount;
                }
            });
        });
    });

    unlikeCommentButtons.forEach((button, index) => {
        unlikeCountsComments[index] = parseInt(unlikeCountDisplays[index].textContent);

        button.addEventListener('click', () => {
            const commentId = button.getAttribute('data-comment-id');
            if (button.disabled) return;

            unlikeCountsComments[index]++;
            unlikeCountDisplays[index].textContent = unlikeCountsComments[index];
            button.disabled = true;
            likeCommentButtons[index].disabled = true;

            fetch('/PostPage/UnlikeComment', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ commentId: commentId })
            }).then(response => response.json()).then(data => {
                if (data.success) {
                    unlikeCountDisplays[index].textContent = data.newUnlikeCount;
                }
            });
        });
    });
});


// JavaScript funkcija za brisanje posta
function deletePost(postId) {
    $.ajax({
        url: '/Home/DeletePost',
        type: 'POST',
        data: { postId: postId }
    });
    alert('Obrisali ste post!');
    window.location.reload();
}

// JavaScript funkcija za dodavanje posta u favorite
var isFirstClick = true;
function addToFavorites(element, postId) {
    if (element.style.color === 'black' || isFirstClick) {
        element.style.color = 'yellow';
        isFirstClick = false;

        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/Home/AddToFavorites", true);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        var data = JSON.stringify({ postId: postId });
        xhr.send(data);
        alert("Post ste dodali u favorite!");
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
function handleTitleClick(id) {
    var xhr = new XMLHttpRequest();
    console.log(id)
    xhr.open("POST", "/Home/OpenPost", true);
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE) {
            if (xhr.status === 200) {
                var response = JSON.parse(xhr.responseText);
                if (response.success) {
                    window.location.href = '/PostPage/PostPage?postId=' + encodeURIComponent(id);
                } else {
                    console.error("Došlo je do greške!");
                }
            } else {
                console.error("Došlo je do greške!");
            }
        }
    };
    xhr.send(JSON.stringify({ postId: id }));
}