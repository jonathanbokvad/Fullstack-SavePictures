const form = document.getElementById("login");

form.addEventListener('submit', function (e) {
    e.preventDefault();
    
    const payload = new FormData(form);

    console.log([...payload]);
  
    fetch("https://localhost:7019/api/user",{
        method : "POST",
        body : JSON.stringify({
            "username" : payload.get("username"),
            "password" : payload.get("password")
        }),
        headers : {
            'Authorization': 'Bearer ',
            "Accept" : "application/json",
            "Content-Type" : "application/json",
            'Access-Control-Allow-Origin' : 'true'
        }})
        .then(res => res.json())
        .then(data => console.log(data))
        .then(data => document.cookie = `access_token=[${data}]`)
        .catch(err => console.log(err));
})
