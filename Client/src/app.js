const form = document.getElementById("login");
//check that form is defined and not null, also check if form has a addeventlistener method
if (form && form.addEventListener) {
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
