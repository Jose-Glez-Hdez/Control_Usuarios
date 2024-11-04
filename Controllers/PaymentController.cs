using System;
using Control_Usuarios.Context;
using Control_Usuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Control_Usuarios.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController (AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    // GET: api/Payment
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
    {
        return await _context.Payments.ToListAsync();
    }

    // GET: api/Payment/{userId}
    [HttpGet("{userId}")]
    public async Task<ActionResult<Payment>> GetPaymentByUserId(int userId)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.UserId == userId);
        return payment is null? NotFound("Payment not found.") : payment;
    }
    
    // POST: api/Payment
    [HttpPost]
    public async Task<ActionResult<Payment>> CreatePayment(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPaymentByUserId", new { userId = payment.UserId }, payment);
    }

    // PUT: api/Payment/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPayment(int id, Payment newPayment)
    {
        if (newPayment == null) return NotFound("Payment data is null.");
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null) return NotFound("Payment not found.");
        SetDefaultValues(newPayment);
        UpdateValues(payment, newPayment);
        _context.Entry(payment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok(payment);
    }

    // DELETE: api/Payment/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<Payment>> DeletePayment(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null) return NotFound("Payment not found.");
        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();
        return Ok(payment);
    }

    private void SetDefaultValues (Payment payment) 
    {
        payment.Amount = payment.Amount < 0? 0 : payment.Amount;
        payment.DatePay = payment.DatePay == DateTime.MinValue? DateTime.Now : payment.DatePay;
        payment.PaymentMethod ??= string.Empty;
        payment.UserId = payment.UserId < 0? 0 : payment.UserId;
        payment.TypeMembershipId = payment.TypeMembershipId < 0? 0 : payment.TypeMembershipId;

    }

    private void UpdateValues (Payment payment, Payment newPayment)
    {
        payment.Amount = newPayment.Amount!= default? newPayment.Amount : payment.Amount;
        payment.DatePay = newPayment.DatePay!= DateTime.MinValue? newPayment.DatePay : payment.DatePay;
        payment.PaymentMethod =!string.IsNullOrEmpty(newPayment.PaymentMethod)? newPayment.PaymentMethod : payment.PaymentMethod;
        payment.UserId = newPayment.UserId!= 0? newPayment.UserId : payment.UserId;
        payment.TypeMembershipId = newPayment.TypeMembershipId!= 0? newPayment.TypeMembershipId : payment.TypeMembershipId;
    }
}
