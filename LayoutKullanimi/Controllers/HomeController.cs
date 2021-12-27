﻿using LayoutKullanimi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LayoutKullanimi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new List<Urun>()
            {
                new Urun()
                {
                    Id = 1,
                    UrunAdi="Elma",
                    Fiyat=5
                },
                new Urun()
                {
                    Id=2,
                    UrunAdi="Armut",
                    Fiyat=7
                }
            };
            ViewBag.Title = "Ana Sayfa";
            return View(model);
        }
    }
}
