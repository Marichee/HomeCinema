using HomeCinema.WebModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.WebModels.WebModels
{
  public class UserActionsViewModel
    {
        public int Id { get; set; }
        public ActionsViewModel Action { get; set; }
        public UserViewModel User { get; set; }
        public List<MovieViewModel> Movies { get; set; }
    }
}
