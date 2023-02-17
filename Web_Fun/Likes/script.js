function addLikes(num) {
    var likes = document.querySelector(`#likes_${num}`);
    var likes_num = parseInt(likes.textContent);
    likes.textContent = likes_num + 1;
}