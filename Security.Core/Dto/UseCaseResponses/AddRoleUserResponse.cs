using System.Collections.Generic;
using Axity.Security.Core.Interfaces;

namespace Axity.Security.Core.Dto.UseCaseResponses
{
    public class AddRoleUserResponse : UseCaseResponseMessage 
    {
        public string Id { get; }
        public IEnumerable<string> Errors {  get; }

        public AddRoleUserResponse(IEnumerable<string> errors, bool success=false, string message=null) : base(success, message)
        {
            Errors = errors;
        }

        public AddRoleUserResponse(string id, bool success = false, string message = null) : base(success, message)
        {
            Id = id;
        }
    }
}
