using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NimapTask.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        
        public ICollection<Product> Products { get; set; }
    }
}