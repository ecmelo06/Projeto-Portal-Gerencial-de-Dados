using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Repository;

public class ControllerUsers : Controller{
 public IActionResult Login()
    {
        return View();
    }

    public IActionResult EditarPerfil()
    {
        return View();
    }

}