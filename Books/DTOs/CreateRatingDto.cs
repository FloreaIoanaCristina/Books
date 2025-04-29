using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.DTOs
{
    public class CreateRatingDto
    {
        public string BookTitle { get; set; }
        public int Rate { get; set; }
    }
}