document.addEventListener('DOMContentLoaded', () => {
    const div = document.getElementById('Getbtn');
    const button = document.createElement('button');
    button.className = 'test';
    button.textContent = 'Hämta';

    div.appendChild(button);

    button.addEventListener('click', () => {
        loadEvoucherTicket("161ea8fb-73d9-41bd-b87e-6164b98357d8", "161ea8fb-73d9-41bd-b87e-6164b98357d8");
    });
});
function loadEvoucherTicket(invoiceId, EventId) {
    fetch(`/EVoucher/GetByIds/${invoiceId}/${EventId}`)
        .then(res => {
            if (!res.ok) throw new Error('HTTP ' + res.status);
            return res.json();
        })
        .then(evoucher => {
            console.log(evoucher)
            const div = document.getElementById('evoucher-tickerInfo');
            div.innerHTML = `
            <div class="evoucher-tickerInfo-item">
                <div>
                    <span>
                        <h4>Namn</h4>
                        <p>${evoucher.ticket.name}</p>
                    </span>
                    <span>
                        <h4>Ticket Category</h4>
                        <p>${evoucher.ticket.type}</p>
                    </span>
                </div>
                <div>
                    <span>
                        <h4>Invoice ID</h4>
                        <p>${evoucher.ticket.invoiceNumber}</p>
                    </span>
                    <span>
                        <h4>Seat Number</h4>
                        <p>${evoucher.ticket.seatNumber}</p>
                    </span>
                </div>
                <div>
                <span>
                        <h4>Gate</h4>
                        <p>${evoucher.ticket.gate}</p>
                    </span>
                </div>
            </div>
            <div class="evoucher-tickerInfo-item">
                <div>
                    <span>
                        <h4>Location</h4>
                        <p>${evoucher.ticket.location}</p>
                    </span>
                    <span>
                        <h4>Date</h4>
                        <p>${evoucher.ticket.date}</p>
                    </span>
                </div>
                <div>
                    <span>
                        <h4>Time</h4>
                        <p>${evoucher.ticket.time}</p>
                    </span>
                </div>
            </div>`
        })
}