using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace Bits.Domain.Validators
{
    public class TaskValidator : AbstractValidator<Task>
    {
        public TaskValidator()
        {
            RuleFor(x => x.Name).Length(1, 255).NotEmpty();
        }
    }
}