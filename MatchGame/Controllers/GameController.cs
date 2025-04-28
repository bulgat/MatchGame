using AutoMapper;
using MatchGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MatchGame.Controllers
{
    public class GameController : Controller
    {

        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetNumber()
        {

            return Json(" ок");
        }


        }
}