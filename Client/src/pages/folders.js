
window.addEventListener('load', () => {
    renderFolders();
});

async function getFolders() {
    try {
      const response = await fetch('https://localhost:7019/api/folder');
      const data = await response.json();
      console.log(data);
      return data;
    } catch (error) {
      console.error(error);
    }
}


async function renderFolders() {
  if(window.location.pathname === "/Client/src/pages/foldersview.html"){
    const folders = await getFolders();
    const folderContainer = document.querySelector('.folder-section');
  let tbody = '';
  for (const folder of folders) {
    tbody += `<tr class="folder hover:bg-blue-600 cursor-pointer" data-folder-id="${folder.id}">
  <td class="p-3 ">
  ${folder.name}
      </td>
      <td class="p-3">
      ${folder.pictures.length}
      </td>
      <td class="p-3">
      ${folder}
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
    //${folder.id.creationTime.substring(0,10)}
  }
  folderContainer.innerHTML = tbody;
  }

    // html += `<div class="bg-gray-200 rounded-lg p-4 flex items-center justify-between hover:bg-gray-300 cursor-pointer" data-folder-id="${folder.id}">
      //            <div class="text-lg">${folder.name}</div>
      //            <div class="daisy-icon daisy-icon-folder"></div>
      //         </div>`;

  //tbody += `<div class="bg-gray-200 rounded-lg p-4 flex items-center justify-between hover:bg-gray-300 cursor-pointer" data-folder-id="${folder.id}"> <div class="text-lg">${folder.name}</div> <div class="daisy-icon daisy-icon-folder"></div> </div>`

    // Add event listeners to the folder elements
    const folderElements = document.querySelectorAll('.folder');
    for (const folderElement of folderElements) {
      //console.log(folderElement);
      folderElement.addEventListener('click', function () {
        const folderId = folderElement.getAttribute('data-folder-id');
        window.location.href = '/Client/src/pages/pictureslist.html?folderId=' + folderId.toString();
      });
    }
}
