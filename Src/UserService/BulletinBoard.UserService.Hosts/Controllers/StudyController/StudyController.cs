using BulletinBoard.UserService.AppServices.Study;
using BulletinBoard.UserService.Hosts.Controllers.ExampleController.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.UserService.Hosts.Controllers.StudyController;

[Route("api/[controller]")]
[ApiController]
public class StudyController : Controller
{
    private IStudyService _studyService;

    public StudyController(IStudyService studyService)
    {
        _studyService = studyService;
    }

    [HttpGet("{someArgument}")]
    public IActionResult StudyEndpoint([FromRoute] string someArgument)
    {
        _studyService.DoSomeThing(someArgument);
        return Ok("Да все норм, не мельтиши");
    }
}
