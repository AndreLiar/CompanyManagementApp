using System.Collections.Generic;
using System.Linq;
//access the database and interact with the Employee table
public class EmployeeService
{
    private readonly DataContext _context;

    public EmployeeService(DataContext context)
    {
        _context = context;
    }

    public List<Employee> GetAllEmployees()
    {
        return _context.Employees.ToList();
    }
//add a new employee to the database
    public void AddEmployee(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
    }
//update an employee in the database
    public void UpdateEmployee(Employee employee)
    {
        var existingEmployee = _context.Employees.Find(employee.ID);
        if (existingEmployee != null)
        {
            // Update all fields, including new ones
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Position = employee.Position;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.HireDate = employee.HireDate;
            existingEmployee.ContractType = employee.ContractType;
            existingEmployee.SocialSecurityNumber = employee.SocialSecurityNumber;
            existingEmployee.Address = employee.Address;
            existingEmployee.Email = employee.Email;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.BirthDate = employee.BirthDate;
            existingEmployee.Status = employee.Status;

            _context.SaveChanges();
        }
    }
//delete an employee from the database
    public void DeleteEmployee(int employeeId)
    {
        var employee = _context.Employees.Find(employeeId);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}
