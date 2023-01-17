window.onload = async function() {
    const folderId = new URLSearchParams(window.location.search).get('folderId');
    const pictures = await getPictures(folderId);
    renderPictures(pictures);
} 

async function getPictures(folderId) {
    try {
        const response = await fetch(`https://localhost:7019/api/pictures?folderId=${folderId}`, {
            method: 'GET'});
        let data = await response.json();
        console.log(response);
        return data;
    } catch (error) {
        console.log(error);
    }
}

async function renderPictures(pictures) {
    const pictureContainer = document.querySelector('.imgGrid');
    let html = '';
    //data-picture-id="${picture.id}";
    for (const picture of pictures) {
        console.log(picture);

      html += `<div>
                <img data-picture-id="${picture.id}" class="picture hover:scale-105 transition-all w-full max-w-sm rounded overflow-hidden shadow-lg m-4 cursor-pointer" src="data:image/jpeg;base64, ${picture.binaryData}" alt="${picture.name}">
                </div>`;

};
pictureContainer.innerHTML = html;
// `<div class="w-full max-w-sm rounded overflow-hidden shadow-lg m-4 cursor-pointer bg-cover bg-center photo-id">
//               <img class="w-full" src="data:image/jpeg;base64, ${picture.data}" alt="${picture.name}">
//               <div class="px-6 py-4">
//                 <div class="font-bold text-xl mb-2">${picture.name}</div>
//               </div>
//             </div>`

const images = document.querySelectorAll('.picture');
const modal = document.querySelector('#imageModal');
const closeButton = document.querySelector('.modal-close');
const modalContainer = document.querySelector('.modal-container');
    for(const pictureElement of images){

        pictureElement.addEventListener('click', function() {
            modalContainer.innerHTML = `<div class="modal-content py-4 text-left px-6">
                                        <img data-picture-id="${pictureElement.getAttribute('data-picture-id')}" src="${pictureElement.src}" alt="${pictureElement.name}">     
                                        <button class="btn btn-blue mt-2 delete-btn">Delete</button>
                                        </div>`;
        modal.classList.remove('modal-close');
        modal.classList.add('modal-open');
        DeleteButton();
        });
    }
    closeButton.addEventListener('click', function(){
        modal.classList.remove('modal-open');
        modal.classList.add('modal-close');
    });
}

function DeleteButton(){
    const button = document.querySelector('.delete-btn');
    button.addEventListener('click', function (){
        const pictureId = this.parentNode.querySelector('img').getAttribute('data-picture-id');
        fetch(`https://localhost:7019/api/pictures?pictureId=${pictureId}`, {method: 'DELETE'})
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.log(error))
        .finally(getPictures())
    })
}