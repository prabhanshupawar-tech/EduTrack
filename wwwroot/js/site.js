// EduTrack global client-side script
// Handles: SweetAlert2 toast notifications for TempData messages, and a loading spinner on form submit.

document.addEventListener('DOMContentLoaded', function () {
    // ---- Toast notifications driven by TempData (see _Notifications.cshtml) ----
    const toastEl = document.getElementById('toast-data');
    if (toastEl) {
        const type = toastEl.dataset.type;
        const message = toastEl.dataset.message;

        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3500,
            timerProgressBar: true
        });

        Toast.fire({
            icon: type === 'success' ? 'success' : 'error',
            title: message
        });
    }

    // ---- Simple loading spinner overlay while forms submit ----
    const overlay = document.createElement('div');
    overlay.className = 'spinner-overlay';
    overlay.innerHTML = '<div class="spinner-border text-primary" style="width:3rem;height:3rem;" role="status"><span class="visually-hidden">Loading...</span></div>';
    document.body.appendChild(overlay);

    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function () {
            // Only show spinner if the browser-native validation passes
            if (form.checkValidity ? form.checkValidity() : true) {
                overlay.classList.add('active');
            }
        });
    });
});
