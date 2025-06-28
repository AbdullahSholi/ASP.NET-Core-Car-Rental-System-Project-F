class Reservations {
    static API_BASE = 'http://localhost:5072/api/user';

    static showBookingForm(carId) {
        console.log(`Car Id: ${carId}`);
        const modal = document.getElementById('booking-modal');
        document.getElementById('booking-car-id').value = carId;
        modal.classList.remove('hidden');
        
        // Set up modal closing
        modal.querySelector('.close-modal').onclick = () => modal.classList.add('hidden');
        window.onclick = (event) => {
            if (event.target === modal) modal.classList.add('hidden');
        };
    }

    static async submitBooking(event) {
        event.preventDefault();
        const token = localStorage.getItem('jwt');
        const userId = localStorage.getItem('userId');
        const modal = document.getElementById('booking-modal');
        const carId = document.getElementById('booking-car-id').value;
        const startDate = document.getElementById('start-date').value;
        const endDate = document.getElementById('end-date').value;

        try {
            const response = await fetch(`${this.API_BASE}/reservation`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({
                    userId,
                    carId,
                    startDate,
                    endDate
                })
            });

            if (!response.ok) throw new Error('Booking failed');
            
            alert('Reservation successful!');
            document.getElementById('booking-modal').classList.add('hidden');
            document.getElementById('booking-form').reset();
            this.loadUserReservations();
        } catch (error) {
            alert('Reservation failed: ' + error.message);
        }
    }

    static async loadUserReservations() {
        const token = localStorage.getItem('jwt');
        const userId = localStorage.getItem('userId'); // You must save this after login

        if (!userId) {
            alert("User ID not found in local storage.");
            return;
        }

        try {
            const response = await fetch(`${this.API_BASE}/reservations/${userId}`, {
                headers: { 'Authorization': `Bearer ${token}` }
            });

            if (!response.ok) throw new Error(`HTTP ${response.status}: Failed to fetch reservations`);

            const reservations = await response.json();
            console.log(reservations);
            this.renderReservations(reservations);
        } catch (error) {
            console.error('Error loading reservations:', error);
        }
    }

    static renderReservations(reservations) {
        const container = document.getElementById('reservations-list');

        if (reservations.length === 0) {
            container.innerHTML = '<p>No reservations found.</p>';
            return;
        }

        container.innerHTML = reservations.map(res => `
            <div class="reservation-card">
                <div class="reservation-header">
                    <span class="reservation-id">#${res.reservationId}</span>
                    <span class="reservation-status status-active">Active</span>
                </div>
                <div class="reservation-details">
                    <div class="detail-item">
                        <span class="detail-label">Car ID</span>
                        <span class="detail-value">${res.carId}</span>
                    </div>
                    <div class="detail-item">
                        <span class="detail-label">Dates</span>
                        <span class="detail-value">${new Date(res.startDate).toLocaleDateString()} – ${new Date(res.endDate).toLocaleDateString()}</span>
                    </div>
                    <div class="detail-item">
                        <span class="detail-label">Total Price</span>
                        <span class="detail-value">$${res.totalPrice.toFixed(2)}</span>
                    </div>
                </div>
                <div class="reservation-actions">
                    <button class="btn-cancel" onclick="Reservations.cancelReservation(${res.reservationId})">
                        Cancel Reservation
                    </button>
                </div>
            </div>
        `).join('');
    }

    static async cancelReservation(reservationId) {
        const token = localStorage.getItem('jwt');
        
        try {
            await fetch(`${this.API_BASE}/reservation/${reservationId}`, {
                method: 'DELETE',
                headers: { 'Authorization': `Bearer ${token}` }
            });

            this.loadUserReservations();
        } catch (error) {
            alert('Failed to cancel reservation');
        }
    }
}
