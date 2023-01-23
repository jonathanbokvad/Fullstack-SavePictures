const form = document.getElementById("create-user");
//check that form is defined and not null, also check if form has a addeventlistener method
if (form && form.addEventListener) {
form.addEventListener('submit', function (e) {
    e.preventDefault();
    const payload = new FormData(form);

    const password1 = payload.get("password1");
    const password2 = payload.get("password2");
    if(password1 !== password2){
        return alert("Both passwords need to match each other!")
    }
 
    fetch("https://localhost:7019/api/user/createacc", {
        method : "POST",
        body : JSON.stringify({
            "username" : payload.get("username"),
            "password" : password1
        }),
        headers : {
            "Content-Type": "application/json",
        }})
        .then(response => {
            if (response.ok) {
              return response.json();
            }
            throw new Error("Request failed");
        })
        .then(data => {
            if (data) {
              alert("The account has been created!");
              window.location.href = "../index.html";
            }
        })
        .catch(error => {
            console.log(error);
            alert("An error occurred. Could be that the user already exists, please try again.");
        });
})}
