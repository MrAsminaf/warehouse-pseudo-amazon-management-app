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

    /// <summary>
    /// Returns a cart by its ID.
    /// </summary>
    /// <response code="200">Returns the cart</response>
    /// <response code="204">If no such cart exists</response>
    /// <response code="400">If exception occurred</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnCartModel>> GetCartById(int id)
    {
        try
        {
            var cart = await _dbContext.Carts.Include(c => c.Products)
                .FirstOrDefaultAsync(x => x.Id == id);
            var cartModel = _mapper.Map<ReturnCartModel>(cart);
                
            return Ok(cartModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            return BadRequest();
        }
    }
    
    /// <summary>
    /// Creates a cart.
    /// </summary>
    /// <returns>A newly created cart</returns>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">If exception occurred</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnCartModel>> CreateCart(CreateCartModel model)
    {
        try
        {
            var mappedCart = _mapper.Map<Cart>(model);
            var createdCart = await _dbContext.Carts.AddAsync(mappedCart);
            await _dbContext.SaveChangesAsync();
            var cartModel = _mapper.Map<ReturnCartModel>(createdCart.Entity);

            return CreatedAtAction(nameof(GetCartById), new { createdCart?.Entity?.Id }, 
                cartModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            return BadRequest();
        }
    }

    /// <summary>
    /// Add a product with productId to cart with cartId.
    /// </summary>
    /// <response code="200">If product was successfully added</response>
    /// <response code="400">If no cart or product was found or an exception occurred</response>
    [HttpPost("{cartId:int}/products/{productId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            
            return BadRequest();
        }
    }
    
    /// <summary>
    /// Deletes a whole cart by its ID.
    /// </summary>
    /// <response code="204">If cart was successfully deleted</response>
    /// <response code="400">If no such cart exists or an exception occurred</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            
            return BadRequest();
        }
    }
    
    /// <summary>
    /// Deletes a product with productId from cart with cartId.
    /// </summary>
    /// <response code="204">If product was successfully deleted</response>
    /// <response code="400">If no such cart or product exists or an exception occurred</response>
    [HttpDelete("{cartId:int}/products/{productId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            return BadRequest();
        }
    }
}