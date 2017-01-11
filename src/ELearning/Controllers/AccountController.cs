﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ELearning.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ELearning.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ELearning.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var exists = _context.UniversityUsers.FirstOrDefaultAsync(x => x.email == model.Email);
            if (ModelState.IsValid && exists == null)
            {
                UniversityUser universityUser = new UniversityUser();
                universityUser.Id = Guid.NewGuid();
                universityUser.Firstname = model.FirstName;
                universityUser.Lastname = model.LastName;
                universityUser.email = model.Email;
                universityUser.Password = model.Password;
                universityUser.Type = "student";
                _context.Add(universityUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _context.UniversityUsers.SingleOrDefaultAsync(m => m.email == model.Email);
                if (user == null)
                {
                    return View();
                }
                else if (user.Password == model.Password)
                {
                    HttpContext.Session.SetString("Email", user.email);
                    HttpContext.Session.SetString("FirstName", user.Firstname);
                    HttpContext.Session.SetString("LastName", user.Lastname);
                    HttpContext.Session.SetString("Type", user.Type);
                    return RedirectToAction("Index", "Home");

                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet, ActionName("GetAll")]
        public IActionResult GetAll()
        {
            List<string> sessionVar = new List<string>();
            sessionVar.Add(HttpContext.Session.GetString("Email"));
            sessionVar.Add(HttpContext.Session.GetString("FirstName"));
            sessionVar.Add(HttpContext.Session.GetString("LastName"));
            sessionVar.Add(HttpContext.Session.GetString("Type"));
            return new ObjectResult(sessionVar);
        }


    }
}
