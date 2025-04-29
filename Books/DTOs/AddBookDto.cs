using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.DTOs
{
    public class AddBookDto
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Author { get; set; }
        [Required]
        public int GenreId { get; set; }
        [Required]
        public string Description{ get; set; }
        [Required]
        public string AboutAuthor { get; set; }
        

       
        
    }
}