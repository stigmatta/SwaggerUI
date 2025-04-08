using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwaggerUI.Models;

namespace SwaggerUI.Controllers;

public class TeacherContext : DbContext
{
    public DbSet<Teacher> Teachers { get; set; }
    public TeacherContext(DbContextOptions<TeacherContext> options)
        : base(options)
    {
        if (Database.EnsureCreated())
        {
            Teachers.Add(new Teacher { Name = "Олег", Surname = "Петренко", Age = 45, Subject = "Історія" });
            Teachers.Add(new Teacher { Name = "Марина", Surname = "Шевченко", Age = 38, Subject = "Біологія" });
            Teachers.Add(new Teacher { Name = "Андрій", Surname = "Ковальчук", Age = 50, Subject = "Географія" });

            SaveChanges();
        }
    }
}

[ApiController]
[Route("api/Teachers")]
public class TeacherController : ControllerBase
{
    private readonly TeacherContext _context;

    public TeacherController(TeacherContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
    {
        var teachers = await _context.Teachers.ToListAsync();
        if (teachers == null)
            return NotFound();
        Console.WriteLine(teachers);
        return Ok(teachers);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Teacher>> GetTeacher(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);

        if (teacher == null)
            return NotFound();

        return teacher;
    }


    [HttpPut]
    public async Task<IActionResult> PutTeacher(Teacher teacher)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!_context.Teachers.Any(e => e.Id == teacher.Id))
            return NotFound();

        _context.Update(teacher);
        await _context.SaveChangesAsync();


        return Ok(teacher);

    }

    [HttpPost]
    public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        return Ok(teacher);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Teacher>> DeleteTeacher(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var teacher = await _context.Teachers.SingleOrDefaultAsync(m => m.Id == id);

        if (teacher == null)
            return NotFound();

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();

        return Ok(teacher);
    }
}
