﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkinAppBackend.Storage;
using SkinAppBackend.Services;
using Microsoft.AspNetCore.Cors;

namespace SkinAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly MyContext _context;
        Database db = new Database();
        Hashing hashing = new Hashing();
        EmailValidator validateEmail = new EmailValidator();
        string errorMessage = "";
        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpPost("[action]")]
        public int Login([FromBody] string[] loginData)
        {
            bool userCheck = false;
            int userId = 0;
            userCheck = db.ValidateUser(_context, loginData[0], loginData[1], ref errorMessage);
            if (userCheck == true)
                userId = db.GetUserId(_context, loginData[0]);
            return userId;
        }
        [HttpPost("[action]")]
        public bool CreateUser([FromBody] string[] userData)
        {
            bool couldCreateUser = false;
            if (userData[0] != null && userData[1] != null)
            {
                bool validEmail = validateEmail.Validate(userData[0]);

                // If both fields are filled and the email is a valid email address, it goes in to try and create the user.
                if (validEmail == true)
                    couldCreateUser = db.CreateUser(_context, userData[0], hashing.HashPassword(userData[1], "new"));
            }
            return couldCreateUser;
        }
    }
}