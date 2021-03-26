using HomeCinema.WebModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.WebModels.WebModels
{
   public class MovieViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public int OriginalId { get; set; }
    }
}
