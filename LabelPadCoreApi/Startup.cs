using System.Text;
using LabelPad.Domain.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using LabelPad.Repository.RegisterManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using System.IO;
using Microsoft.Extensions.FileProviders;
using LabelPad.Repository.RoleManagement;
using LabelPad.Repository.UserManagement;
using LabelPad.Repository.PermissionManagement;
using LabelPad.Repository.SiteManagment;
using LabelPad.Repository.VehicleRegistrationManagement;
using LabelPad.Repository.TenantManagement;
using LabelPad.Repository.VisitorParkingManagement;
using LabelPad.Repository.ReportManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LabelPad.Repository.OperatorManagement;

namespace LabelPadCoreApi
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
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddCors();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder.WithOrigins("http://localhost:4200")
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials());
            //});
            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),

                    };
                });
          
            services.AddDbContextPool<LabelPadDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("LabelPadCoreApi")));
            services.AddScoped<IRegisterRepository, RegisterRepository>();
          //  services.AddScoped<IDomainNameRepository, DomainNameRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
           // services.AddScoped<IDataSourceRepository, DataSourceRepository>();
          //  services.AddScoped<IDataTypeRepository, DataTypeRepository>();
           // services.AddScoped<IAnnotationTypeRepository, AnnotationTypeRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
          //  services.AddScoped<IProjectRepository, ProjectRepository>();
         //   services.AddScoped<ITeamRepository, TeamRepository>();
        //    services.AddScoped<ILabelClassRepository, LabelClassRepository>();
         //   services.AddScoped<IDatasetRepository, DatasetRepository>();
            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<IVehicleRegistrationRepository, VehicleRegistrationRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IVisitorParkingRepository, VisitorParkingRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IOperatorRepository, OperatorRepository>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Smart Permit API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors(a => a.SetIsOriginAllowed(s => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"EmailHtml")),
                RequestPath = new PathString("/EmailHtml")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
       Path.Combine(Directory.GetCurrentDirectory(), @".well-known")),
                RequestPath = new PathString("/.well-known")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"Uploads")),
                RequestPath = new PathString("/Uploads")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
       Path.Combine(Directory.GetCurrentDirectory(), @"ProfileImages")),
                RequestPath = new PathString("/ProfileImages")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
Path.Combine(Directory.GetCurrentDirectory(), @"TenantResidencyFiles")),
                RequestPath = new PathString("/TenantResidencyFiles")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
Path.Combine(Directory.GetCurrentDirectory(), @"TenantIdentityProofFiles")),
                RequestPath = new PathString("/TenantIdentityProofFiles")
            });
            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            
     
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Permit Api");
            });


        }
    }
}
