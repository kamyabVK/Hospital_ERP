using Microsoft.AspNetCore.Mvc;
using Backend_Api.Model;
using System.Data;
using MySql.Data.MySqlClient;


namespace Backend_Api.Controllers
{
   

    [ApiController]
    [Route("api/[controller]")]
    public class PatinetRegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PatinetRegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ========================
        // POST: api/Patient/Insert
        // ========================
        [HttpPost("Insert")]
        public IActionResult Insert([FromBody] PatientRegistration request)
        {
            using var connection = new MySqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            using var cmd = new MySqlCommand("sp_PatientRegistration", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("p_Mode", "INSERT");
            cmd.Parameters.AddWithValue("p_FirstName", request.FirstName);
            cmd.Parameters.AddWithValue("p_LastName", request.LastName);
            cmd.Parameters.AddWithValue("p_MobileNumber", request.MobileNumber);
            cmd.Parameters.AddWithValue("p_DateOfBirth", request.DateOfBirth);
            cmd.Parameters.AddWithValue("p_Gender", request.Gender);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return Ok(new
                {
                    Success = Convert.ToInt32(reader["Success"]),
                    Message = reader["Message"].ToString(),
                    PatientID = reader["PatientID"] == DBNull.Value
                                ? null
                                : (int?)Convert.ToInt32(reader["PatientID"])
                });
            }

            return BadRequest(new { Success = 0, Message = "Something went wrong." });
        }

        // ========================
        // GET: api/Patient/Followup?mobileNumber=9999988888
        [HttpGet("Followup")]
        public IActionResult Followup([FromQuery] string mobileNumber)
        {
            using var connection = new MySqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            using var cmd = new MySqlCommand("sp_PatientRegistration", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("p_Mode", "FOLLOWUP");
            cmd.Parameters.AddWithValue("p_FirstName", DBNull.Value);
            cmd.Parameters.AddWithValue("p_LastName", DBNull.Value);
            cmd.Parameters.AddWithValue("p_MobileNumber", mobileNumber);
            cmd.Parameters.AddWithValue("p_DateOfBirth", DBNull.Value);
            cmd.Parameters.AddWithValue("p_Gender", DBNull.Value);

            using var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                return NotFound(new
                {
                    Success = 0,
                    Message = "No patient found with this mobile number."
                });
            }

            if (reader.Read())
            {
                return Ok(new
                {
                    PatientID = Convert.ToInt32(reader["PatientID"]),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    MobileNumber = reader["MobileNumber"].ToString(),
                    DateOfBirth = reader["DateOfBirth"].ToString(),
                    Gender = reader["Gender"].ToString(),
                    CreatedAt = reader["CreatedAt"].ToString()
                });
            }

            return BadRequest(new { Success = 0, Message = "Something went wrong." });
        }
    }
}
