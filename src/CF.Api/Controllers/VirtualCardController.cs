using System.Net;
using CF.Api.Helpers;
using CF.VirtualCard.Application.Dtos;
using CF.VirtualCard.Application.Facades.Interfaces;
using CF.VirtualCard.Domain.Exceptions;
using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CF.Api.Controllers;

[ApiController]
[Route("api/v1/virtualCard")]
public class VirtualCardController(
    ICorrelationContextAccessor correlationContext,
    ILogger<VirtualCardController> logger,
    IVirtualCardFacade virtualCardFacade) : ControllerBase
{
    [HttpGet]
    [SwaggerResponse((int)HttpStatusCode.OK, "VirtualCard successfully returned.",
        typeof(PaginationDto<VirtualCardResponseDto>))]
    public async Task<ActionResult<PaginationDto<VirtualCardResponseDto>>> Get(
        [FromQuery] VirtualCardFilterDto virtualCardFilterDto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await virtualCardFacade.GetListByFilterAsync(virtualCardFilterDto, cancellationToken);
            return result;
        }
        catch (ValidationException e)
        {
            logger.LogError(e, "Validation Exception Details. CorrelationId: {correlationId}",
                correlationContext.CorrelationContext.CorrelationId);

            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid id.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "VirtualCard not found.")]
    [SwaggerResponse((int)HttpStatusCode.OK, "VirtualCard successfully returned.")]
    public async Task<ActionResult<VirtualCardResponseDto>> Get(long id, CancellationToken cancellationToken)
    {
        try
        {
            if (id <= 0) return BadRequest(ControllerHelper.CreateProblemDetails("Id", "Invalid Id."));

            var filter = new VirtualCardFilterDto { Id = id };
            var result = await virtualCardFacade.GetByFilterAsync(filter, cancellationToken);

            if (result == null) return NotFound();

            return result;
        }
        catch (ValidationException e)
        {
            logger.LogError(e, "Validation Exception. CorrelationId: {correlationId}",
                correlationContext.CorrelationContext.CorrelationId);

            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid Request.")]
    [SwaggerResponse((int)HttpStatusCode.Created, "VirtualCard has been created successfully.")]
    public async Task<IActionResult> Post([FromBody] VirtualCardRequestDto virtualCardRequestDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var id = await virtualCardFacade.CreateAsync(virtualCardRequestDto, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpPut("{id}")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid id.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "VirtualCard not found")]
    [SwaggerResponse((int)HttpStatusCode.NoContent, "VirtualCard has been updated successfully.")]
    public async Task<IActionResult> Put(long id, [FromBody] VirtualCardRequestDto virtualCardRequestDto,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id <= 0) return BadRequest(ControllerHelper.CreateProblemDetails("Id", "Invalid Id."));

            await virtualCardFacade.UpdateAsync(id, virtualCardRequestDto, cancellationToken);
            return NoContent();
        }
        catch (EntityNotFoundException e)
        {
            logger.LogError(e, "Entity Not Found Exception. CorrelationId: {correlationId}",
                correlationContext.CorrelationContext.CorrelationId);

            return NotFound();
        }
        catch (ValidationException e)
        {
            logger.LogError(e, "Validation Exception. CorrelationId: {correlationId}",
                correlationContext.CorrelationContext.CorrelationId);

            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid id.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "VirtualCard not found.")]
    [SwaggerResponse((int)HttpStatusCode.NoContent, "VirtualCard has been deleted successfully.")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        try
        {
            if (id <= 0) return BadRequest(ControllerHelper.CreateProblemDetails("Id", "Invalid Id."));

            await virtualCardFacade.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
        catch (EntityNotFoundException e)
        {
            logger.LogError(e, "Entity Not Found Exception. CorrelationId: {correlationId}",
                correlationContext.CorrelationContext.CorrelationId);

            return NotFound();
        }
        catch (ValidationException e)
        {
            logger.LogError(e, "Validation Exception. CorrelationId: {correlationId}",
                correlationContext.CorrelationContext.CorrelationId);

            return BadRequest(e.Message);
        }
    }

	[HttpPost("withdraw")]
	[SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid Request.")]
	[SwaggerResponse((int)HttpStatusCode.Created, "Withdrawal has been succeeded.")]
	public async Task<IActionResult> Withdraw([FromBody] WithdrawRequestDto withdrawRequestDto,
		CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

        //var id = await virtualCardFacade.CreateAsync(virtualCardRequestDto, cancellationToken);

        return null;// CreatedAtAction(nameof(Get), new { id }, new { id });
	}
}