using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Models;
using Warehouse.Domain.Entities;
using Warehouse.Infrastructure.Data;

namespace Warehouse.WebAPI.controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public CartController(ApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cart>> GetCartById(int id)
    {
        try
        {
            var cart = await _dbContext.Carts.Include(c => c.Products)
                .FirstOrDefaultAsync(x => x.Id == id);

            return cart;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            return NotFound();
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<Cart>> CreateCart(CreateCartModel model)
    {
        try
        {
            var mappedCart = _mapper.Map<Cart>(model);
            var createdCart = await _dbContext.Carts.AddAsync(mappedCart);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCartById), new { createdCart?.Entity?.Id }, 
                createdCart?.Entity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            return NotFound();
        }
    }

    [HttpPost("{cartId:int}/products/{productId:int}")]
    public async Task<IActionResult> AddProductToCartById(int cartId, int productId)
    {
        try
        {
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(x => x.Id == cartId);

            if (cart == null)
            {
                return BadRequest($"Could not find cart with ID = {cartId}");
            }
            
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                return BadRequest($"Could not find product with ID = {productId}");
            }

            product.CartId = cart.Id;            
            cart.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            return NotFound();
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteCart(int id)
    {
        try
        {
            var cartToDelete = await _dbContext.Carts.FirstOrDefaultAsync(x => x.Id == id);

            if (cartToDelete == null)
            {
                return BadRequest();
            }

            _dbContext.Carts.Remove(cartToDelete);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            return NotFound();
        }
    }
    
    [HttpDelete("{cartId:int}/products/{productId:int}")]
    public async Task<IActionResult> DeleteProductFromCartById(int cartId, int productId)
    {
        try
        {
            var cart = await _dbContext.Carts.Include(c => c.Products)
                .FirstOrDefaultAsync(x => x.Id == cartId);

            if (cart == null)
            {
                return BadRequest($"Could not find cart with ID = {cartId}");
            }

            var productToDelete = cart.Products.FirstOrDefault(x => x.Id == productId);

            if (productToDelete == null)
            {
                return BadRequest($"There is no product with ID {productId} in cart with ID = {cartId}");
            }

            cart.Products.Remove(productToDelete);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            return NotFound();
        }
    }
}