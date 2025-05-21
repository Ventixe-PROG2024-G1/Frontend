function loadInvoiceEmail(invoiceId) {
    const sendBtn = document.getElementById('sendEmail');

    sendBtn.addEventListener('click', (event) => {
        event.preventDefault();
        console.log('Skickar faktura...');
        console.log(invoiceId)
    })
};