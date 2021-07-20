using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestBankAPI.Models.Data
{
    public class BaseModel : IEntity
    {
        public int Id { get; set; }
    }
}
