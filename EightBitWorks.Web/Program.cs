using EightBitWorks.Web.Services.Mail;

var builder = WebApplication.CreateBuilder(args);


// add mail service
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

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

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        // redirect user to 404 page in case resource is not available
        context.Request.Path = "/HttpCode404";
        await next();
    }
});

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
/*
app.MapControllerRoute(
    name: "status-code-redirect",
    pattern: "home/redirect/404",
    defaults: new { controller = "Home", action = "StatusCodeRedirect" });
*/
app.MapControllerRoute(
    name: "thank-you",
    pattern: "thank-you/",
    defaults: new { controller = "Home", action = "ThankYou" });

app.MapControllerRoute(
    name: "docker-course",
    pattern: "about-the-author/",
    defaults: new { controller = "Home", action = "AboutAuthor" });

app.MapControllerRoute(
    name: "docker-course",
    pattern: "courses/master-in-docker-technologies/",
    defaults: new { controller = "Home", action = "DockerCourse" });

app.MapControllerRoute(
    name: "kubernetes-course",
    pattern: "courses/from-zero-to-hero-kubernetes-technologies/",
    defaults: new { controller = "Home", action = "KubernetesCourse" });

app.MapControllerRoute(
    name: "default",
    pattern: "{action=Home}",
    defaults: new { controller = "Home", action = "Home" });

app.Run();