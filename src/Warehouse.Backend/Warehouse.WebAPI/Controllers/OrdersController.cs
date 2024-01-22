using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Models;
using Warehouse.Domain.Entities;
using Warehouse.Infrastructure.Data;

namespace Warehouse.WebAPI.controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public OrdersController(ApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns a list of all orders in the system
    /// </summary>
    /// <returns>A list of all orders</returns>
    /// <response code="200">Returns a list of all orders</response>
    /// <response code="400">If exception occured</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<List<ReturnOrderModel>> GetAll()
    {
        try
        {
            var orders = _dbContext.Orders.ToList();
            var ordersModel = _mapper.Map<List<ReturnOrderModel>>(orders);

            return Ok(ordersModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return NotFound();
        }
    }

    /// <summary>
    /// Returns an order by its ID.
    /// </summary>
    /// <response code="200">Returns the order</response>
    /// <response code="204">If no such order exists</response>
    /// <response code="400">If exception occurred</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnOrderModel>> GetById(int id)
    {
        try
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            var orderModel = _mapper.Map<ReturnOrderModel>(order);

            return Ok(orderModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return NotFound();
        }
    }
    
    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <returns>A newly created order</returns>
    /// <response code="201">Returns the newly created order</response>
    /// <response code="404">If exception occurred</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnOrderModel>> Create(CreateOrderModel model)
    {
        try
        {
            var mappedOrder = _mapper.Map<Order>(model);
            var createdOrder = await _dbContext.Orders.AddAsync(mappedOrder);
            await _dbContext.SaveChangesAsync();
            var orderModel = _mapper.Map<ReturnOrderModel>(createdOrder.Entity);
            
            return CreatedAtAction(nameof(GetById), new { createdOrder?.Entity.Id }, 
                orderModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return BadRequest();
        }
    }
    
    // assign and de-assign worker from order
    
    // change order status
    
    // delete an order
}