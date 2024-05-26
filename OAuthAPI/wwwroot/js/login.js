document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('loginForm').addEventListener('submit', function (event) {
        event.preventDefault();

        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;

        fetch('/JWToken/Authorize', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ "username" : username, "password": password })
        })
            .then(response => {
                if (response.status === 401) {
                    throw new Error('Unauthorized: Invalid username or password.');
                }
                return response.json()
            })
            .then(data => {
                if (data.token) {
                    document.getElementById('result').innerText = 'Login successful! Token: ' + data.token;
                }
                else {
                    document.getElementById('result').innerText = 'Login failed!';
                }
            })
            .catch(error => {
                document.getElementById('result').innerText = error;
            });
        
    });
});
