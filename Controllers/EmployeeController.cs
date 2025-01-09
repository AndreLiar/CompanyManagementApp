//Controllers/EmployeeController.cs

using System.Collections.Generic;

public class EmployeeController
{
    private readonly EmployeeService _employeeService;

    public EmployeeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public List<Employee> GetEmployees()
    {
        return _employeeService.GetAllEmployees();
    }

    public void AddEmployee(Employee employee)
    {
        _employeeService.AddEmployee(employee);
    }

    public void UpdateEmployee(Employee employee)
    {
        _employeeService.UpdateEmployee(employee);
    }

    public void DeleteEmployee(int employeeId)
    {
        _employeeService.DeleteEmployee(employeeId);
    }
}
