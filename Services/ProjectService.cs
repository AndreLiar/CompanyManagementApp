using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class ProjectService
{
    private readonly DataContext _context;

    public ProjectService(DataContext context)
    {
        _context = context;
    }
//get all projects from the database
    public List<Project> GetAllProjects()
    {
        return _context.Projects
            .Include(p => p.ResponsibleEmployee)
            .Include(p => p.AssociatedClient)
            .ToList();
    }
//get a project by its ID
    public Project? GetProjectById(int projectId)
    {
        return _context.Projects
            .Include(p => p.ResponsibleEmployee)
            .Include(p => p.AssociatedClient)
            .FirstOrDefault(p => p.ID == projectId);
    }
//add a new project to the database
    public void AddProject(Project project)
    {
        _context.Projects.Add(project);
        _context.SaveChanges();
    }
//update a project in the database
    public void UpdateProject(Project project)
    {
        _context.Projects.Update(project);
        _context.SaveChanges();
    }
//delete a project from the database
    public void DeleteProject(int projectId)
    {
        var project = _context.Projects.Find(projectId);
        if (project != null)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }
    }
}
