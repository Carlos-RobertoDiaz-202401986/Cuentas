using Cuentas.Data;
using Cuentas.Models;
using Cuentas.Models.Cuentas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cuentas.Controllers
{
    public class MovimientoController : Controller
    {
        private readonly CuentasContext _context;

        public MovimientoController(CuentasContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            return View(_context.Movimiento.ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var movimiento = _context.Movimiento
                .FirstOrDefault(m => m.Id == id);

            if (movimiento == null)
                return NotFound();

            return View(movimiento);
        }

        
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movimiento movimiento)
        {
           
            var cuenta = _context.Cuenta
                .FirstOrDefault(c => c.Id == movimiento.IdCuenta);

         
            if (cuenta == null)
            {
                ModelState.AddModelError("",
                    "La cuenta no existe.");
                return View(movimiento);
            }

            
            if (cuenta.balance < 0)
            {
                ModelState.AddModelError("",
                    "No se puede realizar ninguna transacción porque la cuenta tiene balance negativo.");
                return View(movimiento);
            }

           
            if (movimiento.Monto <= 0)
            {
                ModelState.AddModelError("Monto",
                    "El monto debe ser mayor que cero.");
                return View(movimiento);
            }

            
            if (movimiento.Tipo == 1)       
            {
                cuenta.creditos += movimiento.Monto;
                cuenta.balance += movimiento.Monto;
            }
            else if (movimiento.Tipo == 2)  
            {
                cuenta.debitos += movimiento.Monto;
                cuenta.balance -= movimiento.Monto;
            }

           
            _context.Movimiento.Add(movimiento);
            _context.Cuenta.Update(cuenta);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var movimiento = _context.Movimiento.Find(id);
            if (movimiento == null)
                return NotFound();

            return View(movimiento);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Movimiento movimiento)
        {
            if (id != movimiento.Id)
                return NotFound();

            if (movimiento.Monto <= 0)
            {
                ModelState.AddModelError("Monto",
                    "El monto debe ser mayor que cero.");
                return View(movimiento);
            }

            if (ModelState.IsValid)
            {
                _context.Update(movimiento);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(movimiento);
        }

        
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var movimiento = _context.Movimiento
                .FirstOrDefault(m => m.Id == id);

            if (movimiento == null)
                return NotFound();

            return View(movimiento);
        }
               
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movimiento = _context.Movimiento.Find(id);
            if (movimiento != null)
            {
                _context.Movimiento.Remove(movimiento);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
