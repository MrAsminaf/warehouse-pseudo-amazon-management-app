using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Models;
using Warehouse.Domain.Entities;
using Warehouse.Infrastructure.Data;

namespace Warehouse.WebAPI.controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public ProductsController(ApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<List<Product>> GetAllProducts()
    {
        try
        {
            var products = _dbContext.Products.ToList();

            return Ok(products);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return NotFound();
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        try
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            return product;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductModel model)
    {
        try
        {
            var mappedProduct = _mapper.Map<Product>(model);
            
            var createdProduct = await _dbContext.Products.AddAsync(mappedProduct);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return BadRequest();
        }
    }
    
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody]JsonPatchDocument<Product> product)
    {
        var entityToUpdate = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (entityToUpdate == null)
        {
            return BadRequest();
        }
        
        product.ApplyTo(entityToUpdate);
        _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return new ObjectResult(entityToUpdate);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var productToDelete = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (productToDelete == null)
        {
            return BadRequest();
        }

        _dbContext.Products.Remove(productToDelete);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}