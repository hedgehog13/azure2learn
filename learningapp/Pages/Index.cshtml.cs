using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace learningapp.Pages;

public class IndexModel : PageModel
{
    public List<Course> Courses = new List<Course>();
    private readonly ILogger<IndexModel> _logger;
    private IConfiguration _configuration;
    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void OnGet()
    {

        // string connectionString = _configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;
        var config = _configuration.GetSection("Common:Settings");
        string? connectionString = config.GetValue<string>("dbPassword");
        var sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();

        var sqlcommand = new SqlCommand(
        "SELECT CourseID,CourseName,Rating FROM Course;", sqlConnection);
        using (SqlDataReader sqlDatareader = sqlcommand.ExecuteReader())
        {
            while (sqlDatareader.Read())
            {
                int courseId;
                int rating;
                _ = int.TryParse(sqlDatareader["CourseID"].ToString(), out courseId);
                _ = int.TryParse(sqlDatareader["Rating"].ToString(), out rating);
                Courses.Add(new Course()
                {
                    CourseID = courseId,
                    CourseName = sqlDatareader["CourseName"].ToString(),

                    Rating = rating
                });
            }
        }
    }
}
