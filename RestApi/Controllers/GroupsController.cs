using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Services;
using RestApi.Mappers;


[ApiController]
using RestApi.Exceptions;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.AspNetCore.Authorization;


namespace RestApi.Controllers;

[ApiController]

[Authorize]
[Route("[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;
    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }
    [HttpGet("{id}")]


    [Authorize]
    [HttpGet("{id}")]
    [Authorize(Policy = "Read")]
    public async Task<ActionResult<GroupResponse>> GetGroupById(string Id, CancellationToken cancellationToken)
    {
        var group = await _groupService.GetGroupByIdAsync(Id, cancellationToken);
        if(group is null)
        {
            return NotFound();
        }
        return Ok(group.ToDto());
    }


    public async Task<ActionResult<IEnumerable<GroupResponse>>> GetGroupsByName([FromQuery] string name, CancellationToken cancellationToken)
    {
        var groups = await _groupService.GetGroupsByNameAsync(name, cancellationToken);

    //localhost/groups
    //GET localhost/groups/ID
        //200 - retornamos el objeto
        //404 - no existe el objeto
        //400 (bad request) - error del cliente
    //PAGINACIÓN
    //GET ALL localhost/groups?name=8uudsfjakfads98f
        // 200 - retornar el listado de objetos
        // 200 - retornar el listado vacío
        // 400 - Algun campo del query parameter es invalido
    //DELETE localhost/groups/Id
        // 404 - no existe el recurso (Opcional)
        // 204 - No Content
    //POST localhost/groups {sajdkfj}
        // 400 - bad request
        // 409 - conflict (name != name)
        // 201 - Created (response del objeto con el nuevo Id)
    //PUT localhost/group/id {skdfj} -- Siempre mandas todos los campos
        // 400 - bad request
        // 409 - conflict
        // 200 - response del objeto actualizado
        // 204 - sin response
    //PATCH
        // 400 - bad request
        // 409 - conflict
        // 200 - response del objeto actualizado
        // 204 - sin response


    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupResponse>>> GetGroupsByName(
        CancellationToken cancellationToken,
        [FromQuery] string name, 
        [FromQuery] int pageIndex = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] string orderBy = "name")

    //Paginación tarea*
    [HttpGet]
    [Authorize(Policy = "Read")]
    public async Task<ActionResult<IEnumerable<GroupResponse>>> GetGroupsByName(
        [FromQuery] string name, 
        [FromQuery] int pageIndex, 
        [FromQuery] int pageSize, 
        [FromQuery] string orderBy,
        CancellationToken cancellationToken)
    {
        var groups = await _groupService.GetGroupsByNameAsync(name, pageIndex, pageSize, orderBy, cancellationToken);
        
        if(groups == null || !groups.Any())
        {
            return Ok(new List<GroupResponse>());
        }

        return Ok(groups.Select(group => group.ToDto()));
    }

        return Ok(groups.Select(group => group.ToDto()));
    }

        return Ok(groups.Select(group => group.ToDto()));
    }
    
    [HttpDelete("{id}")]
    [Authorize(Policy = "Write")]

    public async Task<IActionResult> DeleteGroup(string id, CancellationToken cancellationToken){
        try{
            await _groupService.DeleteGroupByIdAsync(id, cancellationToken);
            return NoContent();
        }catch(GroupNotFoundException){
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize(Policy = "Write")]

    public async Task<ActionResult<GroupResponse>> CreateGroup([FromBody]CreateGroupRequest groupRequest, CancellationToken cancellationToken){
        try{
            var group = await _groupService.CreateGroupAsync(groupRequest.Name, groupRequest.Users, cancellationToken);
            return CreatedAtAction(nameof(GetGroupById), new {id = group.Id}, group.ToDto());
        }catch(InvalidGroupRequestFormatException){
            return BadRequest(NewValidationProblemDetails("One or more validation errors occured.", HttpStatusCode.BadRequest, new Dictionary<string, string[]>{
                {"Groups", ["Users array is empy"]}
            }));
        }catch(GroupAlreadyExistsException){
            return Conflict(NewValidationProblemDetails("One or more validation errors occured.", HttpStatusCode.Conflict, new Dictionary<string, string[]>{
                {"Groups", ["Group with same name already exists"]}
            }));
        }catch(UserDoesNotExistsException){
            return Conflict(NewValidationProblemDetails("One or more validation errors occured.", HttpStatusCode.Conflict, new Dictionary<string, string[]>{
                {"Groups", ["User not found with the provided ID"]}
            }));
        }
    }

    private static ValidationProblemDetails NewValidationProblemDetails(string title, HttpStatusCode statusCode, Dictionary<string, string[]> errors){
        return new ValidationProblemDetails{
            Title = title,
            Status = (int) statusCode,
            Errors = errors
        };
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Write")]

    public async Task<IActionResult> UpdateGroup(string id, [FromBody] UpdateGroupRequest groupRequest, CancellationToken cancellationToken){
        try{
            await _groupService.UpdateGroupAsync(id, groupRequest.Name, groupRequest.Users, cancellationToken);
            return NoContent(); 
        }catch(GroupNotFoundException){
            return NotFound();
        }catch(InvalidGroupRequestFormatException){
            return BadRequest(NewValidationProblemDetails("One or more validation errors occured.", HttpStatusCode.BadRequest, new Dictionary<string, string[]>{
                {"Groups", ["Users array is empy"]}
            }));
        }catch(GroupAlreadyExistsException){
            return Conflict(NewValidationProblemDetails("One or more validation errors occured.", HttpStatusCode.Conflict, new Dictionary<string, string[]>{
                {"Groups", ["Group with same name already exists"]}
            }));
        }catch(UserDoesNotExistsException){
            return Conflict(NewValidationProblemDetails("One or more validation errors occured.", HttpStatusCode.Conflict, new Dictionary<string, string[]>{
                {"Groups", ["User not found with the provided ID"]}
            }));
        }
    }

}