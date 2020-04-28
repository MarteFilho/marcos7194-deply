using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("users")]
    public class UserController : ControllerBase
    {

        //Lista todos os usuários
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "adm")]
        public async Task<ActionResult<List<User>>> Get(
            [FromServices]DataContext context
            )
        {
            try
            {
                var users = await context.Users.AsNoTracking().ToListAsync();
                if (users == null)
                    return NotFound(new { Erro = "Não foi encontrado nenhum usuário!" });
                return Ok(users);
            }
            catch (Exception)
            {

                return BadRequest(new { Erro = "Não foi encontrado nenhum usuário!" });
            }
        }

        //Cria um usuário
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post(
            [FromServices]DataContext context,
            [FromBody]User user
            )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                user.Password = "";
                return user;
            }
            catch (Exception)
            {

                return BadRequest(new { Erro = "Não foi possível criar o usuário!" });
            }
        }

        [HttpPost]
        [Route("login")]
        
        //Autenticação, login
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromBody]User model,
            [FromServices]DataContext context
            )
        {
            var user = await context
                .Users
                .AsNoTracking()
                .Where(x =>
                x.Username == model.Username &&
                x.Password == model.Password)
                .FirstOrDefaultAsync();
            if (user == null)
                return NotFound(new { Erro = "Usuário ou senha inválidos!" });

            var token = TokenServices.GenerateToken(user);
            user.Password = "";

            return new
            {
                user = user,
                token = token
            };
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "adm")]
        public async Task<ActionResult<User>> Put(
            int id,
            [FromServices] DataContext context,
            [FromBody]User model

            )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != model.Id)
                return BadRequest(new {Erro = "Usuário não encontrado!" });

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {

                return BadRequest(new { Erro = "Não foi possível editar o usuário" });
            }
        }

    }
}
