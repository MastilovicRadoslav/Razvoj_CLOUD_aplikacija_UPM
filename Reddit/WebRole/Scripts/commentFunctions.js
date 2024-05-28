// JavaScript za prikaz/skrivanje komentara i dodavanje novog komentara
document.addEventListener("DOMContentLoaded", function () {
    var toggleCommentsButtons = document.querySelectorAll('.toggle-comments');
    toggleCommentsButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            var comments = this.nextElementSibling;
            if (comments.style.display === 'none') {
                comments.style.display = 'block';
                this.textContent = '-';
            } else {
                comments.style.display = 'none';
                this.textContent = '+';
            }
        });
    });

    var addCommentButtons = document.querySelectorAll('.add-comment');
    addCommentButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            var commentInput = this.previousElementSibling;
            var commentText = commentInput.value;
            var postId = document.getElementById('postId').value;

            if (commentText.trim() !== '') {
                var xhr = new XMLHttpRequest();
                xhr.open('POST', '/PostPage/NewComment', true);
                xhr.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === 4 && xhr.status === 200) {
                        var response = JSON.parse(xhr.responseText);
                        if (response.success) {
                            var commentContainer = button.parentElement.parentElement;
                            var newComment = document.createElement('div');
                            newComment.classList.add('comment');
                            newComment.textContent = commentText;
                            var newCommentMeta = document.createElement('div');
                            newCommentMeta.classList.add('comment-meta');
                            newCommentMeta.textContent = 'Komentarisao je - ' + response.userEmail;
                            commentContainer.insertBefore(newComment, button.parentElement);
                            commentContainer.insertBefore(newCommentMeta, button.parentElement);
                            commentInput.value = '';
                        }
                    }
                };
                xhr.send(JSON.stringify({ postId: postId, commentText: commentText }));
            }
        });
    });
});