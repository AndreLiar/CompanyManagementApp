public class ProjectController
{
    private readonly ProjectService _projectService;

    public ProjectController(ProjectService projectService)
    {
        _projectService = projectService;
    }
//get all projects
    public List<Project> GetAllProjects()
    {
        return _projectService.GetAllProjects();
    }
//get a project by its ID
    public Project? GetProjectById(int projectId)
    {
        return _projectService.GetProjectById(projectId);
    }
//add a project
    public void CreateProject(Project project)
    {
        _projectService.AddProject(project);
    }
//update a project
    public void EditProject(Project project)
    {
        _projectService.UpdateProject(project);
    }
//delete a project
    public bool DeleteProject(int projectId)
    {
        var existing = _projectService.GetProjectById(projectId);
        if (existing == null)
            return false;
        _projectService.DeleteProject(projectId);
        return true;
    }
}
