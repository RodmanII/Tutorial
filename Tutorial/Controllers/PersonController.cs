using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Data;
using Tutorial.Models;
using Tutorial.Utils;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tutorial.Controllers
{
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        Message mess = new Message(1);
        string[] paramNames;
        string procedure = "", badId = "El Id enviado no corresponde a ninguna persona";

        public PersonController(DataContext ctx, IConfiguration conf)
        {
            context = ctx;
            configuration = conf;
        }

        [HttpPost("manager")]
        public Message Save([FromBody][Bind("Id,Firstname,Lastname,DoB,Weight")] Person person)
        {
            try
            {
                paramNames = new string[] { "ID", "FIRSTNAME", "LASTNAME", "DOB", "WEIGHT" };
                procedure = String.Format("dbo.MANAGE_PERSON @{0}, @{1}, @{2}, @{3}, @{4}", paramNames);
                SqlParameter[] sqlparams = new SqlParameter[]
                {
                    new SqlParameter() { ParameterName = paramNames[0], Value = person.Id, SqlDbType = SqlDbType.Int },
                    new SqlParameter() { ParameterName = paramNames[1], Value = person.Firstname, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter() { ParameterName = paramNames[2], Value = person.Lastname, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter() { ParameterName = paramNames[3], Value = person.DoB, SqlDbType = SqlDbType.Date },
                    new SqlParameter() { ParameterName = paramNames[4], Value = person.Weight, SqlDbType = SqlDbType.SmallInt }
                };

                mess = context.Set<Message>().FromSql(procedure, sqlparams).SingleOrDefault();

            }
            catch (Exception err)
            {
                mess.Description = configuration["Messages:ErrGeneric"] + err.Message;
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
                if (Id <= 0)
                {
                    throw new ApplicationException(badId);
                }

                paramNames = new string[] { "ID", "TARGET", "OP" };
                procedure = String.Format("SP_MANAGE_ENTITY_STATE @{0}, @{1}, @{2}", paramNames);
                SqlParameter[] sqlparams = new SqlParameter[]
                {
                    new SqlParameter() { ParameterName = paramNames[0], Value = Id, SqlDbType = SqlDbType.Int },
                    //El 1 es el tipo que corresponde a persona
                    new SqlParameter() { ParameterName = paramNames[1], Value = 1, SqlDbType = SqlDbType.Int },
                    //El 1 corresponde a cambiar estado, el 2 a eliminar
                    new SqlParameter() { ParameterName = paramNames[2], Value = Op, SqlDbType = SqlDbType.Int }
                };
                mess = context.Set<Message>().FromSql(procedure, sqlparams).SingleOrDefault();
            }
            catch (Exception err)
            {
                mess.Description = err.Message;
            }
            return mess;
        }

        // GET: api/<controller>
        [HttpGet("{Id}/{Start}/{End}")]
        public Tuple<Message, List<Person>> List(int Id, int Start, int End)
        {
            List<Person> list = new List<Person>();
            Tuple<Message, List<Person>> result;
            try
            {
                paramNames = new string[] { "ID", "START", "END" };
                procedure = String.Format("SP_LIST_PERSON @{0}, @{1}, @{2}", paramNames);
                SqlParameter[] sqlparams = new SqlParameter[]
                {
                    new SqlParameter() { ParameterName = paramNames[0], Value = Id, SqlDbType = SqlDbType.Int },
                    new SqlParameter() { ParameterName = paramNames[1], Value = Start, SqlDbType = SqlDbType.Int },
                    new SqlParameter() { ParameterName = paramNames[2], Value = End, SqlDbType = SqlDbType.Int }
                };
                list = context.Set<Person>().FromSql(procedure, sqlparams).ToList();
            }
            catch (Exception err)
            {
                mess.Description = err.Message;
            }
            result = new Tuple<Message, List<Person>>(mess, list);

            return result;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{Id}")]
        public Message Delete(int Id)
        {
            return UpdateState(Id, 2);
        }
    }
}
