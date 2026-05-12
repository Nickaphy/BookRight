using BookRight.Application.UseCases.PractitionerUseCases;
using BookRight.Application.UseCases.PractitionerUseCases.UpdatePractitionerStrategy;
using BookRight.Application.UseCases.PractitionerUseCases.UpdatePractitionerStrategy.UpdatePractitioner;
using BookRight.Application.UseCases.UpdateStrategy;
using BookRight.Facade.Dtos.PractitionerCommand;
using BookRight.Facade.Commands.Practitioner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.DependencyInjection.PractitionerDependencyInjection
{
    public static class PractitionerDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUpdateStrategy<Practitioner>,UpdatePractitionerEmailHandler>();
            services.AddScoped<IUpdateStrategy<Practitioner>,UpdatePractitionerPhoneNumber>();
            services.AddScoped<IUpdateStrategy<Practitioner>,UpdatePractitionerAuthorization>();
            services.AddScoped<IUpdateStrategy<Practitioner>,UpdatePractitionerNameHandler>();

            services.AddScoped<IUpdatePractitioner, UpdatePractitionerHandler>();
            services.AddScoped<ICreatePractitioner, CreatePractitionerHandler>();
            services.AddScoped<IDeletePractitioner, DeletePractitionerHandler>();



            return services;
        }
    }
}
