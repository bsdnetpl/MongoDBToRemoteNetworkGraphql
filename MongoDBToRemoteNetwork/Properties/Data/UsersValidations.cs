using FluentValidation;

namespace MongoDBToRemoteNetwork.Properties.Data
{
    public class UsersValidations : AbstractValidator<Users>
    {
        public UsersValidations()
        {
            RuleFor(r => r.Email).EmailAddress().WithMessage("It is bad email");
            RuleFor(r => r.Name).MinimumLength(3).MaximumLength(35).WithMessage("Name is too big or too small !");
            RuleFor(r => r.LastName).MinimumLength(3).MaximumLength(35).WithMessage("Name is too big or too small !");
            RuleFor(r => r.Password).MinimumLength(12).MaximumLength(35).WithMessage("Password is too big(35 string) or too small(12 string) !");
        }
    }
}
