using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Simplifly.Context;
using Simplifly.Exceptions;
using Simplifly.Interfaces;
using Simplifly.Models;
using Simplifly.Repositories;
using Simplifly.Services;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using Route = Simplifly.Models.Route;

namespace Simplifly
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });



                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
            });

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer(options =>
                  {
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuerSigningKey = true,
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"])),
                          ValidateIssuer = false,
                          ValidateAudience = false
                      };
                  });

            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("ReactPolicy", opts =>
                {
                    opts.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
                });
            });

            #region context

            builder.Services.AddDbContext<RequestTrackerContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("requestTrackerConnection"));
            });

            #endregion

            #region RepositoryInjection 

            builder.Services.AddScoped<IRepository<int,Airport>,AirportRepository>();
            builder.Services.AddScoped<IRepository<int,Booking>,BookingsRepository>();
            builder.Services.AddScoped<IRepository<string,Flight>,FlightRepository>();
            builder.Services.AddScoped<IRepository<int,FlightOwner>,FlightOwnerRepository>();
            builder.Services.AddScoped<IRepository<int, Admin>, AdminRepository>();
            builder.Services.AddScoped<IRepository<int, Customer>, CustomerRepository>();
            builder.Services.AddScoped<IRepository<int, PassengerBooking>, PassengerBookingRepository>();
            builder.Services.AddScoped<IRepository<int, Passenger>, PassengerRepository>();
            builder.Services.AddScoped<IRepository<int, CancelledBooking>, CancelBookingRepository>();

            builder.Services.AddScoped<IRepository<int, Payment>,PaymentRepository>();
            builder.Services.AddScoped<IRepository<string, User>, UserRepository>();
            builder.Services.AddScoped<IRepository<int,Route>,RouteRepository>();
            builder.Services.AddScoped<IRepository<int, Schedule>, ScheduleRepository>();
            builder.Services.AddScoped<IRepository<string, SeatDetail>, SeatDetailRepository>();
            builder.Services.AddScoped<IPassengerBookingRepository,PassengerBookingRepository>();
            builder.Services.AddScoped<IBookingRepository ,BookingsRepository>();
           // builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<ISeatDeatilRepository, SeatDetailRepository>();

            #endregion

            #region Service Injection

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IFlightFlightOwnerService,FlightService>();
            builder.Services.AddScoped<IFlightCustomerService,ScheduleServices>();
            builder.Services.AddScoped<IRouteFlightOwnerService, RouteService>();
            builder.Services.AddScoped<IScheduleFlightOwnerService,ScheduleServices>();
            builder.Services.AddScoped<IFlightCustomerService ,ScheduleServices>();
            builder.Services.AddScoped<IUserService, USerService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IFlightOwnerService, FlightOwnerService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IPassengerService, PassengerService>();
            builder.Services.AddScoped<ISeatDetailService, SeatDetailService>();
            builder.Services.AddScoped<IAdminService, AdminService>();


            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("ReactPolicy");
            app.UseAuthentication();
            app.UseAuthorization();           


            app.MapControllers();

            app.Run();
        }
    }
}
