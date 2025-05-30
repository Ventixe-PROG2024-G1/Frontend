function loadInvoiceEmail(invoiceId) {
    const sendBtn = document.getElementById('sendEmail');

    sendBtn.addEventListener('click', (event) => {
        event.preventDefault();
        SendEmailById(invoiceId);
    });
}

async function SendEmailById(invoiceId) {
    try {
        const response = await fetch(`/Invoice/GetEmailById/${invoiceId}`);

        if (!response.ok) {
            const error = await response.text();
            console.log("Error:", error);
            alert("Gick inte att skicka fakturan");
            return;
        }
        const data = await response.text();
        alert("Fakturan är skickad!");
        console.log("Svar:", data);
    } catch (error) {
        console.error("Något gick fel:", error);
    }
}
