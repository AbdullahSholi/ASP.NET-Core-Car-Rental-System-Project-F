class Auth {
    static API_BASE = 'http://localhost:5072/api/auth';
    static token = localStorage.getItem('jwt');

    static async logout() {
    const token = localStorage.getItem("jwt");

    try {
      const response = await fetch("http://localhost:5072/api/auth/logout", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`
        }
      });

      if (!response.ok) throw new Error("Logout failed");

      localStorage.removeItem("jwt");
      localStorage.removeItem("userId");
      Auth.token = null;
      document.body.classList.remove('authenticated');
      this.updateNavVisibility();
      alert("You have been logged out.");
      window.location.hash = "#login";
    } catch (err) {
      console.error("Logout error:", err);
      alert("Logout failed: " + err.message);
    }
  }


    static async handleLogin(event) {
        event.preventDefault();
        const email = document.getElementById('login-email').value;
        const password = document.getElementById('login-password').value;

        try {
            const response = await fetch(`${this.API_BASE}/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password })
            });

            if (!response.ok) throw new Error('Login failed');
            
            const { result } = await response.json();
            console.log(result);
            localStorage.setItem('jwt', result.token);
            localStorage.setItem('userId', result.userId);
            Auth.token = result.token; // Update static property
            this.updateNavVisibility();
            window.location.hash = '#cars';
            window.location.reload(); // Refresh to ensure nav updates
        } catch (error) {
            alert('Invalid credentials');
        }
    }

    static async handleRegister(event) {
    event.preventDefault();

    const user = {
        email: document.getElementById('register-email').value,
        password: document.getElementById('register-password').value,
        confirmPassword: document.getElementById('register-confirm-password').value,
        firstName: document.getElementById('register-first-name').value,
        lastName: document.getElementById('register-last-name').value,
        phoneNumber: document.getElementById('register-phone').value,
        dateOfBirth: document.getElementById('register-dob').value,
        address1: document.getElementById('register-address1').value,
        address2: document.getElementById('register-address2').value,
        city: document.getElementById('register-city').value,
        country: document.getElementById('register-country').value,
        driverLicense: document.getElementById('register-license').value
    };

    // Optional: client-side password match check
    if (user.password !== user.confirmPassword) {
        alert('Passwords do not match.');
        return;
    }

    try {
        const response = await fetch(`${this.API_BASE}/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(user)
        });

        if (!response.ok) {
            const error = await response.text();
            throw new Error(error);
        }

        const result = await response.json();
        alert('Registration successful! Please login.');
        window.location.hash = '#login';
    } catch (error) {
        alert('Registration failed: ' + error.message);
    }
}


    static updateNavVisibility() {
        const isLoggedIn = !!this.token;
        const isAdmin = this.token && this.parseJwt(this.token).role === 'Admin';
        
        document.body.classList.toggle('authenticated', isLoggedIn);
        document.querySelectorAll('.auth-link').forEach(el => el.style.display = isLoggedIn ? 'none' : 'inline');
        document.querySelectorAll('.protected-link').forEach(el => el.style.display = isLoggedIn ? 'inline' : 'none');
        document.querySelectorAll('.admin-link').forEach(el => el.style.display = isAdmin ? 'inline' : 'none');
    }

    static parseJwt(token) {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        return JSON.parse(atob(base64));
    }
}

// Initialize auth state
Auth.updateNavVisibility();
