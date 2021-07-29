using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Linq;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using API.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API
{
    [ApiController]
    [Route("api/[controller]")]
  
    public class UsersController : ControllerBase
    {
          private readonly DataContext _context;
        public UsersController(DataContext context)
        {
          _context=context;
        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<AppUser>>> GetUser(){
            return await  _context.Users.ToListAsync();
           
        }
        [HttpGet("{id}")]

          public  async Task<ActionResult<AppUser>> GetUser(int id){
           return await _context.Users.FindAsync(id);
          
        }
    }
}