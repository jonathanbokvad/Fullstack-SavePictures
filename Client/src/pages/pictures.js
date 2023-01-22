window.onload = async function() {
    const pictures = await getPictures(new URLSearchParams(window.location.search).get('folderId'));
    renderPictures(pictures);
} 

async function getPictures(folderId) {
    try {
        const response = await fetch(`https://localhost:7019/api/pictures?folderId=${folderId}`, {method: 'GET'});
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
      html += `<div>
                <img data-picture-id="${picture.id}" class="picture hover:scale-105 transition-all w-full max-w-sm rounded overflow-hidden shadow-lg m-4 cursor-pointer" src="data:image/jpeg;base64, ${picture.binaryData}" alt="${picture.name}">
                </div>`;

}
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
console.log(pictureElement);
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
    button.addEventListener('click', function (){
        const pictureId = this.parentNode.querySelector('img').getAttribute('data-picture-id');
        fetch(`https://localhost:7019/api/pictures?folderId=${new URLSearchParams(window.location.search).get('folderId')}&pictureId=${pictureId}`, {method: 'DELETE'})
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.log(error))
        .finally(getPictures())
    })
}

// async function AddPicture() {
//     let input = document.createElement('input');
//     input.type = 'file';
//     input.onchange = async _ => {
//       let file = input.files[0];
//       let reader = new FileReader();
//       reader.readAsArrayBuffer(file);
//       reader.onloadend = async function() {
//         let byteArray = new Uint8Array(reader.result);
//         console.log(byteArray)
//         await fetch("https://localhost:7019/api/pictures/create", {
//           method: "POST",
//           headers: { "Content-Type": "application/json" },
//           body: JSON.stringify(byteArray)
//         })
//           .then(response => console.log(response.json()))
//           then(data => console.log(data))
//           .catch(error => console.log(error));
//       };
//     };
//     input.click();
//   }
  
// function AddPicture() {
//     let input = document.createElement('input');
//     input.type = 'file';
//     input.onchange = function () {
//       const file = input.files[0];
//       const reader = new FileReader();
//       reader.readAsArrayBuffer(file);
//       console.log(reader);
//       console.log(reader.result);
//       reader.onloadend = async function() {
//           const byteArray = new Uint8Array(reader.result);
//           const base64 = btoa(new Uint8Array(byteArray).reduce((data, byte) => data + String.fromCharCode(byte), ''));
//           const data = `data:${file.type};base64,${base64}`;
//           console.log(data);
//           await fetch("https://localhost:7019/api/pictures/create", {
//             method: "POST",
//             headers: { "Content-Type": "application/json" },
//             body: JSON.stringify({ image: data, contentType: file.type })
//           })
//             .then(response => console.log(response.json()))
//             .then(data => console.log(data))
//             .catch(error => console.log(error));
//       };
//     }
//     input.click();
//   }


// eslint-disable-next-line no-unused-vars
function AddPicture() {
    let input = document.createElement('input');
    input.type = 'file';
    const folderId = new URLSearchParams(window.location.search).get('folderId');
    input.onchange = function () {
      const file = input.files[0];
      const reader = new FileReader();
      reader.readAsArrayBuffer(file);
      console.log(file);
      console.log(reader.result);
      reader.onloadend = async function() {
          const byteArray = new Uint8Array(reader.result);
          const base64 = btoa(new Uint8Array(byteArray).reduce((data, byte) => data + String.fromCharCode(byte), ''));
          //const data = `data:${file.type};base64,${base64}`;

          await fetch("https://localhost:7019/api/pictures/create", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ data: base64, name: file.name, folderId: folderId})
          })
            .then(response => console.log(response.json()))
            .then(data => console.log(data))
            .catch(error => console.log(error));
            const pictures = await getPictures(URLSearchParams(window.location.search).get('folderId'));
            renderPictures(pictures);
      };
    }
    input.click();
}
