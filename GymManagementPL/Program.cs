using GymManagementBLL;
using GymManagementBLL.Services.AttachmentService;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Context;
using GymManagementDAL.DataSeed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfiles()));
			builder.Services.AddScoped<ISessionRepository,SessionRepository>();
            builder.Services.AddScoped<IAnalyticService, AnalyticService>();
			builder.Services.AddScoped<IAccountService, AccountService>();
			//builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericReository<>));
			//builder.Services.AddScoped<IPlanRepository, PlanRepository>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IMemberService , MemberService>();
			builder.Services.AddScoped<IPlanService, PlanService>();
			builder.Services.AddScoped<ISessionService, SessionService>();
			builder.Services.AddScoped<ITrainerService, TrainerService>();
			builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<GymDbContext>();
			var app = builder.Build();
            #region DataSeeding
            using var Scope = app.Services.CreateScope();
            var dbContext = Scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var PendingMigrations = dbContext.Database.GetPendingMigrations();
            if(PendingMigrations?.Any() ??  false)
                dbContext.Database.Migrate();
            GymDbContextSeeding.SeedData(dbContext);
            IDentityDbContextSeeding.SeedData(roleManager, userManager);

            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
