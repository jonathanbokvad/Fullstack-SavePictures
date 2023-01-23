window.onload = async function() {
    renderPictures();
} 

async function getPictures(folderId) {
    try {
        const response = await fetch(`https://localhost:7019/api/picture?folderId=${folderId}`, {
            method: 'GET',
            headers: {
                Authorization : `Bearer ${localStorage.getItem("token")}`
              }
            });
        let data = await response.json();
        return data;
    } catch (error) {
        console.log(error);
    }
}

async function renderPictures() {
    const pictures = await getPictures(new URLSearchParams(window.location.search).get('folderId'));
    const pictureContainer = document.querySelector('.imgGrid');
    let html = '';
    for (const picture of pictures) {
      html += `<div>
                <img data-picture-id="${picture.id}" class="picture hover:scale-105 transition-all w-full max-w-sm rounded overflow-hidden shadow-lg m-4 cursor-pointer" src="data:image/jpeg;base64, ${picture.binaryData}" alt="${picture.name}">
                </div>`;
}
pictureContainer.innerHTML = html;

const images = document.querySelectorAll('.picture');
const modal = document.querySelector('#imageModal');
const closeButton = document.querySelector('.modal-close');
const modalContainer = document.querySelector('.modal-container');

    for(const pictureElement of images){
        pictureElement.addEventListener('click', function() {
            modalContainer.innerHTML = `<div class="modal-content py-4 text-left px-6 max-w-screen max-h-screen">
            <img class="" data-picture-id="${pictureElement.getAttribute('data-picture-id')}" src="${pictureElement.src}" alt="${pictureElement.name}">     
            <button class="btn btn-blue mt-2 delete-btn">Delete</button>
          </div>`;
        modal.classList.remove('modal-close');
        modal.classList.add('modal-open');
        DeletePicture();
        });
    }
    closeButton.addEventListener('click', function(){
        modal.classList.remove('modal-open');
        modal.classList.add('modal-close');
    });
}

function DeletePicture(){
    const button = document.querySelector('.delete-btn');
    button.addEventListener('click', async function (){
        const pictureId = this.parentNode.querySelector('img').getAttribute('data-picture-id');
        await fetch(`https://localhost:7019/api/picture?folderId=${new URLSearchParams(window.location.search).get('folderId')}&pictureId=${pictureId}`, {
            method: 'DELETE',
            headers: {
                Authorization : `Bearer ${localStorage.getItem("token")}`
            }
        })
        .then(response => response)
        .then(data => data)
        .catch(error => console.log(error));
        renderPictures();
    })
}

async function AddPicture() {
    let input = document.createElement('input');
    input.type = 'file';
    const folderId = new URLSearchParams(window.location.search).get('folderId');
    input.onchange = function () {
      const file = input.files[0];
      const reader = new FileReader();
      reader.readAsArrayBuffer(file);
      reader.onloadend = async function() {
          const byteArray = new Uint8Array(reader.result);
          const base64 = btoa(new Uint8Array(byteArray).reduce((data, byte) => data + String.fromCharCode(byte), ''));
          await fetch("https://localhost:7019/api/picture/create", {
            method: "POST",
            headers: { 
                "Content-Type": "application/json",
                Authorization : `Bearer ${localStorage.getItem("token")}`
              },
            body: JSON.stringify({ data: base64, name: file.name, folderId: folderId})
          })
            .then(response => response.json())
            .then(data => data)
            .catch(error => console.log(error));
            renderPictures();
      };
    }
    input.click();
}

async function deleteAccount(){
    if(!confirm("Are you sure you want to delete your account?")){
        return;
    }
    await fetch(`https://localhost:7019/api/user?userId=${localStorage.getItem("currentUser")}`, { 
        method: "Delete",
        headers: {
            Authorization : `Bearer ${localStorage.getItem("token")}`
          }
    })
    .then(response => response.json())
    .then(data => data)
    .catch(error => console.error(error));
    window.location.href = "../index.html";
    }