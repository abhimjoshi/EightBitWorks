using EightBitWorks.Web.Services.Mail;
using Microsoft.AspNetCore.ResponseCompression;
using WebMarkupMin.AspNetCore7;

var builder = WebApplication.CreateBuilder(args);

// add mail service
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

// Add services to the container.
builder.Services.AddOutputCache();

// minify html
// ref: https://github.com/Taritsyn/WebMarkupMin/wiki/ASP.NET-Core-6
// the reference is for .NET 6, but it is exactly same for .NET 7 too.
builder.Services.AddWebMarkupMin(o =>
    {
        o.AllowCompressionInDevelopmentEnvironment = true;
        o.AllowMinificationInDevelopmentEnvironment = true;
    })
    .AddHtmlMinification()
    .AddHttpCompression();

builder.Services.AddControllersWithViews();
builder.Services.AddResponseCompression( options => {
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "text/javascript", "text/css"}
    );
});

// minify css and js files
builder.Services.AddWebOptimizer(pipeline =>
    {
        pipeline.MinifyCssFiles("/assets/css/style.css", "/assets/css/fontawesome.css");
    },
    option =>
    {
        option.EnableCaching = true;
        option.EnableDiskCache = false;
        option.EnableMemoryCache = true;
        option.AllowEmptyBundle = true;
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseWebOptimizer();
app.UseStaticFiles();

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
app.UseWebMarkupMin();
app.UseRouting();
app.UseOutputCache();

#region --- Routing ---

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

#endregion

app.Run();