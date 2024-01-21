using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Models;
using Warehouse.Domain.Entities;

namespace Warehouse.WebAPI.controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Creates a new client record in the system
    /// </summary>
    /// <response code="200">If a client was successfully created</response>
    /// <response code="400">If an exception occured</response>
    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp(SignUpModel model)
    {
        try
        {
            var user = _mapper.Map<Client>(model);
            user.UserName = user.Email;
            var userCreatedResult = await _userManager.CreateAsync(user, model.Password);

            if (!userCreatedResult.Succeeded) return BadRequest();
            
            var createdUser = await _userManager.FindByEmailAsync(user.Email);
            user.ApplicationUserId = createdUser.Id;
            await _userManager.UpdateAsync(user);

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }

    /// <summary>
    /// Returns a JWT bearer token for the user
    /// </summary>
    /// <returns>JWT bearer token</returns>
    /// <response code="200">Returns a JWT bearer token</response>
    /// <response code="400">If email or password is incorrect or an exception occured</response>
    [HttpPost("signin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignIn(SignInModel model)
    {
        try
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Email == model.Email);

            if (user is null)
            {
                return BadRequest();
            }

            var userSignInResult = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!userSignInResult)
            {
                return BadRequest();
            }
            
            var token = _tokenService.GenerateJwtToken(user);

            return Ok(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }
}