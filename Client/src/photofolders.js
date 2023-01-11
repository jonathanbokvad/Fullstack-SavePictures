




// async function getFolders() {
//     try {
//       const response = await fetch('/api/folders');
//       const data = await response.json();
//       return data;
//     } catch (error) {
//       console.error(error);
//     }
// }

// async function getPictures(folderId) {
//     try {
//       const response = await fetch(`/api/pictures?folderId=${folderId}`);
//       const data = await response.json();
//       return data;
//     } catch (error) {
//       console.error(error);
//     }
// }

// async function renderFolders() {
//     const folders = await getFolders();
//     const folderContainer = document.querySelector('.folder-section');

//     let html = '';
//     for (const folder of folders) {
//       html += `<div class="folder" data-folder-id="${folder.id}">${folder.name}</div>`;
//     }
//     folderContainer.innerHTML = html;

//     // Add event listeners to the folder elements
//     const folderElements = document.querySelectorAll('.folder');
//     for (const folderElement of folderElements) {
//       folderElement.addEventListener('click', async function () {
//         const folderId = this.getAttribute('data-folder-id');
//         const pictures = await getPictures(folderId);
//         renderPictures(pictures);
//       });
//     }
// }

// function renderPictures(pictures) {
//     const pictureContainer = document.querySelector('.picture-section');

//     let html = '';
//     for (const picture of pictures) {
//       html += `<div class="picture" data-picture-id="${picture.id}">${picture.name}</div>`;
//     }
//     pictureContainer.innerHTML = html;
// }