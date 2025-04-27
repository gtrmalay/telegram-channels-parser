document.addEventListener('DOMContentLoaded', () => {
    const btn = document.getElementById('startImportBtn');
    if (!btn) return;

    btn.addEventListener('click', async function () {
        btn.disabled = true;
        btn.innerHTML = '<span class="spinner-border spinner-border-sm"></span> Импорт...';

        try {
            const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
            const token = tokenInput ? tokenInput.value : '';

            const response = await fetch('/api/import-news', {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': token
                }
            });

            if (response.ok) {
                alert('Импорт успешно запущен');
                location.reload();
            } else {
                const error = await response.text();
                alert('Ошибка: ' + error);
            }
        } catch (error) {
            alert('Ошибка сети: ' + error.message);
        } finally {
            btn.disabled = false;
            btn.innerHTML = '<i class="bi bi-cloud-download"></i> Запустить импорт';
        }
    });
});
