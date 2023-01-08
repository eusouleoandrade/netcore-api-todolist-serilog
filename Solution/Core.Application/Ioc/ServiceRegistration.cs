using Core.Application.Interfaces.UseCases;
using Core.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Core.Application.Ioc
{
    [ExcludeFromCodeCoverage]
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IGetAllTodoUseCase, GetAllTodoUseCase>();
            services.AddScoped<ICreateTodoUseCase, CreateTodoUseCase>();
            services.AddScoped<IDeleteTodoUseCase, DeleteTodoUseCase>();
            services.AddScoped<IGetTodoUseCase, GetTodoUseCase>();
            services.AddScoped<IUpdateTodoUseCase, UpdateTodoUseCase>();
            services.AddScoped<ISetDoneTodoUseCase, SetDoneTodoUseCase>();
        }
    }
}