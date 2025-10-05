// wwwroot/js/interop.js

// Hàm ẩn thanh tiến trình
export function stopProgressBar() {
  NProgress.done();
}

// Lắng nghe click để hiện progress
export function setupNavigationProgressListener() {
  document.body.addEventListener('click', function(event) {
    const anchor = event.target.closest('a');
    if (anchor && anchor.hasAttribute('href') && anchor.href && anchor.target !== '_blank') {
      const url = new URL(anchor.href);
      if (url.origin === location.origin) {
        if (url.pathname === location.pathname && url.hash) return;
        NProgress.start();
      }
    }
  });
}

// In trang
export function printPage() {
  window.print();
}

// === Modal helpers ===
export function showModal(selector) {
  const el = document.querySelector(selector);
  if (!el) return;
  const modal = bootstrap.Modal.getOrCreateInstance(el);
  modal.show();
}

export function hideModal(selector) {
  const el = document.querySelector(selector);
  if (!el) return;
  const modal = bootstrap.Modal.getInstance(el);
  if (modal) modal.hide();
}

// 🔽 GẮN GLOBAL FALLBACK để Blazor có thể gọi dạng JSRuntime.InvokeVoidAsync("showModal", ...)
window.showModal = showModal;
window.hideModal = hideModal;
