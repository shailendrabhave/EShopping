﻿using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator() 
        {
            RuleFor(o => o.UserName)
                .NotEmpty().NotNull().WithMessage("{UserName} is required")
                .MaximumLength(70).WithMessage("{UserName} must not exceed 70 characters");

            RuleFor(o => o.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required")
                .GreaterThan(-1).WithMessage("{TotalPrice} should not be negative");

            RuleFor(o => o.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required")
                .EmailAddress().WithMessage("Please enter valid email address");

            RuleFor(o => o.FirstName)
                .NotEmpty().NotNull().WithMessage("{FirstName} is required");

            RuleFor(o => o.LastName)
                .NotEmpty().NotNull().WithMessage("{LastName} is required");

        }
    }
}
