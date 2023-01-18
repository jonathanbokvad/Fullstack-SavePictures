
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
  }
    folderContainer.innerHTML = tbody;
  }
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

function showModal() {
  document.getElementById("modal").classList.remove("invisible");
}
function hideModal(){
  document.getElementById("modal").classList.add("invisible");
}



async function addFolder(){
  var inputValue = document.getElementById("folderName").value;
  await fetch("https://localhost:7019/api/folder", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ name: inputValue})
  })
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error(error));
  document.getElementById("modal").classList.add("invisible");
  await renderFolders();
}