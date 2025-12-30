using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;


namespace BulletinBoard.UserService.Hosts.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class ExaController : ControllerBase
{
    private readonly ILogger<ExaController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;


    public ExaController(ILogger<ExaController> logger, IMapper mapper, IMediator mediator, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    





}
