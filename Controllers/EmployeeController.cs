using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using Final_project.Models;

public class EmployeeController : Controller
{
    private string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDBConnection"].ConnectionString;

    // GET: Employee
    public ActionResult Index()
    {
        List<Employee> employees = new List<Employee>();
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Employee emp = new Employee
                {
                    EmployeeID = (int)reader["EmployeeID"],
                    Name = reader["Name"].ToString(),
                    Position = reader["Position"].ToString(),
                    Age = (int)reader["Age"],
                    Salary = (decimal)reader["Salary"]
                };
                employees.Add(emp);
            }
        }
        return View(employees);
    }

    // GET: Employee/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: Employee/Create
    [HttpPost]
    public ActionResult Create(Employee employee)
    {
        // Validation logic
        if (string.IsNullOrWhiteSpace(employee.Name))
        {
            ModelState.AddModelError("Name", "Name is required.");
        }

        if (string.IsNullOrWhiteSpace(employee.Position))
        {
            ModelState.AddModelError("Position", "Position is required.");
        }

        if (employee.Age < 18 || employee.Age > 65)
        {
            ModelState.AddModelError("Age", "Age must be between 18 and 65.");
        }

        if (employee.Salary < 0)
        {
            ModelState.AddModelError("Salary", "Salary must be a positive value.");
        }

        // Check if the model state is valid before inserting into the database
        if (!ModelState.IsValid)
        {
            return View(employee); // Return the view with validation messages
        }

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Employees (Name, Position, Age, Salary) VALUES (@Name, @Position, @Age, @Salary)", con);
            cmd.Parameters.AddWithValue("@Name", employee.Name);
            cmd.Parameters.AddWithValue("@Position", employee.Position);
            cmd.Parameters.AddWithValue("@Age", employee.Age);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        return RedirectToAction("Index");
    }

    // GET: Employee/Edit/5
    public ActionResult Edit(int id)
    {
        Employee employee = new Employee();
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE EmployeeID = @EmployeeID", con);
            cmd.Parameters.AddWithValue("@EmployeeID", id);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                employee.EmployeeID = (int)reader["EmployeeID"];
                employee.Name = reader["Name"].ToString();
                employee.Position = reader["Position"].ToString();
                employee.Age = (int)reader["Age"];
                employee.Salary = (decimal)reader["Salary"];
            }
        }
        return View(employee);
    }

    // POST: Employee/Edit/5
    [HttpPost]
    public ActionResult Edit(Employee employee)
    {
        // Validation logic
        if (string.IsNullOrWhiteSpace(employee.Name))
        {
            ModelState.AddModelError("Name", "Name is required.");
        }

        if (string.IsNullOrWhiteSpace(employee.Position))
        {
            ModelState.AddModelError("Position", "Position is required.");
        }

        if (employee.Age < 18 || employee.Age > 65)
        {
            ModelState.AddModelError("Age", "Age must be between 18 and 65.");
        }

        if (employee.Salary < 0)
        {
            ModelState.AddModelError("Salary", "Salary must be a positive value.");
        }

        // Check if the model state is valid before updating the database
        if (!ModelState.IsValid)
        {
            return View(employee); // Return the view with validation messages
        }

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("UPDATE Employees SET Name=@Name, Position=@Position, Age=@Age, Salary=@Salary WHERE EmployeeID=@EmployeeID", con);
            cmd.Parameters.AddWithValue("@Name", employee.Name);
            cmd.Parameters.AddWithValue("@Position", employee.Position);
            cmd.Parameters.AddWithValue("@Age", employee.Age);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);
            cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        return RedirectToAction("Index");
    }

    // GET: Employee/Delete/5
    public ActionResult Delete(int id)
    {
        Employee employee = new Employee();
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE EmployeeID = @EmployeeID", con);
            cmd.Parameters.AddWithValue("@EmployeeID", id);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                employee.EmployeeID = (int)reader["EmployeeID"];
                employee.Name = reader["Name"].ToString();
                employee.Position = reader["Position"].ToString();
                employee.Age = (int)reader["Age"];
                employee.Salary = (decimal)reader["Salary"];
            }
        }
        return View(employee);
    }

    // POST: Employee/Delete/5
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Employees WHERE EmployeeID=@EmployeeID", con);
            cmd.Parameters.AddWithValue("@EmployeeID", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        return RedirectToAction("Index");
    }
}
