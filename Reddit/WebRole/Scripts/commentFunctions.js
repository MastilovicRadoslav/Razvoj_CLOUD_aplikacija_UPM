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
            if (commentText.trim() !== '') {
                var commentContainer = this.parentElement.parentElement;
                var newComment = document.createElement('div');
                newComment.classList.add('comment');
                newComment.textContent = commentText;
                var newCommentMeta = document.createElement('div');
                newCommentMeta.classList.add('comment-meta');
                newCommentMeta.textContent = 'Komentarisao je - User';
                commentContainer.insertBefore(newComment, this.parentElement);
                commentContainer.insertBefore(newCommentMeta, this.parentElement);
                commentInput.value = '';
            }
        });
    });
});