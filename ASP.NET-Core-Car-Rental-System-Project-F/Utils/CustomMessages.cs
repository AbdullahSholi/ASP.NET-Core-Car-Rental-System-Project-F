namespace ASP.NET_Core_Car_Rental_System_Project_F.Utils;

public static class CustomMessages
{
    public const string UnSetSecretKey = "SECRET_KEY environment variable is not set.";
    public const string UnSetConnectionString = "CAR_RENTAL_CONNECTION_STRING environment variable is not set.";
    public const string UnSetAppPassword = "APP_PASSWORD environment variable is not set.";
    public const string InvalidCredentials = "Invalid credentials.";
    public const string InvalidEmailAddress = "Invalid email address.";
    public const string EmailSentSuccessfully = "Email sent successfully.";
    public const string InvalidOtp = "Invalid otp.";
    public const string PasswordResetSuccessfully = "Password reset successfully.";
    public const string YourOtpCode = "Your OTP Code";
    public const string DuplicatedEmail = "Duplicated email";
    public const string InvalidToken = "Invalid token";
    public const string LoggedOutSuccessfully = "Logged out successfully";
    public const string TokenIsBlacklisted = "Token is blacklisted.";
    public const string InvalidCarInformation = "Invalid car information.";
    public const string AddingNewCarError = "Error occurred while adding new car.";
    public const string InternalServerError = "An internal error occurred.";
    public const string UpdatingCarError = "Error occurred while updating car with ID {CarId}";
    public const string DeletingCarError = "Error occurred while deleting car with ID {CarId}";
    public const string ListingCarsError = "Error occurred while listing cars.";
    public const string GettingCarError = "Error occurred while getting car with ID {CarId}";
    public const string CarNotFound = "Car not found.";
    public const string BookCarError = "Error occurred while booking car with ID {CarId}";
    public const string FailedToBookCar = "Failed to book car.";
    public const string FailedToRemoveReservation = "Failed to remove reservation.";
}