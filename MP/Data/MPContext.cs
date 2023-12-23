using Microsoft.EntityFrameworkCore;
using MP.Models;
using System.Collections.Generic;

namespace MP.Data
{
    public class MPContext// : DbContext
    {
        public List<Order> Orders { get; set; }

        public MPContext()//DbContextOptions<MPContext> options) : base(options)
        {
            Orders = new List<Order>();
        }
    }
}
