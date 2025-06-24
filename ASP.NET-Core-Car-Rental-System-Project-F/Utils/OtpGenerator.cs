using System.Text;

namespace ASP.NET_Core_Car_Rental_System_Project_F.Utils;

public class OtpGenerator
{
    public static string GenerateOtp(int length = 6)
    {
        var rng = new Random();
        var otp = new StringBuilder();
        for (var i = 0; i < length; i++) otp.Append(rng.Next(0, 10));
        return otp.ToString();
    }
}