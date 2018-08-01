using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.Utils
{
    public class Message
    {
        public int Code { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
    }

    public Message(int type = 1)
    {
        //El tipo me indica el tipo de operación
        string mess = "";
        Code = 500;
        Id = "0";         
        Description = (type == 1) ? "No fue posible efectuar la operación" : "No fue posible recuperar la información";
    }    
}
