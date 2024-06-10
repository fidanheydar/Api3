
using CourseApi.Data;
using CourseApi.Dtos.CourseDtos;
using CourseApi.Dtos.StudentDtos;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CourseCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CourseUpdateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<StudentCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<StudentUpdateDtoValidator>();

builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
