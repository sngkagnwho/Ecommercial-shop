// Admin Dashboard Helper Functions

// Notification Toast
function showToast(message, type = 'success') {
    const alertClass = `alert-${type}`;
    const iconClass = type === 'success' ? 'fa-check-circle' : 'fa-exclamation-circle';
    
    const alertHtml = `
        <div class="alert ${alertClass} alert-dismissible fade show" role="alert">
            <i class="fas ${iconClass}"></i> ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;
    
    const container = document.createElement('div');
    container.className = 'position-fixed top-0 start-50 translate-middle-x mt-3';
    container.style.zIndex = '9999';
    container.innerHTML = alertHtml;
    document.body.appendChild(container);
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        container.remove();
    }, 5000);
}

// Format Currency
function formatCurrency(value, currency = 'USD') {
    const formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: currency,
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
    return formatter.format(value);
}

// Format Date
function formatDate(dateString, format = 'MMM dd, yyyy') {
    const date = new Date(dateString);
    const options = {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    };
    return date.toLocaleDateString('en-US', options);
}

// Confirm Dialog
function confirmDialog(message, callback) {
    if (confirm(message)) {
        callback();
    }
}

// Table Search
function filterTable(tableId, searchInputId) {
    const input = document.getElementById(searchInputId);
    const table = document.getElementById(tableId);
    const rows = table.querySelectorAll('tbody tr');
    
    input.addEventListener('keyup', function() {
        const searchTerm = input.value.toLowerCase();
        
        rows.forEach(row => {
            const text = row.textContent.toLowerCase();
            row.style.display = text.includes(searchTerm) ? '' : 'none';
        });
    });
}

// Export Table to CSV
function exportTableToCSV(tableId, filename = 'export.csv') {
    const table = document.getElementById(tableId);
    let csv = [];
    
    // Get headers
    const headers = [];
    table.querySelectorAll('thead th').forEach(th => {
        headers.push(th.textContent.trim());
    });
    csv.push(headers.join(','));
    
    // Get rows
    table.querySelectorAll('tbody tr').forEach(tr => {
        const row = [];
        tr.querySelectorAll('td').forEach(td => {
            row.push('"' + td.textContent.trim().replace(/"/g, '""') + '"');
        });
        csv.push(row.join(','));
    });
    
    // Download
    const csvContent = csv.join('\n');
    const blob = new Blob([csvContent], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();
    window.URL.revokeObjectURL(url);
}

// Print Table
function printTable(tableId) {
    const table = document.getElementById(tableId).outerHTML;
    const newWindow = window.open('', '', 'height=400,width=800');
    newWindow.document.write('<html><head><title>Print</title>');
    newWindow.document.write('<link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css">');
    newWindow.document.write('</head><body >');
    newWindow.document.write(table);
    newWindow.document.write('</body></html>');
    newWindow.document.close();
    newWindow.print();
}

// Form Validation
function validateForm(formId) {
    const form = document.getElementById(formId);
    const inputs = form.querySelectorAll('input[required], textarea[required], select[required]');
    let isValid = true;
    
    inputs.forEach(input => {
        if (!input.value.trim()) {
            input.classList.add('is-invalid');
            isValid = false;
        } else {
            input.classList.remove('is-invalid');
        }
    });
    
    return isValid;
}

// Debounce Function
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// API Request Helper
async function apiRequest(url, method = 'GET', data = null) {
    const options = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        }
    };
    
    if (data) {
        options.body = JSON.stringify(data);
    }
    
    try {
        const response = await fetch(url, options);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        showToast('An error occurred', 'danger');
        return null;
    }
}

// Initialize Tooltips
function initializeTooltips() {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Initialize Popovers
function initializePopovers() {
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
}

// Show Loading Spinner
function showSpinner(buttonId) {
    const button = document.getElementById(buttonId);
    button.disabled = true;
    button.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Loading...';
}

// Hide Loading Spinner
function hideSpinner(buttonId, originalText) {
    const button = document.getElementById(buttonId);
    button.disabled = false;
    button.innerHTML = originalText;
}

// Format File Size
function formatFileSize(bytes) {
    if (bytes === 0) return '0 Bytes';
    
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
}

// Copy to Clipboard
function copyToClipboard(text, messageId = null) {
    navigator.clipboard.writeText(text).then(function() {
        if (messageId) {
            const element = document.getElementById(messageId);
            const originalText = element.textContent;
            element.textContent = 'Copied!';
            setTimeout(() => {
                element.textContent = originalText;
            }, 2000);
        }
    });
}

// Initialize on Document Ready
document.addEventListener('DOMContentLoaded', function() {
    // Initialize tooltips
    initializeTooltips();
    
    // Initialize popovers
    initializePopovers();
    
    // Remove validation classes on input
    document.querySelectorAll('input, textarea, select').forEach(element => {
        element.addEventListener('change', function() {
            if (this.value.trim()) {
                this.classList.remove('is-invalid');
            }
        });
    });
});

// Export functions for global use
window.Admin = {
    showToast,
    formatCurrency,
    formatDate,
    confirmDialog,
    filterTable,
    exportTableToCSV,
    printTable,
    validateForm,
    debounce,
    apiRequest,
    initializeTooltips,
    initializePopovers,
    showSpinner,
    hideSpinner,
    formatFileSize,
    copyToClipboard
};
