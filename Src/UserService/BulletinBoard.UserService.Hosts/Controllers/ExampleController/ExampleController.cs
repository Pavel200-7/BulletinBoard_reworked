using BulletinBoard.UserService.Hosts.Controllers.ExampleController.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.UserService.Hosts.Controllers.ExampleController;

[Route("api/[controller]")]
[ApiController]
public class ExampleController : ControllerBase
{
    [HttpPost]
    public IActionResult ExampleEndpoint(ExampleRequests requests)
    {
        string text = $"{requests.Text} + {requests.Text}";
        ExampleResponse response = new ExampleResponse() { Text = text };
        return Ok(response);
    }
}
