// Admin Dashboard - Modern Navigation Component
document.addEventListener("DOMContentLoaded", function () {
  // Sidebar toggle for mobile
  const sidebarToggle = document.querySelector(".sidebar-toggle");
  const sidebar = document.querySelector(".sidebar");

  if (sidebarToggle) {
    sidebarToggle.addEventListener("click", function () {
      sidebar?.classList.toggle("active");
    });
  }

  // Active menu item
  const currentPath = window.location.pathname;
  document.querySelectorAll(".nav-link").forEach((link) => {
    if (link.getAttribute("href") === currentPath) {
      link.classList.add("active");
      link.closest(".nav-item")?.classList.add("active");
    }
  });

  // Notification bell
  const notificationBell = document.querySelector(".notification-bell");
  if (notificationBell) {
    notificationBell.addEventListener("click", function (e) {
      e.preventDefault();
      const dropdown = this.nextElementSibling;
      dropdown?.classList.toggle("show");
    });
  }

  // Close dropdowns when clicking outside
  document.addEventListener("click", function (e) {
    if (!e.target.closest(".navbar-item")) {
      document.querySelectorAll(".dropdown-menu.show").forEach((menu) => {
        menu.classList.remove("show");
      });
    }
  });

  // Dark mode toggle
  const darkModeToggle = document.querySelector(".dark-mode-toggle");
  if (darkModeToggle) {
    darkModeToggle.addEventListener("click", function () {
      document.documentElement.classList.toggle("dark-mode");
      localStorage.setItem(
        "darkMode",
        document.documentElement.classList.contains("dark-mode"),
      );
    });

    // Load saved preference
    if (localStorage.getItem("darkMode") === "true") {
      document.documentElement.classList.add("dark-mode");
    }
  }
});

// Helper function to show toast notifications
function showToast(message, type = "info") {
  const toast = document.createElement("div");
  toast.className = `toast toast-${type}`;
  toast.textContent = message;
  document.body.appendChild(toast);

  setTimeout(() => {
    toast.classList.add("show");
  }, 10);

  setTimeout(() => {
    toast.classList.remove("show");
    setTimeout(() => toast.remove(), 300);
  }, 3000);
}

// Helper function for API calls with error handling
async function apiCall(url, options = {}) {
  try {
    const response = await fetch(url, {
      headers: {
        "Content-Type": "application/json",
        ...options.headers,
      },
      ...options,
    });

    if (!response.ok) {
      throw new Error(`API Error: ${response.statusText}`);
    }

    return await response.json();
  } catch (error) {
    console.error("API Call Error:", error);
    showToast("Lỗi: " + error.message, "error");
    throw error;
  }
}
