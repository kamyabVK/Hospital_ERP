using Microsoft.AspNetCore.Mvc;
using Backend_Api.Model;
using System.Data;
using MySql.Data.MySqlClient;



namespace Backend_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController: ControllerBase
    {
        private readonly string _connectionString;

        public DoctorController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: api/doctor
        [HttpGet]
        public ActionResult<IEnumerable<Doctor>> GetAll()
        {
            List<Doctor> doctors = new List<Doctor>();

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM doctor_emp", con);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    doctors.Add(new Doctor
                    {
                        EmployeeId = Convert.ToInt32(reader["employee_id"]),
                        FirstName = reader["first_name"].ToString(),
                        LastName = reader["last_name"].ToString(),
                        Email = reader["email"].ToString(),
                        Department = reader["department"].ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        HireDate = Convert.ToDateTime(reader["hire_date"])
                    });
                }
            }

            return Ok(doctors);
        }


        //Post Method
        // POST: api/doctor
        [HttpPost]
        public ActionResult<int> AddOrUpdate(Doctor doctor)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                // INSERT CASE — EmployeeId 0 hai
                if (doctor.EmployeeId == 0)
                {
                    // Check karo email + department already exist karta hai ya nahi
                    MySqlCommand checkCmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM doctor_emp WHERE email = @Email AND department = @Department", con);
                    checkCmd.Parameters.AddWithValue("@Email", doctor.Email);
                    checkCmd.Parameters.AddWithValue("@Department", doctor.Department);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                        return Ok(2);  // 2 → Already exists

                    // Insert karo
                    MySqlCommand insertCmd = new MySqlCommand(
                        @"INSERT INTO doctor_emp (first_name, last_name, email, department, hire_date, IsActive) 
                  VALUES (@FirstName, @LastName, @Email, @Department, @HireDate, @IsActive)", con);
                    insertCmd.Parameters.AddWithValue("@FirstName", doctor.FirstName);
                    insertCmd.Parameters.AddWithValue("@LastName", doctor.LastName);
                    insertCmd.Parameters.AddWithValue("@Email", doctor.Email);
                    insertCmd.Parameters.AddWithValue("@Department", doctor.Department);
                    insertCmd.Parameters.AddWithValue("@HireDate", DateTime.Now);
                    insertCmd.Parameters.AddWithValue("@IsActive", 1);
                    insertCmd.ExecuteNonQuery();
                    return Ok(0);  // 0 → Insert hua
                }

                // UPDATE CASE — EmployeeId > 0 hai
                if (doctor.EmployeeId > 0)
                {
                    MySqlCommand updateCmd = new MySqlCommand(
                        @"UPDATE doctor_emp 
                  SET first_name = @FirstName, 
                      last_name = @LastName, 
                      email = @Email, 
                      department = @Department 
                  WHERE employee_id = @EmployeeId", con);
                    updateCmd.Parameters.AddWithValue("@FirstName", doctor.FirstName);
                    updateCmd.Parameters.AddWithValue("@LastName", doctor.LastName);
                    updateCmd.Parameters.AddWithValue("@Email", doctor.Email);
                    updateCmd.Parameters.AddWithValue("@Department", doctor.Department);
                    updateCmd.Parameters.AddWithValue("@EmployeeId", doctor.EmployeeId);
                    updateCmd.ExecuteNonQuery();
                    return Ok(1);  // 1 → Update hua
                }

                return Ok(-1);
            }
        }


        // PUT: api/doctor/{id}/{isActive}
        [HttpPut("{id}/{isActive}")]
        public ActionResult<int> ToggleActive(int id, bool isActive)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                // Pehle current status check karo
                MySqlCommand checkCmd = new MySqlCommand(
                    "SELECT IsActive FROM doctor_emp WHERE employee_id = @EmployeeId", con);
                checkCmd.Parameters.AddWithValue("@EmployeeId", id);
                object result = checkCmd.ExecuteScalar();

                if (result == null)
                    return NotFound();  // Employee nahi mila

                bool currentStatus = Convert.ToBoolean(result);

                // Agar same status hai to koi change nahi
                if (currentStatus == isActive)
                    return Ok(2);  // 2 → No change (already same status)

                // Update karo
                MySqlCommand updateCmd = new MySqlCommand(
                    "UPDATE doctor_emp SET IsActive = @IsActive WHERE employee_id = @EmployeeId", con);
                updateCmd.Parameters.AddWithValue("@IsActive", isActive ? 0 : 1);
                updateCmd.Parameters.AddWithValue("@EmployeeId", id);
                updateCmd.ExecuteNonQuery();

                return Ok(isActive ? 0 : 1);  // 0 → Inactive hua, 1 → Active hua
            }
        }
    }
}
