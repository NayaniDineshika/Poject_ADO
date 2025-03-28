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

        public static IConfiguration Configuration { get; set; }

        private string GetCoonectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefultConnection");

        }

        public List<Employee> GetAll()
        {
            List<Employee> employeeList = new List<Employee>();
            using (_connection = new SqlConnection(GetCoonectionString()))
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
        public bool Add(Employee employee)
        {
            int id = 0;
            using (_connection = new SqlConnection(GetCoonectionString()))
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

    }
    

}
