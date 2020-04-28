using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{

    [Route("products")]
    public class ProductController : ControllerBase
    {
        //Lista todos os produtos
        [HttpGet]
        [Route("")]

        public async Task<ActionResult<List<Product>>> Get(
            [FromServices]DataContext context

            )
        {
            var products = await context.
                Products.
                Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();
            return products;
        }



        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        //Retorna um produto pelo id passado por url
        public async Task<ActionResult<Product>> GetById(
            [FromServices]DataContext context,
            int Id
            )
        {

            var product = await
                context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Id);
            return product;
        }

        //Lista todos os produtos de uma categoria passada por url
        [HttpGet]
        [Route("categories/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> GetByCategory(
            int id,
            [FromServices] DataContext context
            )
        {
            var products = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id)
                .ToListAsync();
            return products;
        }

        //Cria um produto
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "adm")]
        public async Task<ActionResult<Product>> Post(
            [FromBody] Product produto,
            [FromServices] DataContext context
            )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Products.Add(produto);
                    await context.SaveChangesAsync();
                    return Ok(produto);
                }
                catch (Exception)
                {

                    return BadRequest(new { erro = "Não foi possível criar o produto!" });
                }
            }
            else
                return BadRequest();
        }

    }
}