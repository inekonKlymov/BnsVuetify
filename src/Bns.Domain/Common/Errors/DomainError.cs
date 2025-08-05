using FluentResults;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bns.Domain.Common.Errors;

public class DomainError(string message) : Error(message)
{
}

public class NotFoundError(string message) : Error(message)
{
}

public class UnauthorizedError(string message) : Error(message)
{
}

public class ValidationError(string message) : Error(message)
{
}
public class ErrorsAspMapProfile : DefaultAspNetCoreResultEndpointProfile
{
    public override ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context)
    {
        var result = context.Result;

        if (result.HasError<UnauthorizedError>(out var unauthorizedErrors))
        {
            return new ObjectResult(unauthorizedErrors.Select(s => new { Message = s.Message, Metadata = s.Metadata, Reasons = s.Reasons }))
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
        if (result.HasError<NotFoundError>(out var notFoundErrors))
        {
            return new NotFoundObjectResult(notFoundErrors.Select(s => new { Message = s.Message, Metadata = s.Metadata, Reasons = s.Reasons }));
        }
        if (result.HasError<ValidationError>(out var validationErrors))
        {
            return new BadRequestObjectResult(validationErrors.Select(s => new { Message = s.Message, Metadata = s.Metadata, Reasons = s.Reasons }));
        }

        if (result.HasError<DomainError>(out var domainErrors))
        {
            return new BadRequestObjectResult(domainErrors.Select(s => new { Message = s.Message, Metadata = s.Metadata, Reasons = s.Reasons }));
        }

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}