using Microsoft.AspNetCore.Mvc;

namespace Axity.Security.Api.Presenters
{
  public sealed class JsonContentResult : ContentResult
  {
    public JsonContentResult()
    {
      ContentType = "application/json";
    }
  }
}
