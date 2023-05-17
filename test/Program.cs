var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DbContext, TestContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:Text"]));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/test-get", (DbContext db) =>
{
    var students = db.Set<Student>().ToList();
    return students;
});

app.MapGet("/add-test", (DbContext db) =>
{
    var student = new Student { Name = "unknow" };
    db.Set<Student>().Add(student);
    db.SaveChanges();
});
app.MapGet("/update-text", (DbContext db) =>
{
    var students = db.Set<Student>().Where(i => i.Name == "unknow").ToList();
    students.ForEach(i => i.Name = "knew");
    db.SaveChanges();
});

app.MapGet("/delete-test", (DbContext db) =>
{
    var students = db.Set<Student>().ToList();
    db.Set<Student>().RemoveRange(students);
    db.SaveChanges();
});

app.Run();
