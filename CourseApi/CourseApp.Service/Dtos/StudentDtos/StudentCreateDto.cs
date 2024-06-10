﻿using FluentValidation;

namespace CourseApi.Dtos.StudentDtos
{
    public class StudentCreateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public int CourseId { get; set; }
    }



    public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
    {
        public StudentCreateDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(50).MinimumLength(5);
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.CourseId).NotNull().GreaterThanOrEqualTo(0);

        }
    }
}



