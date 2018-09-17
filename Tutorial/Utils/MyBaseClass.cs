using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tutorial.Utils
{
    public class MyBaseClass : ControllerBase
    {
        protected DataContext context;
        protected IConfiguration configuration;
        protected Message mess;
        protected string[] paramNames;
        protected string procedure = "";
    }
}
