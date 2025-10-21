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
    const item = {};
    if (type === 'localities') {
        item.name = document.getElementById('locality-name').value;
        item.country = document.getElementById('locality-country').value;
        item.stateProvince = document.getElementById('locality-stateProvince').value;
        item.region = document.getElementById('locality-region').value;
        item.latitude = parseFloat(document.getElementById('locality-latitude').value) || 0;
        item.longitude = parseFloat(document.getElementById('locality-longitude').value) || 0;
        item.mindatUrl = document.getElementById('locality-mindatUrl').value;
        item.notes = document.getElementById('locality-notes').value;
    } else {
        // Generic fallback
        const form = document.getElementById(`${type}-form`);
        const inputs = form.querySelectorAll('input, textarea');
        inputs.forEach(input => {
            const prop = input.id.split('-')[1];
            item[prop] = input.value;
        });
    }

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
    let formHtml = `<h3>${item.localityId ? 'Edit' : 'Add'} Locality</h3>`;

    if (type === 'localities') {
        formHtml += `
            <div><label>Name:</label><input type="text" id="locality-name" value="${item.name || ''}" placeholder="e.g., Skardu"></div>
            <div><label>Country:</label><input type="text" id="locality-country" value="${item.country || ''}" placeholder="e.g., Pakistan"></div>
            <div><label>State/Province:</label><input type="text" id="locality-stateProvince" value="${item.stateProvince || ''}"></div>
            <div><label>Region:</label><input type="text" id="locality-region" value="${item.region || ''}"></div>
            <div><label>Latitude:</label><input type="number" id="locality-latitude" value="${item.latitude || ''}"></div>
            <div><label>Longitude:</label><input type="number" id="locality-longitude" value="${item.longitude || ''}"></div>
            <div><label>Mindat URL:</label><input type="text" id="locality-mindatUrl" value="${item.mindatUrl || ''}"></div>
            <div><label>Notes:</label><textarea id="locality-notes">${item.notes || ''}</textarea></div>
        `;
    } else {
        // Fallback for other types (can be specialized too)
        const properties = Object.keys(item).length ? Object.keys(item) : ['name', 'notes']; // Simple default
        properties.forEach(prop => {
            if (prop !== 'id' && prop !== 'localityId' && typeof item[prop] !== 'object') {
                formHtml += `<div><label>${prop}:</label><input type="text" id="${type}-${prop}" value="${item[prop] || ''}"></div>`;
            }
        });
    }

    formHtml += `
        <button onclick="submitForm('${type}', '${item.localityId || item.id || ''}')">Submit</button>
        <button onclick="hideForm('${type}-form')">Cancel</button>
    `;
    formContainer.innerHTML = formHtml;
    showForm(`${type}-form`);
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