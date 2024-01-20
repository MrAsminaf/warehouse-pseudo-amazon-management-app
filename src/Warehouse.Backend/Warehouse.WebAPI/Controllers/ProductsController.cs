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

    /// <summary>
    /// Returns a list of all products in the system
    /// </summary>
    /// <returns>A list of all products</returns>
    /// <response code="200">Returns a list of all products</response>
    /// <response code="400">If exception occured</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<List<ReturnProductModel>> GetAllProducts()
    {
        try
        {
            var products = _dbContext.Products.ToList();
            var productsModel = _mapper.Map<List<ReturnProductModel>>(products);
            
            return Ok(productsModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return NotFound();
        }
    }

    /// <summary>
    /// Returns a product by its ID.
    /// </summary>
    /// <response code="200">Returns the product</response>
    /// <response code="204">If no such product exists</response>
    /// <response code="400">If exception occurred</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnProductModel>> GetById(int id)
    {
        try
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            var productModel = _mapper.Map<ReturnProductModel>(product);

            return Ok(productModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return NotFound();
        }
    }

    /// <summary>
    /// Creates a product.
    /// </summary>
    /// <returns>A newly created product</returns>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="404">If exception occurred</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnProductModel>> CreateProduct(CreateProductModel model)
    {
        try
        {
            var mappedProduct = _mapper.Map<Product>(model);
            
            var createdProduct = await _dbContext.Products.AddAsync(mappedProduct);
            await _dbContext.SaveChangesAsync();
            var productModel = _mapper.Map<ReturnProductModel>(createdProduct.Entity);

            return CreatedAtAction(nameof(GetById), new { createdProduct?.Entity?.Id }, 
                productModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return BadRequest();
        }
    }
    
    /// <summary>
    /// Modifies a product found by ID.
    /// </summary>
    /// <returns>A modified product</returns>
    /// <response code="200">If product was successfully modified</response>
    /// <response code="400">If no such product exists or an exception occurred</response>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    
    /// <summary>
    /// Deletes a product found by ID.
    /// </summary>
    /// <response code="204">If product was successfully deleted</response>
    /// <response code="400">If no such product exists or an exception occurred</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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