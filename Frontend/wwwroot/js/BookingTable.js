document.addEventListener('DOMContentLoaded', () => {
    const tableBody = document.getElementById('booking-table-body');
    tableBody.innerHTML = '<tr><td colspan="12">Loading Bookings.</td></tr>';
    const queryParams = window.location.search;
    console.log("params: " + queryParams);

    fetch(`/Booking/GetTableData${queryParams}`)
        .then(res => res.ok ? res.json() : Promise.reject(res.status))
        .then(data => {
            console.log(data);
            tableBody.innerHTML = '';
            data.bookings.forEach(b => {
                const created = new Date(b.created);
                const createdDate = created ? created.toLocaleDateString() : '';
                const createdTime = created ? created.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) : '';
                const row = document.createElement('tr');
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
                                    <div class="category-name">${b.ticketCategory || 'Unknown' }</div>
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
                tableBody.appendChild(row);
            })
        })
        .catch(err => {
            console.error('Failed to load bookings: ', err);
            tableBody.innerHTML = '<tr><td colspan="12">Failed to load Bookings.</td></tr>';
        });
});