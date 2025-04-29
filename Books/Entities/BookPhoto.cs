using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Entities
{
    [Table("BPhotos")]
    public class BookPhoto
    {
        public int Id { get; set; }
        public string UrlBook { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public int AppBooksId { get; set; }
        public AppBooks AppBooks { get; set; }
    }
}