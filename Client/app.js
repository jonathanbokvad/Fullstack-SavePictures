const form = document.getElementById('myForm');

form.addEventListener('submit', function (e) {
    e.preventDefault();
    
    const payload = new FormData(form);

    console.log([...payload.values()]);

    //CorsInfo to maybe make request work
//https://codehunter.cc/a/json/react-fetch-api-getting-415-unsupported-media-type-using-post
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    headers.append('Accept', 'application/json');
  
    headers.append('Access-Control-Allow-Origin', "https://localhost:7019/api");
    headers.append('Access-Control-Allow-Credentials', '*');
  
    headers.append('GET', 'POST', 'OPTIONS');
    fetch("https://localhost:7019/api",{
        credentials: 'include',
        mode : "no-cors",
        method : "POST",
        body: {
            username: payload.get("username"),
            password: payload.get("password")
        },
        headers : {
            'Accept': 'application/json'
            
          }
    })
        .then(res => res.json())
        .then(data => console.log(data))
        .catch(err => console.log(err));
})