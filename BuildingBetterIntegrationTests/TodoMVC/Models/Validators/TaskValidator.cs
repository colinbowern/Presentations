using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace TodoMVC.Models.Validators
{
    public class TaskValidator : AbstractValidator<Task>
    {
        public TaskValidator()
        {
            RuleFor(x => x.Name).Length(1, 255).NotEmpty();
        }
    }
}