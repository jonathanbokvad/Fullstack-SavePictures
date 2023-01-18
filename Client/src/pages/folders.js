
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
        <button onclick="showModal(this, 'modal-update')" class="text-gray-400 hover:text-gray-100 mx-2 edit">
          <i class="material-icons-outlined text-base edit-icon">edit</i>
        </button>
        <button onclick="deleteFolder(this)" class="text-gray-400 hover:text-gray-100  ml-2 delete">
          <i class="material-icons-round text-base delete-icon ">delete</i>
        </button>
        </td>
    </tr>`;
  } folderContainer.innerHTML = tbody;
  }
  document.querySelectorAll('.edit, .delete').forEach(elem => {
    elem.addEventListener('click', function(event) {
        event.stopPropagation();
    });
});
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

let currentFolderId = "";
function showModal(element, modal) {
  document.getElementById(modal).classList.remove("invisible");

  if(modal == 'modal-update'){
    currentFolderId = element.parentElement.parentElement.getAttribute("data-folder-id");
  }
}

function hideModal(modal, folderName){
  document.getElementById(modal).classList.add("invisible");
  document.getElementById(folderName).value = "";
}

async function addFolder(){
  var inputValue = document.getElementById("folderNameCreate").value;
  await fetch("https://localhost:7019/api/folder", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({folderId: currentFolderId, name: inputValue})
  })
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error(error));
  hideModal('modal', 'folderNameCreate');
  await renderFolders();
}

async function updateFolderName(){
  var inputValue = document.getElementById("folderNameUpdate").value;
  await fetch("https://localhost:7019/api/folder", {
    method: "PATCH",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({folderId: currentFolderId, name: inputValue})
  })
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error(error));
  hideModal('modal-update', 'folderNameUpdate');
  await renderFolders();
}

async function deleteFolder(element){
  const folderId = element.parentElement.parentElement.getAttribute("data-folder-id");
  console.log("This is the Delete Folder function!")
  await fetch("https://localhost:7019/api/folder", {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(folderId)
  })
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error(error));
  hideModal('modal-update', 'folderNameUpdate');
  await renderFolders();
}