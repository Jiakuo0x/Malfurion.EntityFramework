var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TestContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:Text"]));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/add-test", (TestContext db) =>
{
    var student = new Student { Name = "unknow" };
    db.Set<Student>().Add(student);
    db.SaveChanges();
});
app.MapGet("/update-text", (TestContext db) =>
{
    var students = db.Set<Student>().Where(i => i.Name == "unknow").ToList();
    students.ForEach(i => i.Name = "knew");
    db.SaveChanges();
});

app.Run();
