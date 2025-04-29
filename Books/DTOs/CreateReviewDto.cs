using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.DTOs
{
    public class CreateReviewDto
    {
        public string BookTitle { get; set; }
        public string Content { get; set; }
        [Range(1,5)]
        public int Rate { get; set; }
    }
}