document.addEventListener('DOMContentLoaded', () => {
    const btn = document.getElementById('startImportBtn');
    if (!btn) return;

    btn.addEventListener('click', async function () {
        btn.disabled = true;
        btn.innerHTML = '<span class="spinner-border spinner-border-sm"></span> Импорт...';

        try {
            const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
            const token = tokenInput ? tokenInput.value : '';

            console.log('Отправка запроса на /api/parser/import-news');

            const response = await fetch('/api/parser/import-news', {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': token
                }
            });

            console.log('Ответ получен', response);

            if (response.ok) {
                alert('Импорт успешно запущен');
                location.reload();
            } else {
                const error = await response.text();
                console.error('Ошибка при получении данных: ', error);
                alert('Ошибка: ' + error);
            }
        } catch (error) {
            console.error('Ошибка сети: ', error);
            alert('Ошибка сети: ' + error.message);
        } finally {
            btn.disabled = false;
            btn.innerHTML = '<i class="bi bi-cloud-download"></i> Запустить импорт';
        }
    });
});
