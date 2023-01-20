const form = document.getElementById("create-user");
//check that form is defined and not null, also check if form has a addeventlistener method
if (form && form.addEventListener) {
form.addEventListener('submit', function (e) {
    e.preventDefault();
    const payload = new FormData(form);

    console.log([...payload]);

    const password1 = payload.get("password1");
    const password2 = payload.get("password2");
    if(password1 !== password2){
        return alert("Passwords need to match each other!")
    }
 
    fetch("https://localhost:7019/api/user/createacc",{
        method : "POST",
        body : JSON.stringify({
            "username" : payload.get("username"),
            "password" : password1
        }),
        headers : {
            Authorization: "Bearer ",
            Accept: "application/json",
            "Content-Type": "application/json",
        }})
        .then(response => {
            if (response.ok) {
              return response.json();
            }
            throw new Error("Request failed");
        })
        .then(data => {
            //check if is not null or undefined, this would cause error when trying to save to localstorage
            if (data) {
            // Save the token in a localstorage
              localStorage.setItem("token", `${data}`);
              console.log(data);
              window.location.href = "pages/listview.html";
            }
        })
        .catch(error => {
            console.log(`Error: ${error}`);
            alert("An error occurred. Please try again.");
        });
})}
