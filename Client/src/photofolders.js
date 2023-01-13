


window.addEventListener('load', () => {
    renderFolders();
});

async function getFolders() {
    try {
      const response = await fetch('https://localhost:7019/api/folder');
      console.log(response);
      const data = await response.json();
      console.log(data);
      return data;
    } catch (error) {
      console.error(error);
    }
}


async function renderFolders() {
    const folders = await getFolders();
    const folderContainer = document.querySelector('.folder-section');

    let tbody = '';
    for (const folder of folders) {
      tbody += `<tr class="folder hover:bg-blue-600" data-folder-id="${folder.id}">
      <td class="p-3">
      ${folder.name}
        </td>
      <td class="p-3">
      ${folder.numberOfPictures}
      </td>
      <td class="p-3">
      ${folder.createdAt}
      </td>
      <td class="p-3">
        <a href="#" class="text-gray-400 hover:text-gray-100  mx-2">
          <i class="material-icons-outlined text-base">edit</i>
        </a>
        <a href="#" class="text-gray-400 hover:text-gray-100  ml-2">
          <i class="material-icons-round text-base">delete</i>
        </a>
      </td>
    </tr>`;
      
    }
    folderContainer.innerHTML = tbody;
      // html += `<div class="bg-gray-200 rounded-lg p-4 flex items-center justify-between hover:bg-gray-300 cursor-pointer" data-folder-id="${folder.id}">
      //            <div class="text-lg">${folder.name}</div>
      //            <div class="daisy-icon daisy-icon-folder"></div>
      //         </div>`;

  //tbody += `<div class="bg-gray-200 rounded-lg p-4 flex items-center justify-between hover:bg-gray-300 cursor-pointer" data-folder-id="${folder.id}"> <div class="text-lg">${folder.name}</div> <div class="daisy-icon daisy-icon-folder"></div> </div>`

    // Add event listeners to the folder elements
    const folderElements = document.querySelectorAll('.folder');
    for (const folderElement of folderElements) {
      folderElement.addEventListener('click', async function () {
        const folderId = this.getAttribute('data-folder-id');
        const pictures = await getPictures(folderId);
        renderPictures(pictures);
      });
    }
}

async function getPictures(folderId) {
    try {
        const response = await fetch(`https://localhost:7019/api/pictures/${folderId}`);
        const data = await response.json();
        return data;
    } catch (error) {
        console.error(error);
    }
}

// function renderPictures(pictures) {
//     const pictureContainer = document.querySelector('.picture-section');

//     let html = '';
//     for (const picture of pictures) {
//       html += `<div class="picture" data-picture-id="${picture.id}">${picture.name}</div>`;
//     }
//     pictureContainer.innerHTML = html;
// }
function renderPictures(pictures) {
    const pictureContainer = document.querySelector('.picture-section');
  
    let html = '';
    for (const picture of pictures) {
      html += `<div class="w-full max-w-sm rounded overflow-hidden shadow-lg m-4 cursor-pointer" data-picture-id="${picture.id}">
                  <img class="w-full" src="${picture.url}" alt="${picture.name}">
                  <div class="px-6 py-4">
                    <div class="font-bold text-xl mb-2">${picture.name}</div>
                    <p class="text-gray-700 text-base">
                      ${picture.description}
                    </p>
                  </div>
                </div>`;
    }
    pictureContainer.innerHTML = html;
  
    // Add event listeners to the picture elements
    const pictureElements = document.querySelectorAll('.picture');
    for (const pictureElement of pictureElements) {
      pictureElement.addEventListener('click', function() {
        // Show modal window with the picture
        const modal = document.querySelector('.modal');
        modal.classList.add('block');
        modal.querySelector('.modal-body').innerHTML = this.innerHTML;
      });
    }
  }
  