using BookRight.Application.UseCases.PractitionerUseCases;
using BookRight.Domain.Entities.Practitioners;
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
            

            services.AddScoped<IUpdatePractitioner, UpdatePractitionerHandler>();
            services.AddScoped<ICreatePractitioner, CreatePractitionerHandler>();
            services.AddScoped<IDeletePractitioner, DeletePractitionerHandler>();



            return services;
        }
    }
}
