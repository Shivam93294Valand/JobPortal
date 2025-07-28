using FluentValidation;
using JobPortalAPI.Models;

namespace JobPortalAPI.Validaters
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.").Equal("Naimish");
            RuleFor(x => x.LastName).NotEmpty().NotEqual("Patel");
            RuleFor(x => x.Password).NotEmpty().NotNull().Length(8, 20).WithMessage("Password must be between 8 and 20 characters.");
        }
    }
}
