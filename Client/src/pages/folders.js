
window.addEventListener('load', () => {
    renderFolders();
});

async function getFolders() {
    try {
      const userId = localStorage.getItem("currentUser");
      const response = await fetch(`https://localhost:7019/api/folder?userId=${userId}`);
      const data = await response.json();
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
  for (const folder of folders){
    tbody += `<tr class="folder hover:bg-blue-800 cursor-pointer" data-folder-id="${folder.id}">
  <td class="p-3 ">
  ${folder.name}
      </td>
      <td class="p-3">
      ${folder.pictures.length}
      </td>
      <td class="p-3">
      ${folder.creationDate.substring(0, 10)}
      </td>
      <td class="p-3">
        <button onclick="showModal(this, 'modal-update')" class="text-gray-400 hover:text-yellow-500 mx-2 edit">
          <i class="material-icons-outlined text-base edit-icon">edit</i>
        </button>
        <button onclick="deleteFolder(this)" class="text-gray-400 hover:text-red-600 ml-2 delete">
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
    const folderElements = document.querySelectorAll('.folder');
    for (const folderElement of folderElements) {
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
    body: JSON.stringify({name: inputValue, userId: localStorage.getItem("currentUser")})
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

async function deleteAccount(){
  if(!confirm("Are you sure you want to delete your account?")){
    return;
  }
await fetch(`https://localhost:7019/api/user?userId=${localStorage.getItem("currentUser")}`, { method: "Delete"})
.then(response => response.json())
.then(data => data)
.catch(error => console.error(error));
window.location.href = "../index.html";
}