using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using login_reg.Models;
using DbConnection;
using Microsoft.AspNetCore.Http;

namespace login_reg.Controllers
{
    public class HomeController : Controller
    {
        private string message;

        public IActionResult Index(string message)
        {
            if (!string.IsNullOrEmpty(message))
            TempData["Message"] = message;
            return View();
        }

        public IActionResult Success(string message)
        {
            string user=HttpContext.Session.GetString("user");
            ViewBag.user=user;
            return View("Success");
        }
        [HttpPost]
        [Route("User/register")]
        public IActionResult register(User user)

        {   
            if(ModelState.IsValid){
                var found=DbConnector.Query($"SELECT * FROM quotes WHERE Email='{user.Email}'");
                if(found.Count()>0){
                    return RedirectToAction("Index",new{message="There is already another user with this email!"});
                }
                DbConnector.Execute($"INSERT INTO quotes(First_Name,Last_Name,Email,Password) VALUES ('{user.First_Name}','{user.Last_Name}','{user.Email}','{user.Password}')");
                    found=DbConnector.Query($"SELECT * FROM quotes WHERE Email='{user.Email}'");
                    var userr=found[0]["First_Name"].ToString();
                    HttpContext.Session.SetString("user",userr);                                    
                return RedirectToAction("Success",new{message=userr});
            }
           
            return View("Index");
        }
        [HttpPost]
        [Route("login")]
        public IActionResult login(string email,string password){
            var found=DbConnector.Query($"SELECT * FROM quotes WHERE Email='{email}'");
               string error="";
                if(found.Count()>0){
                    string pass=found[0]["Password"].ToString();
                    if(pass==password){
                        message="You have successfully logged in!";
                        HttpContext.Session.SetString("message",message);
                        var log_user=found[0]["First_Name"].ToString();
                        HttpContext.Session.SetString("user",log_user);
                        return RedirectToAction("Success");
                    }else error="Password you entered,does not match what we have on file!";
                }else{
                    error="Email you entered,does not match what we have on file!";

                }
                return RedirectToAction("Index",new{message=error});  
        }

        
    }
}
