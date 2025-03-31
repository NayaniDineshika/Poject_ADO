using System;
using System.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project_ado.Models;


namespace Project_ado
{
    public class Employee_DAL
    {

        SqlConnection _connection = null;
        SqlCommand _command = null;

        //public static IConfiguration Configuration { get; set; }

        //private string GetCoonectionString()
        //{
        //    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        //    Configuration = builder.Build();
        //    return Configuration.GetConnectionString("DefultConnection");

        //}

        private readonly string _connectionString;

        public Employee_DAL (IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefultConnection");
        }

        public List<Employee> GetAll()
        {
            List<Employee> employeeList = new List<Employee>();
            using (_connection = new SqlConnection(_connectionString))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_GET_Employees]";
                _connection.Open();
                SqlDataReader dr = _command.ExecuteReader();

                while (dr.Read())
                {
                    Employee employee = new Employee();
                    employee.Id = Convert.ToInt32(dr["Id"]);
                    employee.FirstName = dr["FirstName"].ToString();
                    employee.LastName = dr["LastName"].ToString(); ;
                    employee.DateOfBirth = DateOnly.FromDateTime(Convert.ToDateTime(dr["DateOfBirth"]));
                    employee.Email = dr["Email"].ToString(); ;
                    employee.Salary = Convert.ToDouble(dr["Salary"]);
                    employeeList.Add(employee);
                }
                _connection.Close();
            }
            return employeeList;
        }

        public Employee GetEmployeeeById(int id)
        {
            Employee employee = null;
            using (_connection = new SqlConnection(_connectionString))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Get_EmployeeBId]";
                // add the input value for the  stored procedure
                _command.Parameters.AddWithValue(@"Id", id);
                _connection.Open();

                SqlDataReader dr = _command.ExecuteReader();

                if (dr.Read())
                {
                    employee = new Employee()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        DateOfBirth = DateOnly.FromDateTime(Convert.ToDateTime(dr["DateOfBirth"])),
                        Email = dr["Email"].ToString(),
                        Salary = Convert.ToDouble(dr["Salary"])
                    };
                }
                _connection.Close();
            }

            return employee;
        }
        public bool Add(Employee employee)
        {
            int id = 0;
            using (_connection = new SqlConnection(_connectionString))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Insert_Employee]";
                _command.Parameters.AddWithValue(@"FirstName", employee.FirstName);
                _command.Parameters.AddWithValue(@"LastName", employee.LastName);
                _command.Parameters.AddWithValue(@"DateOfBirth", employee.DateOfBirth);
                _command.Parameters.AddWithValue(@"Email", employee.Email);
                _command.Parameters.AddWithValue(@"Salary", employee.Salary);
                _connection.Open();
                id = _command.ExecuteNonQuery();
                _connection.Close();
              
            }
            return id > 0 ? true : false;
           
        }
        public bool UpdateEmployee (Employee employee)
        {
            bool isUpdated = false;
            using (_connection = new SqlConnection(_connectionString)) 
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Update_Employee]";
                _command.Parameters.AddWithValue(@"Id", employee.Id);
                _command.Parameters.AddWithValue(@"FirstName", employee.FirstName);
                _command.Parameters.AddWithValue(@"LastName", employee.LastName);
                _command.Parameters.AddWithValue(@"DateOfBirth", employee.DateOfBirth);
                _command.Parameters.AddWithValue(@"Email", employee.Email);
                _command.Parameters.AddWithValue(@"Salary", employee.Salary);
                _connection.Open();
                try
                {
                    int rowsAffect = _command.ExecuteNonQuery();
                    isUpdated = rowsAffect > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating data: " + ex.Message);
                }

                return isUpdated;
               
            }
        }

        public bool DeleteEmployee(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_Delete_Employee", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    // Log exception
                    return false;
                }
            }
        }


    }


}
