function loadInvoiceDetails(invoiceId) {
    console.log('Laddar detaljer för', invoiceId);
    fetch(`/Invoice/GetById/${invoiceId}`)
        .then(res => {
            if (!res.ok) throw new Error('HTTP ' + res.status);
            return res.json();
        })
        .then(invoice => {
            const div = document.getElementById('invoice-details');

            div.innerHTML = `
        <h3>Faktura: ${invoice.title}</h3>
        <p><strong>Skapad:</strong> ${new Date(invoice.createdDate).toLocaleString('sv-SE')}</p>
        <p><strong>Förfallodatum:</strong> ${new Date(invoice.dueDate).toLocaleDateString('sv-SE')}</p>
        <p><strong>Pris:</strong> ${invoice.price} kr</p>
        <p><strong>Antal:</strong> ${invoice.qty}</p>
        <p><strong>Summa:</strong> ${invoice.amount.toFixed(2)} kr</p>
        
        <h4>Kund</h4>
        <p><strong>Namn:</strong> ${invoice.customerName}</p>
        <p><strong>Adress:</strong> ${invoice.customerAddress}, ${invoice.customerPostalCode} ${invoice.customerCity}</p>
        <p><strong>E-post:</strong> ${invoice.customerEmail}</p>
        <p><strong>Telefon:</strong> ${invoice.customerPhone}</p>
        
        <h4>Evenemang</h4>
        <p><strong>Namn:</strong> ${invoice.eventName}</p>
        <p><strong>Adress:</strong> ${invoice.eventAddress}, ${invoice.eventCity}</p>
        <p><strong>E-post:</strong> ${invoice.eventEmail}</p>
        <p><strong>Telefon:</strong> ${invoice.eventPhone}</p>
      `;
        })
        .catch(err => {
            console.error('Kunde inte hämta fakturadetaljer:', err);
            document.getElementById('invoice-details').innerHTML =
                '<p class="text-red-600">Kunde inte ladda fakturadetaljer.</p>';
        });
}
console.log('InvoiceDetails.js loaded');
