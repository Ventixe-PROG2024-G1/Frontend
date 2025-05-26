export const initCustomSelect = () => {
    document.querySelectorAll('.custom-select').forEach(s => {
        const selected = s.querySelector('.custom-select-selected');
        const options = s.querySelector('.custom-select-options');
        const text = selected.querySelector('.custom-select-text');
        const defaultValue = selected.dataset.value;

        if (defaultValue) {
            const defaultOption = options.querySelector(`li[data-value="${defaultValue}"]`);
            if (defaultOption) {
                text.textContent = defaultOption.textContent;
            }
        }

        selected.addEventListener('click', () => {
            closeAllCustomSelects(s);
            options.classList.toggle('custom-select-hide');
            selected.classList.toggle('custom-select-active');
        });

        

        options.querySelectorAll('li').forEach(o => {
            o.addEventListener('click', () => {
                text.textContent = o.textContent;
                selected.dataset.value = o.dataset.value;
                options.classList.add('custom-select-hide');
                selected.classList.remove('custom-select-active');

                const changeEvent = new CustomEvent('custom-select-change', {
                    detail: {
                        value: o.dataset.value,
                        text: o.textContent
                    }
                });
                s.dispatchEvent(changeEvent);
            });
        });
    });

    document.addEventListener('click', (e) => {
        if (!e.target.closest('.custom-select')) {
            document.querySelectorAll('.custom-select-options').forEach(o => o.classList.add('custom-select-hide'));
            document.querySelectorAll('.custom-select-selected').forEach(s => s.classList.remove('custom-select-active'));
        }
    });

    const closeAllCustomSelects = (current) => {
        document.querySelectorAll('.custom-select').forEach(s => {
            if (s !== current) {
                s.querySelector('.custom-select-options').classList.add('custom-select-hide');
                s.querySelector('.custom-select-selected').classList.remove('custom-select-active');
            }
        });
    };
}