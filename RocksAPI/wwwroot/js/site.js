document.addEventListener('DOMContentLoaded', () => {
    // Show the first view by default
    showView('minerals-view');
    // Load initial data
    loadAllData();
});

// --- View Management ---
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

function closeDetails() {
    document.getElementById('sample-details').style.display = 'none';
}

// --- Data Loading & Display ---
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
        if (!response.ok) throw new Error(`Failed to fetch ${type}`);
        const data = await response.json();
        const listContainer = document.getElementById(`${type}-list`);
        listContainer.innerHTML = ''; // Clear previous items

        data.forEach(item => {
            const div = document.createElement('div');
            div.className = 'list-item';
            const name = item.name || item.description || item.sampleId || 'Unnamed Item';
            const itemId = item.id || item.sampleId || item.localityId || item.collectorId;

            div.innerHTML = `
                <span>${name}</span>
                <div class="item-buttons">
                    <button onclick="editItem('${type}', '${itemId}')">Edit</button>
                    <button onclick="deleteItem('${type}', '${itemId}')">Delete</button>
                </div>
            `;
            listContainer.appendChild(div);
        });
    } catch (error) {
        console.error(`Error loading ${type}:`, error);
        document.getElementById(`${type}-list`).innerHTML = `<p class="error">Could not load ${type}.</p>`;
    }
}

// --- Form Generation ---
function generateForm(type, item = {}) {
    const formContainer = document.getElementById(`${type}-form`);
    const isEdit = Object.keys(item).length > 0;
    let formHtml = `<h3>${isEdit ? 'Edit' : 'Add'} ${capitalize(type)}</h3>`;

    switch (type) {
        case 'minerals':
            formHtml += `
                <div><label>Name:</label><input type="text" id="mineral-name" value="${item.name || ''}"></div>
                <div><label>Formula:</label><input type="text" id="mineral-formula" value="${item.formula || ''}"></div>
                <div><label>Variety:</label><input type="text" id="mineral-variety" value="${item.variety || ''}"></div>
                <div><label>Notes:</label><textarea id="mineral-notes">${item.notes || ''}</textarea></div>
            `;
            break;
        case 'rocks':
             formHtml += `
                <div><label>Name:</label><input type="text" id="rock-name" value="${item.name || ''}"></div>
                <div><label>Type:</label><input type="text" id="rock-type" value="${item.type || ''}"></div>
                <div><label>Sub-Type:</label><input type="text" id="rock-subType" value="${item.subType || ''}"></div>
                <div><label>Texture:</label><input type="text" id="rock-texture" value="${item.texture || ''}"></div>
                <div><label>Origin:</label><textarea id="rock-origin">${item.origin || ''}</textarea></div>
                <div><label>Notes:</label><textarea id="rock-notes">${item.notes || ''}</textarea></div>
            `;
            break;
        case 'localities':
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
            break;
        case 'collectors':
             formHtml += `
                <div><label>Name:</label><input type="text" id="collector-name" value="${item.name || ''}"></div>
                <div><label>Email:</label><input type="email" id="collector-email" value="${item.email || ''}"></div>
                <div><label>Member Since:</label><input type="date" id="collector-memberSince" value="${(item.memberSince || '').substring(0, 10)}"></div>
                <div><label>Notes:</label><textarea id="collector-notes">${item.notes || ''}</textarea></div>
            `;
            break;
        case 'samples':
             formHtml += `
                <div><label>Description:</label><input type="text" id="sample-description" value="${item.description || ''}"></div>
                <div><label>Collector ID:</label><input type="text" id="sample-collectorId" value="${item.collectorId || ''}"></div>
                <div><label>Date Acquired:</label><input type="date" id="sample-dateAcquired" value="${(item.dateAcquired || '').substring(0, 10)}"></div>
                <div><label>Item Type:</label><input type="text" id="sample-itemType" value="${item.itemType || ''}"></div>
                <div><label>Notes:</label><textarea id="sample-notes">${item.notes || ''}</textarea></div>
            `;
            break;
    }

    const itemId = item.id || item.sampleId || item.localityId || item.collectorId || '';
    formHtml += `
        <button onclick="submitForm('${type}', '${itemId}')">Submit</button>
        <button type="button" onclick="hideForm('${type}-form')">Cancel</button>
    `;
    formContainer.innerHTML = formHtml;
    showForm(`${type}-form`);
}


// --- Data Manipulation ---
async function editItem(type, id) {
    try {
        const response = await fetch(`/api/${type}/${id}`);
        if (!response.ok) throw new Error(`Item ${id} not found.`);
        const item = await response.json();

        if (type === 'samples') {
            // Special view for samples with gallery
            const detailsContainer = document.getElementById('sample-details');
            let photosHtml = '';
            if (item.photos && item.photos.length > 0) {
                item.photos.forEach(photo => {
                    photosHtml += `<img src="${photo.url}" alt="${photo.caption}" title="${photo.caption}">`;
                });
            }

            detailsContainer.innerHTML = `
                <h3>Details for ${item.description}</h3>
                <p><strong>ID:</strong> ${item.sampleId}</p>
                <div class="gallery">${photosHtml}</div>
                <h4>Upload New Media</h4>
                <form onsubmit="uploadMedia(event, '${item.sampleId}')">
                    <input type="file" name="file" required />
                    <button type="submit">Upload</button>
                </form>
                <button type="button" onclick="closeDetails()">Close</button>
            `;
            detailsContainer.style.display = 'block';
        } else {
            generateForm(type, item);
        }
    } catch (error) {
        console.error(`Error fetching item ${id}:`, error);
        alert(`Could not load item for editing.`);
    }
}

async function submitForm(type, id) {
    const item = {};
    // Gather data from the correct specialized form
    switch (type) {
        case 'localities':
            item.name = document.getElementById('locality-name').value;
            item.country = document.getElementById('locality-country').value;
            item.stateProvince = document.getElementById('locality-stateProvince').value;
            item.region = document.getElementById('locality-region').value;
            item.latitude = parseFloat(document.getElementById('locality-latitude').value) || 0;
            item.longitude = parseFloat(document.getElementById('locality-longitude').value) || 0;
            item.mindatUrl = document.getElementById('locality-mindatUrl').value;
            item.notes = document.getElementById('locality-notes').value;
            break;
        case 'minerals':
            item.name = document.getElementById('mineral-name').value;
            item.formula = document.getElementById('mineral-formula').value;
            item.variety = document.getElementById('mineral-variety').value;
            item.notes = document.getElementById('mineral-notes').value;
            break;
        case 'rocks':
            item.name = document.getElementById('rock-name').value;
            item.type = document.getElementById('rock-type').value;
            item.subType = document.getElementById('rock-subType').value;
            item.texture = document.getElementById('rock-texture').value;
            item.origin = document.getElementById('rock-origin').value;
            item.notes = document.getElementById('rock-notes').value;
            break;
        case 'collectors':
            item.name = document.getElementById('collector-name').value;
            item.email = document.getElementById('collector-email').value;
            item.memberSince = document.getElementById('collector-memberSince').value;
            item.notes = document.getElementById('collector-notes').value;
            break;
        case 'samples':
            item.description = document.getElementById('sample-description').value;
            item.collectorId = document.getElementById('sample-collectorId').value;
            item.dateAcquired = document.getElementById('sample-dateAcquired').value;
            item.itemType = document.getElementById('sample-itemType').value;
            item.notes = document.getElementById('sample-notes').value;
            break;
    }

    const url = id ? `/api/${type}/${id}` : `/api/${type}`;
    const method = id ? 'PUT' : 'POST';

    try {
        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(item)
        });

        if (response.ok) {
            alert(`${capitalize(type)} saved successfully.`);
            hideForm(`${type}-form`);
            loadData(type);
        } else {
            const error = await response.json();
            alert(`Error saving ${type}: ${JSON.stringify(error.errors)}`);
        }
    } catch (error) {
        console.error(`Error saving ${type}:`, error);
    }
}

async function deleteItem(type, id) {
    if (!confirm(`Are you sure you want to delete this ${type.slice(0, -1)}?`)) return;

    try {
        const response = await fetch(`/api/${type}/${id}`, { method: 'DELETE' });
        if (response.ok) {
            alert(`${capitalize(type.slice(0, -1))} deleted successfully.`);
            loadData(type);
        } else {
            alert(`Error deleting ${type.slice(0, -1)}.`);
        }
    } catch (error) {
        console.error(`Error deleting ${type}:`, error);
    }
}

async function uploadMedia(event, sampleId) {
    event.preventDefault();
    const form = event.target;
    const fileInput = form.querySelector('input[type="file"]');
    const file = fileInput.files[0];
    if (!file) {
        alert('Please select a file to upload.');
        return;
    }

    const formData = new FormData();
    formData.append('file', file);

    try {
        const response = await fetch(`/api/media/upload/${sampleId}`, {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            alert('File uploaded successfully!');
            editItem('samples', sampleId); // Refresh the details view
        } else {
            alert('File upload failed.');
        }
    } catch (error) {
        console.error('Upload error:', error);
    }
}

// --- Utility & Search ---
function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

document.querySelectorAll('.controls input[type="text"]').forEach(input => {
    const listId = input.id.replace('-search', '-list');
    input.addEventListener('input', (e) => filterList(listId, e.target.value));
});

function filterList(listId, searchTerm) {
    const list = document.getElementById(listId);
    const items = list.getElementsByClassName('list-item');
    const term = searchTerm.toLowerCase();

    for (let i = 0; i < items.length; i++) {
        const itemText = items[i].querySelector('span').textContent.toLowerCase();
        if (itemText.includes(term)) {
            items[i].style.display = '';
        } else {
            items[i].style.display = 'none';
        }
    }
}