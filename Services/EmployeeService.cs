using System.Collections.Generic;
using System.Linq;

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

    public void AddEmployee(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
    }

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
