var builder = WebApplication.CreateBuilder(args);

// some comments here. Test Only.

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

#region --- default routing ---
/*
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
*/
#endregion

app.MapControllerRoute(
    name: "docker-course",
    pattern: "courses/master-in-docker-technologies/",
    defaults: new { controller = "home", action = "DockerCourse" });

app.MapControllerRoute(
    name: "kubernetes-course",
    pattern: "courses/from-zero-to-hero-kubernetes-technologies/",
    defaults: new { controller = "home", action = "KubernetesCourse" });

app.MapControllerRoute(
    name: "default",
    pattern: "{action=home}",
    defaults: new { controller = "home", action = "home" });

app.Run();