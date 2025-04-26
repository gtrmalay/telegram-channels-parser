document.addEventListener('scroll', function () {
    var scrollBtn = document.getElementById('scrollTopBtn');
    if (window.scrollY > 300) { 
        scrollBtn.style.display = 'block'; 
    } else {
        scrollBtn.style.display = 'none'; 
    }
});

document.getElementById('scrollTopBtn').addEventListener('click', function () {
    window.scrollTo({ top: 0, behavior: 'smooth' }); 
});
