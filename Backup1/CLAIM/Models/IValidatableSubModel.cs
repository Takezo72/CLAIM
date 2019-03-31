using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CLAIM.Models
{
    public interface IValidatableSubModel
    {
        IEnumerable<ValidationResult> Validate(string instanceName, bool isRequired = false);
    }
}

