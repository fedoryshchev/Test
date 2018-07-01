﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2.Models;
using Task2.Models.CarOwnerDB;
using Task2.Models.ViewModel;

namespace Task2.Controllers
{
    public class HomeController : Controller
    {
        CarOwnerContext db;

        public HomeController(CarOwnerContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cars()
        {
            return View(db.Cars);
        }

        public IActionResult CreateCar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCar(Car car)
        {
            db.Cars.Add(car);
            db.SaveChanges();
            return RedirectToAction("Cars");
        }

        public IActionResult EditCar(int? id)
        {
            if (id != null)
            {
                Car car = db.Cars.FirstOrDefault(x => x.Id == id);
                if (car != null)
                {
                    return View(car);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult EditCar(Car car)
        {
            db.Cars.Update(car);
            db.SaveChanges();
            return RedirectToAction("Cars");
        }

        public IActionResult DeleteCar(int? id)
        {
            if (id != null)
            {
                Car car = db.Cars.FirstOrDefault(p => p.Id == id);
                if (car != null)
                {
                    db.Cars.Remove(car);
                    db.SaveChanges();
                    return RedirectToAction("Cars");
                }
            }
            return NotFound();
        }

        public IActionResult CarOwner(int? id) // for car all owners
        {
            return Content("From Car Owner");
            if (id != null)
            {
                CarOwnerViewModel CarOwner = new CarOwnerViewModel
                {
                    Car = db.Cars.FirstOrDefault(x => x.Id == id)
                };

                if (CarOwner.Car != null)
                {
                    var ownersIds = db.CarOwners
                        .Where(carOwner => carOwner.CarId == id)
                        .Select(y => y.OwnerId)
                        .ToHashSet();
                    CarOwner.Owners = db.Owners
                        .Where(x => ownersIds.Contains(x.Id));

                    return View("PartialCarOwner", CarOwner);
                }
            }
            return NotFound();
        }

        public IActionResult Owners()
        {
            return View(db.Owners);
        }

        public IActionResult CreateOwner()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateOwner(Owner owner)
        {
            db.Owners.Add(owner);
            db.SaveChanges();
            return RedirectToAction("Owners");
        }

        public IActionResult EditOwner(int? id)
        {
            if (id != null)
            {
                Owner owner = db.Owners.FirstOrDefault(x => x.Id == id);
                if (owner != null)
                {
                    return View(owner);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult EditOwner(Owner owner)
        {
            db.Owners.Update(owner);
            db.SaveChanges();
            return RedirectToAction("Owners");
        }

        public IActionResult DeleteOwner(int? id)
        {
            if (id != null)
            {
                Owner owner = db.Owners.FirstOrDefault(p => p.Id == id);
                if (owner != null)
                {
                    db.Owners.Remove(owner);
                    db.SaveChanges();
                    return RedirectToAction("Owners");
                }
            }
            return NotFound();
        }

        public IActionResult OwnerCar(int? id) // for owners all cars
        {
            if (id != null)
            {
                var OwnerCar = db.CarOwners.Where(x => x.OwnerId == id);
                return View(OwnerCar);
            }
            return NotFound();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}