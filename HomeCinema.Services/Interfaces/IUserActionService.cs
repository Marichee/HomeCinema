using HomeCinema.WebModels.WebModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.Services.Interfaces
{
  public interface IUserActionService
    {
        IEnumerable<UserActionsViewModel> GetAllActions();
        UserActionsViewModel GetActionById(int id);
        UserActionsViewModel GetCurrentAction(string userId);
        IEnumerable<UserActionsViewModel> GetActionForMovie(int movieId,string userId);
        IEnumerable<UserActionsViewModel> GetUserActions(string userId);
        void CreateAction(UserActionsViewModel model);
        void AddMovieToAction(int actionId, int movieId,string userId);
        void CheckActionIfExist(int movieId, string userId);
        void UpdateMovieToAction(int movieId, WebModels.Enums.ActionsViewModel newAction, string userId,int useractionId);
    }
}
