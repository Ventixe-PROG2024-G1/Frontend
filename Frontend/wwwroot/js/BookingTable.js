import { initCustomSelect } from "/js/CustomSelect.js";

document.addEventListener('DOMContentLoaded', () => {
    loadBookings();

    const categorySelect = document.getElementById('category-select');
    categorySelect.innerHTML = '';

    fetch('/Booking/GetAllEventCategories')
        .then(res => res.ok ? res.json() : Promise.reject(res.status))
        .then(data => {
            categorySelect.innerHTML = `
                <div class="custom-select-selected">
                    <span class="custom-select-text">All Categories</span>
                    <svg class="custom-select-icon" width="14" height="14" viewBox="0 0 14 14" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path fill-rule="evenodd" clip-rule="evenodd" d="M3.19064 5.37814C3.3615 5.20729 3.6385 5.20729 3.80936 5.37814L7 8.56878L10.1906 5.37814C10.3615 5.20729 10.6385 5.20729 10.8094 5.37814C10.9802 5.549 10.9802 5.826 10.8094 5.99686L7.30936 9.49686C7.1385 9.66771 6.8615 9.66771 6.69064 9.49686L3.19064 5.99686C3.01979 5.826 3.01979 5.549 3.19064 5.37814Z" fill="currentColor" />
                    </svg>
                </div>
            `;
            const selectOptions = document.createElement('ul');
            selectOptions.className = 'custom-select-options custom-select-hide';

            const allCategories = document.createElement('li');
            allCategories.dataset.value = '';
            allCategories.textContent = 'All Categories';
            selectOptions.appendChild(allCategories);

            data.forEach(c => {
                const item = document.createElement('li');
                item.dataset.value = c.categoryName;
                item.textContent = c.categoryName;
                selectOptions.appendChild(item);
            });
            categorySelect.appendChild(selectOptions);

            initCustomSelect();
        })
        .catch(err => {
            console.error('Failed to load Event Categories: ', err);
        });
});

const loadBookings = () => {
    const tableBody = document.getElementById('booking-table-body');
    tableBody.innerHTML = '<tr><td colspan="12">Loading Bookings.</td></tr>';
    const queryParams = window.location.search;

    fetch(`/Booking/GetTableData${queryParams}`)
        .then(res => res.ok ? res.json() : Promise.reject(res.status))
        .then(data => {
            tableBody.innerHTML = '';
            data.bookings.forEach(b => {
                const created = new Date(b.created);
                const createdDate = created ? created.toLocaleDateString() : '';
                const createdTime = created ? created.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) : '';
                const row = document.createElement('tr');
                row.dataset.event = b.eventId;
                row.dataset.invoice = b.invoiceId;
                row.className = 'body-row';
                row.innerHTML = `
                <td class="invoice-desktop-col cell-reference">${b.invoiceId}</td>
                <td class="invoice-mobile-col">
                    <div class="bookingtable-multi-cell">
                        <div>${b.invoiceId}</div>
                        <div class="cell-subrow">${b.userName || 'Unknown'}</div>
                    </div>
                </td>
                <td>
                    <div class="bookingtable-multi-cell">
                        <div>${createdDate}</div>
                        <div class="cell-subrow">${createdTime}</div>
                    </div>
                </td>
                <td class="name-col">${b.userName || 'Unknown'}</td>
                <td>
                    <div class="bookingtable-multi-cell">
                        <div>${b.eventName || 'Unknown'}</div>
                        <div class="cell-subrow">${b.eventCategory || 'Unknown'}</div>
                    </div>
                </td>
                <td class="category-col">
                    <div class="bookingticket-category">
                        <div class="category-dot category-${(b.ticketCategory || '').toLowerCase()}"></div>
                        <div class="category-name">${b.ticketCategory || 'Unknown'}</div>
                    </div>
                </td>
                <td class="price-col">$${b.ticketPrice}</td>
                <td class="qty-col col-center center">${b.ticketQuantity}</td>
                <td class="ticket-col">
                    <div class="bookingtable-multi-cell">
                        <div class="bookingticket-category">
                            <div class="category-dot"></div>
                            <div class="category-name">${b.ticketCategory || 'Unknown'}</div>
                        </div>
                        <div class="ticketprice-qty">
                            <div class="cell-subrow">${b.ticketQuantity}x</div>
                            <div class="cell-subrow">$${b.ticketPrice}</div>
                        </div>
                    </div>
                </td>
                <td class="amount-col">$${b.totalTicketPrice}</td>
                <td>
                    <div class="bookingstatus-cell bookingstatus-${b.status.toLowerCase()}">${b.status}</div>
                </td>
                <td class="voucher-col cell-reference">-</td>
            `;
                row.addEventListener('click', (e) => {
                    window.location.href =`/EVoucher/GetByIds/${e.currentTarget.dataset.invoice}/${e.currentTarget.dataset.event}`;
                });
                tableBody.appendChild(row);
            });
        })
        .catch(err => {
            console.error('Failed to load bookings: ', err);
            tableBody.innerHTML = '<tr><td colspan="12">Failed to load Bookings.</td></tr>';
        });
};

const updateBookingQuery = (key, value) => {
    const url = new URL(window.location.href);
    if (value)
        url.searchParams.set(key, value);
    else
        url.searchParams.delete(key);
    window.history.pushState({}, '', url);
    loadBookings();
};