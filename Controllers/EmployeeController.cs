//Controllers/EmployeeController.cs

using System.Collections.Generic;

public class EmployeeController
{
    private readonly EmployeeService _employeeService;

    public EmployeeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
//get all employees
    public List<Employee> GetEmployees()
    {
        return _employeeService.GetAllEmployees();
    }
//add an employee
    public void AddEmployee(Employee employee)
    {
        _employeeService.AddEmployee(employee);
    }
//update an employee
    public void UpdateEmployee(Employee employee)
    {
        _employeeService.UpdateEmployee(employee);
    }
//delete an employee
    public void DeleteEmployee(int employeeId)
    {
        _employeeService.DeleteEmployee(employeeId);
    }
}
