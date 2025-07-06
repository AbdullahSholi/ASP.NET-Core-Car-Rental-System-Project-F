class Admin {
  static API_BASE = "http://localhost:5072/api/admin";

  static showAddCarForm() {
    const form = `
        <div class="admin-form">
            <h3>Add New Car</h3>
            <input type="number" id="car-year" placeholder="Year" required>
            <input type="number" id="car-price" placeholder="Daily Rate" step="0.01" required>
            <select id="car-color">
                <option value="0">Red</option>
                <option value="1">Blue</option>
                <option value="2">Yellow</option>
                <option value="3">Green</option>
                <option value="4">Cyan</option>
                <option value="5">Magenta</option>
                <option value="6">Orange</option>
                <option value="7">Pink</option>
                <option value="8">Black</option>
            </select>
            <select id="car-fuel">
                <option value="0">Petrol</option>
                <option value="1">Diesel</option>
                <option value="2">Gasoline</option>
            </select>
            <select id="car-transmission">
                <option value="0">Automatic</option>
                <option value="1">Manual</option>
            </select>
            <input type="number" id="car-seats" placeholder="Seating Capacity" required>
            <input type="number" id="car-model-id" placeholder="Car Model ID" required>
            <textarea id="car-description" placeholder="Description" rows="3"></textarea>
            <label>
                <input type="checkbox" id="car-availability"> Available
            </label>
            <button onclick="Admin.createCar()">Add Car</button>
        </div>
    `;
    const formContainer = document.getElementById("car-management-form");
    formContainer.innerHTML = form;
    formContainer.classList.remove("hidden");
  }

  static async createCar() {
    const token = localStorage.getItem("jwt");

    const carData = {
      year: parseInt(document.getElementById("car-year").value),
      color: parseInt(document.getElementById("car-color").value),
      fuelType: parseInt(document.getElementById("car-fuel").value),
      transmissionType: parseInt(
        document.getElementById("car-transmission").value
      ),
      seatingCapacity: parseInt(document.getElementById("car-seats").value),
      dailyRentalPrice: parseFloat(document.getElementById("car-price").value),
      availabilityStatus: document.getElementById("car-availability").checked,
      description: document.getElementById("car-description").value,
      createdAt: new Date().toISOString(),
      carModelId: parseInt(document.getElementById("car-model-id").value),
    };

    try {
      const response = await fetch(`${this.API_BASE}/car`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(carData),
      });

      if (!response.ok) throw new Error("Failed to add car");

      alert("Car added successfully!");
      this.loadCars();
      document.getElementById("car-management-form").classList.add("hidden");
    } catch (error) {
      alert("Error: " + error.message);
    }
  }

  static async loadCars() {
    try {
      const response = await fetch("http://localhost:5072/api/user/cars");
      const cars = await response.json();
      this.renderCars(cars);
    } catch (error) {
      console.error("Error loading cars:", error);
    }
  }

  static renderCars(cars) {
    const colorEnum = [
      "Red",
      "Blue",
      "Yellow",
      "Green",
      "Cyan",
      "Magenta",
      "Orange",
      "Pink",
      "Black",
    ];
    const fuelEnum = ["Petrol", "Diesel", "Gasoline"];
    const transmissionEnum = ["Automatic", "Manual"];

    const container = document.getElementById("cars-list-admin");
    container.innerHTML = cars
      .map(
        (car) => `
        <div class="car-card-admin">
            <div class="reservation-header">
                <span class="reservation-id">#${car.carId}</span>
                <span class="reservation-status ${
                  car.availabilityStatus ? 'status-active' : 'status-completed'
                }">${car.availabilityStatus ? 'Available' : 'Unavailable'}</span>
            </div>
            <div class="car-meta">
                <div class="detail-item">
                    <span class="detail-label">Year</span>
                    <span class="detail-value">${car.year}</span>
                </div>
                <div class="detail-item">
                    <span class="detail-label">Color</span>
                    <span class="detail-value">${colorEnum[car.color]}</span>
                </div>
                <div class="detail-item">
                    <span class="detail-label">Fuel</span>
                    <span class="detail-value">${fuelEnum[car.fuelType]}</span>
                </div>
                <div class="detail-item">
                    <span class="detail-label">Transmission</span>
                    <span class="detail-value">${
                      transmissionEnum[car.transmissionType]
                    }</span>
                </div>
                <div class="detail-item">
                    <span class="detail-label">Seats</span>
                    <span class="detail-value">${car.seatingCapacity}</span>
                </div>
                <div class="detail-item">
                    <span class="detail-label">Daily Price</span>
                    <span class="detail-value">$${car.dailyRentalPrice.toFixed(2)}</span>
                </div>
            </div>
            <div class="car-actions">
                <button class="btn-cancel" onclick="Admin.deleteCar(${car.carId})">
                    <i class="fas fa-trash"></i> Delete
                </button>
                <button class="btn-book" onclick="Admin.editCar(${car.carId})">
                    <i class="fas fa-edit"></i> Edit
                </button>
            </div>
        </div>
        `
      )
      .join("");
  }

  static closeEditModal() {
    document.getElementById('edit-car-modal').classList.add('hidden');
  }

  static async editCar(carId) {
    const token = localStorage.getItem("jwt");

    try {
      const response = await fetch(`http://localhost:5072/api/user/car/${carId}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
      });
      console.log(response.data);
      if (!response.ok) throw new Error('Failed to fetch car details');
      const car = await response.json();
      
      // Populate form fields
      document.getElementById('edit-car-id').value = carId;
      document.getElementById('edit-year').value = car.year;
      document.getElementById('edit-color').value = car.color;
      document.getElementById('edit-fuel').value = car.fuelType;
      document.getElementById('edit-transmission').value = car.transmissionType;
      document.getElementById('edit-seats').value = car.seatingCapacity;
      document.getElementById('edit-price').value = car.dailyRentalPrice;
      document.getElementById('edit-description').value = car.description;
      document.getElementById('edit-availability').checked = car.availabilityStatus;
      
      // Show modal
      document.getElementById('edit-car-modal').classList.remove('hidden');
    } catch (error) {
      alert('Error loading car details: ' + error.message);
    }
  }

  static async handleCarUpdate(event) {
    event.preventDefault();
    const carId = document.getElementById('edit-car-id').value;
    const token = localStorage.getItem("jwt");
    
    const updateData = {
      year: parseInt(document.getElementById('edit-year').value),
      color: parseInt(document.getElementById('edit-color').value),
      fuelType: parseInt(document.getElementById('edit-fuel').value),
      transmissionType: parseInt(document.getElementById('edit-transmission').value),
      seatingCapacity: parseInt(document.getElementById('edit-seats').value),
      dailyRentalPrice: parseFloat(document.getElementById('edit-price').value),
      description: document.getElementById('edit-description').value,
      availabilityStatus: document.getElementById('edit-availability').checked,
      carModelId: 1 // Static for now - update if you have model selection
    };

    try {
      const response = await fetch(`http://localhost:5072/api/admin/car/${carId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(updateData)
      });

      if (!response.ok) {
        const error = await response.text();
        throw new Error(error);
      }

      this.closeEditModal();
      this.loadCars();
      alert('Car updated successfully!');
    } catch (error) {
      alert('Update failed: ' + error.message);
    }
  }

  static async deleteCar(carId) {
    const token = localStorage.getItem("jwt");

    try {
      await fetch(`${this.API_BASE}/car/${carId}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` },
      });
      this.loadCars();
    } catch (error) {
      alert("Failed to delete car");
    }
  }
}

// Initial load of cars
Admin.loadCars();
