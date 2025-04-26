document.addEventListener('DOMContentLoaded', () => {
    const scrollTopBtn = document.getElementById('scrollTopBtn');
    if (scrollTopBtn) {
        scrollTopBtn.addEventListener('click', () => {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }

    // Автообновление страницы каждые 5 минут
    setTimeout(() => location.reload(), 300000);
});
