using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tutorial.Models;
using Tutorial.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tutorial.Controllers
{
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        DataContext context;
        Message mess = new Message();
        string[] paramNames = new string[];
        string procedure = "", badId = "El Id enviado no corresponde a ninguna persona";

        [HttpPost("manager")]
        public Message Save([Bind("Id,Firstname,Lastname,DoB,Weight")] Person person)
        {
            try
            {
                paramNames = new string[] { "ID", "FIRSTNAME", "LASTNAME", "DOB", "WEIGHT" };
                procedure = String.Format("SP_MANAGE_PERSON @{0}, @{1}, @{2}, @{3}, @{4}", paramNames);
                SqlParameter[] sqlparams = new SqlParameter[]
                {
                    new SqlParameter() { ParameterName = paramNames[0], Value = person.Id, Direction = ParameterDirection.Input },
                    new SqlParameter() { ParameterName = paramNames[1], Value = person.Firstname, Direction = ParameterDirection.Input },
                    new SqlParameter() { ParameterName = paramNames[2], Value = person.Lastname, Direction = ParameterDirection.Input },
                    new SqlParameter() { ParameterName = paramNames[3], Value = person.DoB.ToString("yyyy-MM-dd"), Direction = ParameterDirection.Input },
                    new SqlParameter() { ParameterName = paramNames[4], Value = person.Weight, Direction = ParameterDirection.Input }
                };

                mess = context.Database.SqlQuery<Message>(procedure, sqlparams).SingleOrDefault();
            }catch(Exception err)
            {
                mess.Description = err.Message;
            }
            return mess;
        }

        [HttpPut("state")]
        public Message State([FromBody] int Id)
        {
            return UpdateState(Id);
        }

        public Message UpdateState(int Id, int Op = 1)
        {
            try
            {
                if(Id <= 0)
                {
                    throw new ApplicationException(badId);
                }

                paramNames = new string[] { "ID", "TARGET", "OP" };
                procedure = String.Format("SP_MANAGE_ENTITY_STATE @{0}, @{1}, @{2}", paramNames);
                SqlParameter[] sqlparams = new SqlParameter[]
                {
                    new SqlParameter() { ParameterName = paramNames[0], Value = Id, Direction = ParameterDirection.Input },
                    //El 1 es el tipo que corresponde a persona
                    new SqlParameter() { ParameterName = paramNames[1], Value = 1, Direction = ParameterDirection.Input },
                    //El 1 corresponde a cambiar estado, el 2 a eliminar
                    new SqlParameter() { ParameterName = paramNames[2], Value = Op, Direction = ParameterDirection.Input }
                };
                mess = context.Database.SqlQuery<Message>(procedure, sqlparams).SingleOrDefault();
            }
            catch (Exception err)
            {
                mess.Description = err.Message;
            }
            return mess;
        }

        // GET: api/<controller>
        [HttpGet("{Id}/{Start}/{End}")]
        public List<Person> List(int Id, int Start, int End)
        {
            try
            {
                paramNames = new string[] { "ID", "START", "END" };
                procedure = String.Format("SP_LIST_PERSON @{0}, @{1}, @{2}", paramNames);
                SqlParameter[] sqlparams = new SqlParameter[]
                {
                    new SqlParameter() { ParameterName = sqlparams[0], Value = Id, Direction = ParameterDirection.Input },
                    new SqlParameter() { ParameterName = sqlparams[1], Value = Start, Direction = ParameterDirection.Input },
                    new SqlParameter() { ParameterName = sqlparams[2], Value = End, Direction = ParameterDirection.Input }
                };
                mess = context.Database.SqlQuery<Message>(procedure, sqlparams).SingleOrDefault();
            }
            catch (Exception err)
            {
                mess.Description = err.Message;
            }
            return mess;
        }        

        // DELETE api/<controller>/5
        [HttpDelete("{Id}")]
        public Message Delete(int Id)
        {
            return UpdateState(Id, 2);
        }
    }
}
