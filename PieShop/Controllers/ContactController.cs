﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieShop.Controllers
{
    public class ContactController : Controller
    {
        public ContactController()
        {

        }
        public IActionResult Index()
        {

            return View();
        }
    }
}
