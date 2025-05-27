document.addEventListener('DOMContentLoaded', () => {
    var modal = document.getElementById('booking-modal');
    var open = document.getElementById('booking-modal-open');
    var close = document.querySelector('.booking-modal-close');

    open.addEventListener('click', () => {
        modal.classList.remove('hide-modal');
    });

    close.addEventListener('click', () => {
        modal.classList.add('hide-modal');
    });

    window.addEventListener('click', (e) => {
        if (e.target === modal)
            modal.classList.add("hide-modal");
    });

    populateCategory();

});

const populateCategory = () => {
    const categorySelect = document.getElementById('ticket-category');
    fetch("/EventDetails/Tickets")
        .then(res => res.ok ? res.json() : Promise.reject(res.status))
        .then(data => {
            categorySelect.innerHTML = '<option value="">-- Select --</option>';
            console.log(data);
            data.forEach(t => {
                const option = document.createElement('option');
                option.value = t.id;
                option.textContent = `${t.tiers} - $${t.price}`;
                categorySelect.appendChild(option);
            });
        });
    categorySelect.addEventListener('change', (e) => {
        populateQuantity(e.target.value);
    });

}

const populateQuantity = (id) => {
    console.log(id);
    fetch(`/EventDetails/Tickets/${id}`)
        .then(res => res.ok ? res.json() : Promise.reject(res.status))
        .then(t => {
            const quantitySelect = document.getElementById('ticket-quantity');
            quantitySelect.innerHTML = '';
            console.log(t);
            for (i = 1; i <= t.quantity && 10; i++) {
                const option = document.createElement('option');
                option.value = i;
                option.textContent = i;
                quantitySelect.appendChild(option);
                console.log(option);
            }
        });
}
