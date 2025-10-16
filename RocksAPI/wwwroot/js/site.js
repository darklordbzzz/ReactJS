document.addEventListener('DOMContentLoaded', () => {
    // Show the first view by default
    showView('minerals-view');
    // Load initial data
    loadAllData();
});

function showView(viewId) {
    document.querySelectorAll('.view').forEach(view => {
        view.style.display = 'none';
    });
    document.getElementById(viewId).style.display = 'block';
}

function showForm(formId) {
    document.getElementById(formId).style.display = 'block';
}

function hideForm(formId) {
    document.getElementById(formId).style.display = 'none';
}

async function loadAllData() {
    loadData('minerals');
    loadData('rocks');
    loadData('samples');
    loadData('localities');
    loadData('collectors');
}

async function loadData(type) {
    try {
        const response = await fetch(`/api/${type}`);
        const data = await response.json();
        const listContainer = document.getElementById(`${type}-list`);
        listContainer.innerHTML = '';

        data.forEach(item => {
            const div = document.createElement('div');
            div.className = 'list-item';

            // Simple display - just the name or description
            const name = item.name || item.description || item.sampleId;
            div.innerHTML = `
                <span>${name}</span>
                <div class="item-buttons">
                    <button onclick="editItem('${type}', '${item.id || item.sampleId}')">Edit</button>
                    <button onclick="deleteItem('${type}', '${item.id || item.sampleId}')">Delete</button>
                </div>
            `;
            listContainer.appendChild(div);
        });
    } catch (error) {
        console.error(`Error loading ${type}:`, error);
    }
}

// Placeholder functions for edit/delete - to be implemented
async function editItem(type, id) {
    if (type === 'samples') {
        const response = await fetch(`/api/samples/${id}`);
        const sample = await response.json();
        const detailsContainer = document.getElementById('sample-details');

        let photosHtml = '';
        if (sample.photos && sample.photos.length > 0) {
            sample.photos.forEach(photo => {
                photosHtml += `<img src="${photo.url}" alt="${photo.caption}" title="${photo.caption}">`;
            });
        }

        detailsContainer.innerHTML = `
            <h3>Details for ${sample.description}</h3>
            <p><strong>ID:</strong> ${sample.sampleId}</p>
            <div class="gallery">${photosHtml}</div>
            <h4>Upload New Media</h4>
            <input type="file" id="file-${sample.sampleId}" />
            <button onclick="uploadMedia('${sample.sampleId}')">Upload</button>
            <button onclick="closeDetails()">Close</button>
        `;
        detailsContainer.style.display = 'block';
    } else {
        const response = await fetch(`/api/${type}/${id}`);
        const item = await response.json();
        generateForm(type, item);
        showForm(`${type}-form`);
    }
}

async function submitForm(type, id) {
    const form = document.getElementById(`${type}-form`);
    const inputs = form.querySelectorAll('input');
    const item = {};
    inputs.forEach(input => {
        const prop = input.id.split('-')[1];
        item[prop] = input.value;
    });

    const url = id ? `/api/${type}/${id}` : `/api/${type}`;
    const method = id ? 'PUT' : 'POST';

    try {
        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        });

        if (response.ok) {
            alert(`${type} saved successfully.`);
            hideForm(`${type}-form`);
            loadData(type);
        } else {
            alert(`Error saving ${type}.`);
        }
    } catch (error) {
        console.error(`Error saving ${type}:`, error);
    }
}

function closeDetails() {
    document.getElementById('sample-details').style.display = 'none';
}

function generateForm(type, item = {}) {
    const formContainer = document.getElementById(`${type}-form`);
    let formHtml = `<h3>${item.id ? 'Edit' : 'Add'} ${type}</h3>`;

    // A more robust solution would use a schema or reflection
    const properties = Object.keys(item);
    if (properties.length === 0) {
        // A simple fallback for new items
        switch(type) {
            case 'minerals': properties.push('name', 'formula', 'variety', 'notes'); break;
            case 'rocks': properties.push('name', 'type', 'subType', 'texture', 'origin', 'notes'); break;
            // Add other types as needed
        }
    }

    properties.forEach(prop => {
        if (prop !== 'id' && typeof item[prop] !== 'object') {
            formHtml += `
                <div>
                    <label for="${type}-${prop}">${prop}</label>
                    <input type="text" id="${type}-${prop}" value="${item[prop] || ''}">
                </div>
            `;
        }
    });

    formHtml += `
        <button onclick="submitForm('${type}', '${item.id || ''}')">Submit</button>
        <button onclick="hideForm('${type}-form')">Cancel</button>
    `;
    formContainer.innerHTML = formHtml;
}

async function deleteItem(type, id) {
    if (!confirm(`Are you sure you want to delete this ${type}?`)) {
        return;
    }

    try {
        const response = await fetch(`/api/${type}/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            alert(`${type} deleted successfully.`);
            loadData(type); // Reload the list
        } else {
            alert(`Error deleting ${type}.`);
        }
    } catch (error) {
        console.error(`Error deleting ${type}:`, error);
    }
}

// Add event listeners for search boxes
document.getElementById('minerals-search').addEventListener('input', (e) => filterList('minerals-list', e.target.value));
document.getElementById('rocks-search').addEventListener('input', (e) => filterList('rocks-list', e.target.value));
document.getElementById('samples-search').addEventListener('input', (e) => filterList('samples-list', e.target.value));
document.getElementById('localities-search').addEventListener('input', (e) => filterList('localities-list', e.target.value));
document.getElementById('collectors-search').addEventListener('input', (e) => filterList('collectors-list', e.target.value));

function filterList(listId, searchTerm) {
    const list = document.getElementById(listId);
    const items = list.getElementsByClassName('list-item');
    const term = searchTerm.toLowerCase();

    for (let i = 0; i < items.length; i++) {
        const item = items[i];
        const text = item.textContent || item.innerText;
        if (text.toLowerCase().indexOf(term) > -1) {
            item.style.display = '';
        } else {
            item.style.display = 'none';
        }
    }
}