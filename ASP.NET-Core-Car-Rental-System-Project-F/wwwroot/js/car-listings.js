class CarListings {
  static API_BASE = "http://localhost:5072/api/user";

  static async searchCars() {
    console.log("=== searchCars called ===");
    
    try {
      // Wait for DOM to be ready
      await new Promise(resolve => setTimeout(resolve, 200));
      
      console.log("=== DOM Check ===");
      
      // Check if cars section is active
      const carsSection = document.getElementById("cars-section");
      console.log("Cars section found:", !!carsSection);
      console.log("Cars section active:", carsSection?.classList.contains('active'));
      
      // Check each element individually
      const colorElem = document.getElementById("filter-color");
      const yearElem = document.getElementById("search-year");
      const fuelElem = document.getElementById("filter-fuel");
      const transmissionElem = document.getElementById("filter-transmission");
      const pageNumberElem = document.getElementById("page-number");
      const pageSizeElem = document.getElementById("page-size");

      console.log("Element check:");
      console.log("- filter-color:", !!colorElem, colorElem);
      console.log("- search-year:", !!yearElem, yearElem);
      console.log("- filter-fuel:", !!fuelElem, fuelElem);
      console.log("- filter-transmission:", !!transmissionElem, transmissionElem);
      console.log("- page-number:", !!pageNumberElem, pageNumberElem);
      console.log("- page-size:", !!pageSizeElem, pageSizeElem);

      // If any critical elements are missing, show error
      if (!colorElem || !yearElem || !fuelElem || !transmissionElem) {
        const container = document.getElementById("cars-container");
        if (container) {
          container.innerHTML = `
            <div class="error-message">
              <h3>DOM Elements Missing</h3>
              <p>Some search filter elements are not found in the DOM.</p>
              <p>This might be a timing issue or the HTML structure is different.</p>
              <ul>
                <li>filter-color: ${!!colorElem}</li>
                <li>search-year: ${!!yearElem}</li>
                <li>filter-fuel: ${!!fuelElem}</li>
                <li>filter-transmission: ${!!transmissionElem}</li>
              </ul>
              <button onclick="CarListings.searchCars()">Try Again</button>
            </div>
          `;
        }
        return;
      }

      const query = new URLSearchParams();
      
      // Safely access values with detailed logging
      console.log("=== Value Check ===");
      if (colorElem && colorElem.value) {
        console.log("Color value:", colorElem.value);
        query.append("color", colorElem.value);
      }
      if (yearElem && yearElem.value) {
        console.log("Year value:", yearElem.value);
        query.append("year", yearElem.value);
      }
      if (fuelElem && fuelElem.value) {
        console.log("Fuel value:", fuelElem.value);
        query.append("fuelType", fuelElem.value);
      }
      if (transmissionElem && transmissionElem.value) {
        console.log("Transmission value:", transmissionElem.value);
        query.append("transmissionType", transmissionElem.value);
      }
      if (pageNumberElem && pageNumberElem.value) {
        console.log("Page number value:", pageNumberElem.value);
        query.append("pageNumber", pageNumberElem.value);
      }
      if (pageSizeElem && pageSizeElem.value) {
        console.log("Page size value:", pageSizeElem.value);
        query.append("pageSize", pageSizeElem.value);
      }

      const token = localStorage.getItem("jwt");
      console.log("Token:", token ? "Present" : "Missing");
      console.log("Search query:", query.toString());
      
      const url = `${this.API_BASE}/search?${query.toString()}`;
      console.log("Request URL:", url);
      console.log(token);
      
      const response = await fetch(url, {
        headers: {
          Authorization: `Bearer ${token || ""}`,
        },
      });

      console.log("Response status:", response.status);
      console.log("Response ok:", response.ok);

      if (!response.ok) {
        const errorText = await response.text();
        console.error("API Error:", errorText);
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }
      
      const cars = await response.json();
      console.log("Received cars:", cars);
      console.log("Cars count:", Array.isArray(cars) ? cars.length : 'Not an array');
      
      this.renderCars(cars);
    } catch (error) {
      console.error("Error in searchCars:", error);
      console.error("Error stack:", error.stack);
      
      // Show user-friendly error message
      const container = document.getElementById("cars-container");
      if (container) {
        container.innerHTML = `
          <div class="error-message">
            <h3>Error Loading Cars</h3>
            <p><strong>Error:</strong> ${error.message}</p>
            <p><strong>Details:</strong> ${error.stack ? error.stack.split('\n')[0] : 'No stack trace'}</p>
            <button onclick="CarListings.searchCars()">Try Again</button>
          </div>
        `;
      }
    }
  }

  static renderCars(cars) {
    console.log("=== renderCars called ===");
    const container = document.getElementById("cars-container");
    if (!container) {
      console.warn("[renderCars] #cars-container not found in DOM");
      return;
    }
    
    if (!Array.isArray(cars)) {
      console.error("Cars data is not an array:", cars);
      container.innerHTML = "<p>Error: Invalid data received from server.</p>";
      return;
    }
    
    if (cars.length === 0) {
      container.innerHTML = "<p>No cars found. Try adjusting your search filters.</p>";
      return;
    }

    const ColorMap = [
      "Red", "Blue", "Yellow", "Green", "Cyan", "Magenta", "Orange", "Pink", "Black"
    ];
    const FuelTypeMap = ["Petrol", "Diesel", "Gasoline"];
    const TransmissionTypeMap = ["Automatic", "Manual"];

    container.innerHTML = cars
      .map((car) => {
        const transmission = TransmissionTypeMap[car.transmissionType] || "Unknown";
        const fuel = FuelTypeMap[car.fuelType] || "Unknown";
        const color = ColorMap[car.color] || "Unknown";
        const image = car.imageUrl ? `/images/${car.imageUrl}` : "/images/default-car.jpg";

        return `
            <div class="car-card">
                <img src="${image}" alt="${car.model}">
                <div class="car-card-content">
                    <h3>${car.model || 'Car Model'}</h3>
                    <div class="car-details">
                        <div class="car-detail-item">
                            <span class="detail-label">Year</span>
                            <span class="detail-value">${car.year || 'N/A'}</span>
                        </div>
                        <div class="car-detail-item">
                            <span class="detail-label">Color</span>
                            <span class="detail-value">${color}</span>
                        </div>
                        <div class="car-detail-item">
                            <span class="detail-label">Fuel</span>
                            <span class="detail-value">${fuel}</span>
                        </div>
                        <div class="car-detail-item">
                            <span class="detail-label">Transmission</span>
                            <span class="detail-value">${transmission}</span>
                        </div>
                        <div class="car-detail-item">
                            <span class="detail-label">Seats</span>
                            <span class="detail-value">${car.seatingCapacity || 'N/A'}</span>
                        </div>
                        <div class="car-detail-item">
                            <span class="detail-label">Daily Rate</span>
                            <span class="detail-value">$${car.dailyRentalPrice || 'N/A'}</span>
                        </div>
                    </div>
                    <button class="btn-book" onclick="Reservations.showBookingForm(${car.carId})" ${!car.carId ? 'disabled' : ''}>
                        <i class="fas fa-calendar-check"></i> Book Now
                    </button>
                </div>
            </div>
        `;
      })
      .join("");
      
    console.log("Cars rendered successfully");
  }

  static async loadBrandFilters() {
    console.log("=== loadBrandFilters called ===");
    const select = document.getElementById("filter-brand");
    if (!select) {
      console.warn("[loadBrandFilters] #filter-brand not found in DOM");
      return;
    }

    try {
      const token = localStorage.getItem("token");
      const response = await fetch(`${this.API_BASE}/brands`, {
        headers: {
          Authorization: `Bearer ${token || ""}`,
        },
      });
      
      if (!response.ok) {
        throw new Error(`HTTP ${response.status}: ${await response.text()}`);
      }
      
      const brands = await response.json();
      console.log("Brands loaded:", brands);

      select.innerHTML =
        '<option value="">All Brands</option>' +
        brands
          .map((brand) => `<option value="${brand.id}">${brand.name}</option>`)
          .join("");
    } catch (error) {
      console.error("Error loading brands:", error);
    }
  }
}
