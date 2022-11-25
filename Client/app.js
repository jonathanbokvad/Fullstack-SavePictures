const form = document.getElementById('myForm');

form.addEventListener('submit', function (e) {
    e.preventDefault();
    
    const payload = new FormData(form);

    console.log([...payload]);
    
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    headers.append('Accept', 'application/json');
  
    headers.append('Access-Control-Allow-Origin', "https://localhost:7019/api");
    headers.append('Access-Control-Allow-Credentials', 'true');
  
    headers.append('GET', 'POST', 'OPTIONS');
    fetch("https://localhost:7019/api",{
        credentials: 'include',
        method : "POST",
        body : payload,
        headers : headers
    })
        .then(res => res.json())
        .then(data => console.log(data))
        .catch(err => console.log(err));
})