public class ProjectController
{
    private readonly ProjectService _projectService;

    public ProjectController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    public List<Project> GetAllProjects()
    {
        return _projectService.GetAllProjects();
    }

    public Project? GetProjectById(int projectId)
    {
        return _projectService.GetProjectById(projectId);
    }

    public void CreateProject(Project project)
    {
        _projectService.AddProject(project);
    }

    public void EditProject(Project project)
    {
        _projectService.UpdateProject(project);
    }

    public bool DeleteProject(int projectId)
    {
        var existing = _projectService.GetProjectById(projectId);
        if (existing == null)
            return false;
        _projectService.DeleteProject(projectId);
        return true;
    }
}
