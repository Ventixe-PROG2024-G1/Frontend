document.addEventListener('DOMContentLoaded', () => {
    const detailDiv = document.getElementById('invoice-details');
    detailDiv.innerHTML = '<p>Klicka på en faktura för att se mer information.</p>';
    const ul = document.getElementById('invoice-list');
    ul.innerHTML ='<p>Fakturor laddas</p>'

    fetch('/Invoice/GetInvoices')
        .then(res => {
            if (!res.ok) throw new Error('HTTP ' + res.status);
            return res.json();
        })
        .then(invoices => {
            ul.innerHTML = '';
            invoices.forEach(inv => {
                const li = document.createElement('li');
                li.className = 'invoice-list-item';
                li.innerHTML = `
          <button class="invoice-button">
              <div>
                <h3>${inv.invoiceNumber}</h3>
                <p class="invoice-list-date">${new Date(inv.createdDate).toLocaleDateString('sv-SE')}</p>
              </div>
              <div>
                <p>${inv.amount.toFixed(2)} kr</p>
                <p class="invoice-list-status">${inv.paid ? 'Paid' : 'Unpaid'}</p>
              </div>
          </button>`;
                li.querySelector('.invoice-button')
                    .addEventListener('click', () => {
                        loadInvoiceDetails(inv.id);
                        loadInvoiceEmail(inv.id);
                    });
                ul.appendChild(li);
            });
        })
        .catch(err => {
            console.error('Fel vid hämtning av fakturor:', err);
            document.getElementById('invoice-list')
                .innerHTML = '<li class="invoice-list-item">Laddar fakturor...</li>';
        });


});



