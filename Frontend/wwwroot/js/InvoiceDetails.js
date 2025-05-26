function loadInvoiceDetails(invoiceId) {
    fetch(`/Invoice/GetById/${invoiceId}`)
        .then(res => {
            if (!res.ok) throw new Error('HTTP ' + res.status);
            return res.json();
        })
        .then(invoice => {
            console.log(invoice)
            const div = document.getElementById('invoice-details');
            div.innerHTML = `
            <div class="invoice-details-header">
                <div>
                    <h3>${invoice.invoiceNumber}</h3>
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
                <table border="1" cellpadding="8" cellspacing="0" class="invoice-ticet-table">
                  <thead>
                    <tr>
                      <th>Ticket Category</th>
                      <th>Price</th>
                      <th>Qty</th>
                      <th>Amount</th>
                    </tr>
                  </thead>
                  <tbody>

                    <tr>
                      <td>VIP</td>
                      <td>$ ${invoice.price}</td>
                      <td>${invoice.qty}</td>
                      <td>$ ${invoice.amount}</td>
                    </tr>
                    <tr>
                      <td>Moms</td>
                      <td></td>
                      <td></td>
                      <td>$ ${(invoice.amount * 0.20).toFixed(2)}</td>
                    </tr>
                    <tr>
                      <td>Total</td>
                      <td></td>
                      <td></td>
                      <td>$ ${invoice.amount}</td>
                    </tr>
                  </tbody>
                </table>
            </div>
            <div class="invoice-detail-footer">
            <p>Note</p>
            <p>Please make payment before the due date to avoid any penalties or cancellation of your ticket. For any questions or concerns, contact our support team at support@eventmgmt.com or </p>
            <p>call +1-800-555-1234.</p>
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
