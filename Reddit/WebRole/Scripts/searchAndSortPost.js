// JavaScript za pretragu i sortiranje postova
function sendPostRequest() {
    var sortOption = $('#sortSelect').val();
    $.ajax({
        type: "POST",
        url: '/Home/PostSort',
        data: { sortOption: sortOption },
        success: function (response) {
            var postsContainer = $('#allPostsContainer');
            postsContainer.empty();
            $.each(response, function (index, post) {
                postsContainer.append(
                    '<div class="post">' +
                    '<h2><button class="hidden-button" onclick="handleTitleClick(\'' + post.Id + '\')" id="' + post.Id + '">' + post.Title + '</button> <span class="add-to-favorites favorite-star" onclick="addToFavorites(this)">★</span></h2>' +
                    '</div>'
                );
            });
        },
        error: function (xhr, status, error) {
            console.error("Error: " + status + " - " + error);
        }
    });
}

function searchText() {
    var searchText = $('#searchInput').val().trim();
    $.ajax({
        type: "POST",
        url: '/Home/PostSearch',
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify({ searchText: searchText }),
        success: function (response) {
            var postsContainer = $('#allPostsContainer');
            postsContainer.empty();
            $.each(response, function (index, post) {
                postsContainer.append(
                    '<div class="post">' +
                    '<h2><button class="hidden-button" onclick="handleTitleClick(\'' + post.Id + '\')" id="' + post.Id + '">' + post.Title + '</button> <span class="add-to-favorites favorite-star" onclick="addToFavorites(this)">★</span></h2>' +
                    '</div>'
                );
            });
        },
        error: function (xhr, status, error) {
            console.error("Error: " + status + " - " + error);
        }
    });
}
