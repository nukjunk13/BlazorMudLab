using System.Text;
using Jose;

namespace BlazorMudLab.Services
{
    public class JweService
    {
        private readonly string secretKey =
            "eZH4ED4Mx0jPnz1MCZxQ+gDtQ0OrW4Vnzazhx1rUcLuVBfNoBdwICSWqr6kBYD8h5dRxEeUkVstxpq4OnNf2zw=="; // คีย์ที่ใช้สำหรับการเข้ารหัส

        public string Encode(object payload) {
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            return JWT.Encode(payload, secretKeyBytes, JwsAlgorithm.HS512);
        }

        public string Decode(string token) {
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            return JWT.Decode(token, secretKeyBytes);
        }
    }
}
