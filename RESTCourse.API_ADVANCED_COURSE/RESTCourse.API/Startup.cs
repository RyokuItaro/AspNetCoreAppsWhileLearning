using CourseLibrary.API.DbContexts;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;

namespace CourseLibrary.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson(setupAction => //NuGet: .NewtonsoftJson
            {
                setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters()
            .ConfigureApiBehaviorOptions(setupAction => //returnowanie w headersach problemów z walidacj¹
            {
                setupAction.InvalidModelStateResponseFactory = context => //InvalidModel... - to bedzie odapalane jak modelstate.IsNotValid
                {
                    var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();        //tworzy obiekt ktory jest detailsami errorów
                    var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState); //przeksztalci z modelstate do rfc formatu

                    problemDetails.Detail = "See the errors field for details."; //dodatkowe defaultowe informacje
                    problemDetails.Instance = context.HttpContext.Request.Path;  //same

                    var actionExecutingContext = context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext; //sprawdzamy jakiego status code uzyc

                    if ((context.ModelState.ErrorCount > 0) && 
                        (actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count)) //sprawdzamy czy sa errory i czy wszystkie argumenty zostaly sparsowane
                    {                                                                                                 //wtedy nie zwrocimy 422 jak beda puste
                        problemDetails.Type = "https://courselibrary.com/modelvalidationproblem"; // ustawiamy headersy
                        problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                        problemDetails.Title = "One or more validation errors occured";

                        return new UnprocessableEntityObjectResult(problemDetails) // same
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    }
                    problemDetails.Status = StatusCodes.Status400BadRequest; // jesli byly argumenty ktorych nie sparsowano to wywala 400badrequest
                    problemDetails.Title = "One or more errors on input occured.";
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            }); 

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();

            services.AddDbContext<CourseLibraryContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=CourseLibraryDB;Trusted_Connection=True;");
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else 
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context => 
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
