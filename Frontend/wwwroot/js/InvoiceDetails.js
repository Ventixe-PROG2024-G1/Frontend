function loadInvoiceDetails(invoiceId) {
    console.log('Laddar detaljer för', invoiceId);
    fetch(`/Invoice/GetById/${invoiceId}`)
        .then(res => {
            if (!res.ok) throw new Error('HTTP ' + res.status);
            return res.json();
        })
        .then(invoice => {
            const div = document.getElementById('invoice-details');
            console.log(invoice)

            div.innerHTML = `
            <div class="invoice-details-header">
                <div>
                    <h3>#INV10012</h3>
                    <p class="invoice-list-status">UnPaid</p>
                </div>
                <div>
                    <p>Issued Date ${new Date(invoice.createdDate).toLocaleString('sv-SE')}</p>
                    <p>Due Date${new Date(invoice.dueDate).toLocaleDateString('sv-SE')}</p>
                </div>
            </div>
            <div class="invoice-details-body">
                <div>
                <p>Bill from:</>
                    <h4>${invoice.eventName}</h4>
                    <p>${invoice.eventAddress}, ${invoice.eventCity}</p>
                    <p>${invoice.eventEmail}</p>
                    <p>${invoice.eventPhone}</p>
                </div>
                <div>
                    <p>Bill to:</p>
                    <h4>${invoice.customerName}</h4>
                    <p>${invoice.customerAddress}, ${invoice.customerPostalCode} ${invoice.customerCity}</p>
                    <p>${invoice.customerEmail}</p>
                    <p>${invoice.customerPhone}</p>
                </div>
            </div>
            <div class="invoice-details-ticets">
            <h4>Ticket Details</h4>
            <div>
                <p><strong>Pris:</strong> ${invoice.price} kr</p>
                <p><strong>Antal:</strong> ${invoice.qty}</p>
            </div>
                <p><strong>Summa:</strong> ${invoice.amount.toFixed(2)} kr</p>
            </div>

      `;
        })
        .catch(err => {
            console.error('Kunde inte hämta fakturadetaljer:', err);
            document.getElementById('invoice-details').innerHTML =
                '<p class="text-red-600">Kunde inte ladda fakturadetaljer.</p>';
        });
}
console.log('InvoiceDetails.js loaded');
