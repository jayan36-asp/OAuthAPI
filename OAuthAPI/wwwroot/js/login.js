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
                  
                    localStorage.setItem('token', data.token);

                    fetchUserInfo(data.token);
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


async function fetchUserInfo(token) {
        try {
        const response = await fetch('JWToken/GetUserDetails', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error('Failed to fetch user info');
        }

        const userInfo = await response.json();

        document.getElementById('fetchedUsername').innerText = userInfo.username;
        document.getElementById('fetchedRole').innerText = userInfo.roles.join(', ');
        document.getElementById('fetchedRegion').innerText = userInfo.regions.join(', ');

        document.getElementById('userInfo').classList.remove('hidden');
    } catch (error) {
        alert(error.message);
    }
}
