using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phidelis_api.Models;

namespace phidelis_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna a lista com todos os Estudantes matriculados.
        /// É possivel utilizar o parametro nome para buscar um aluno
        /// </summary>
        /// <param name="Jose"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents(string name)
        {
            return await _context.Students.Where(a => name == null || a.Name.Contains(name)).ToListAsync();
        }

        /// <summary>
        /// Edita os dados de um aluno. Não é possivel editar a matricula do aluno.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Students
        ///     {
        ///         "registration": 34,
        ///         "name": "Maria",
        ///         "docNumber": "32153543123",
        ///         "dateTimeRegistration": "2022-07-26T01:22:51.552"
        ///     }
        ///    
        /// </remarks>
        /// <returns></returns>
        [HttpPut("{registration}")]
        public async Task<IActionResult> PutStudent(int registration, Student student)
        {
            if (registration != student.Registration)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(registration))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Realiza a matricula de um aluno. Só é necessário enviar o nome do aluno e o número de documento.
        /// </summary>
        /// <param name="student"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Students
        ///     {
        ///         "name": "Maria",
        ///         "docNumber": "32153543123",
        ///     }
        ///    
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            student.DateTimeRegistration = DateTime.Now;
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { registration = student.Registration }, student);
        }

        /// <summary>
        /// Realiza o DELETE do aluno.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{registration}")]
        public async Task<IActionResult> DeleteStudent(int registration)
        {
            var student = await _context.Students.FindAsync(registration);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Registration == id);
        }
    }
}
