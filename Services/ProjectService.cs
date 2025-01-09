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

    public List<Project> GetAllProjects()
    {
        return _context.Projects
            .Include(p => p.ResponsibleEmployee)
            .Include(p => p.AssociatedClient)
            .ToList();
    }

    public Project? GetProjectById(int projectId)
    {
        return _context.Projects
            .Include(p => p.ResponsibleEmployee)
            .Include(p => p.AssociatedClient)
            .FirstOrDefault(p => p.ID == projectId);
    }

    public void AddProject(Project project)
    {
        _context.Projects.Add(project);
        _context.SaveChanges();
    }

    public void UpdateProject(Project project)
    {
        _context.Projects.Update(project);
        _context.SaveChanges();
    }

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
