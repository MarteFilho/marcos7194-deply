using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
   
        [Route("v1/Categories")]
        public class CategoryController : ControllerBase
        {

            //Retorna uma lista de categorias através do List
            [HttpGet]
            [Route("")]
            public async Task<ActionResult<List<Category>>> Get(
                [FromServices]DataContext context
                )
            {

                try
                {
                    var categories = await context.Categories.AsNoTracking().ToListAsync();

                    if (categories == null)
                        return NotFound(new { erro = "Não foi possível encontrar as categorias!" });

                    return Ok(categories);
                }
                catch (Exception)
                {

                    return BadRequest(new { Erro = "Não foi possível encontrar as categorias!" });
                }
            }

            //Parametros por url
            //Colocar entre chaves na rota o nome do paramêtro e restringi-lo ao tipo
            //Retorna uma categoria pelo id
            [HttpGet]
            [Route("{id:int}")]
            public async Task<ActionResult<Category>> GetById(
                int id,
                [FromServices]DataContext context)
            {

                try
                {
                    var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                    if (category == null)
                        return NotFound(new { erro = "Não foi possível encontrar a categoria!" });

                    return Ok(category);
                }
                catch (Exception)
                {
                    return BadRequest(new { Erro = "Não foi possível encontrar as categorias!" });
                }
            }

            //Cria uma categoria 
            [HttpPost]
            [Route("")]
            public async Task<ActionResult<List<Category>>> Post(
                [FromBody]Category model,
                [FromServices] DataContext context
                )
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                try
                {
                    context.Categories.Add(model);
                    await context.SaveChangesAsync();
                    return Ok(model);
                }
                catch
                {

                    return BadRequest(new { erro = "Não foi possível criar a categoria!" });
                }
            }

            [Authorize(Roles ="adm")]
            [HttpPut]
            [Route("{id:int}")]
            public async Task<ActionResult<List<Category>>> Put(
                int id,
                [FromBody] Category model,
                [FromServices] DataContext context
                )
            {

                if (id != model.Id)
                    return NotFound(new { Erro = "Categoria não encontrada!" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                try
                {
                    context.Entry<Category>(model).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return Ok(model);

                }
                catch (Exception)
                {

                    return BadRequest(new { erro = "Não foi possível atualizar a categoria!" });
                }
            }

            [HttpDelete]
            [Route("{id:int}")]
            [Authorize(Roles ="adm")]

            public async Task<ActionResult<List<Category>>> Delete(
                int id,
                [FromServices]DataContext context
                )
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new { erro = "Categoria não encontrada!" });
                try
                {
                    context.Categories.Remove(category);
                    await context.SaveChangesAsync();
                    return Ok(new { Mensagem = "Categoria excluída com sucesso!" });
                }
                catch (Exception)
                {

                    return BadRequest(new { erro = "Não foi possível excluir a categoria!" });
                }
            }
        }
    }
