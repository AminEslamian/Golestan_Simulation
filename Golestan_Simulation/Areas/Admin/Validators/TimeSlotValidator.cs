using FluentValidation;
using Golestan_Simulation.ViewModels;

namespace Golestan_Simulation.Areas.Admin.Validators
{
    public class TimeSlotValidator : AbstractValidator<SectionViewModel>
    {
        public TimeSlotValidator()
        {
            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after the start time");
        }
    }
}