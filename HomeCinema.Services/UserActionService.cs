using AutoMapper;
using HomeCinema.DataAccess.Interfaces;
using HomeCinema.Domain;
using HomeCinema.Domain.Models;
using HomeCinema.Services.Interfaces;
using HomeCinema.WebModels.WebModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeCinema.Services
{
    public class UserActionService : IUserActionService
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<UserActions> _userActionRepository;
        private readonly IRepository<UserActionMovies> _actionMovieRepository;
        private readonly IUserRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public UserActionService(IRepository<UserActions> userActionRepository, IRepository<UserActionMovies> actionMovieRepository, IUserRepository<User> userRepository, IMapper mapper, IRepository<Movie> movieRepository)
        {
            _actionMovieRepository = actionMovieRepository;
            _mapper = mapper;
            _userActionRepository = userActionRepository;
            _movieRepository = movieRepository;
        }
        public void CreateAction(UserActionsViewModel model)
        {
            _userActionRepository.Insert(_mapper.Map<UserActions>(model));
        }
        public void UpdateMovieToAction(int movieId, WebModels.Enums.ActionsViewModel newAction, string userId, int useractionId)
        {
            //var oldActionModel = new UserActionsViewModel
            //{
            //    Action = currentAction
            //};
            //var oldAction = _mapper.Map<UserActions>(oldActionModel);
            //var userAction = _userActionRepository.GetAll().FirstOrDefault(x => x.Action == oldAction.Action && x.UserId == userId);
            var userAction = _userActionRepository.GetAll().FirstOrDefault(x => x.Id == useractionId);
            var newActionModel = new UserActionsViewModel
            {
                Action = newAction

            };
            var action = _mapper.Map<UserActions>(newActionModel);
            userAction.Action = action.Action;
            //var updateAction = new UserActionMovies
            //{
            //    MovieId = movieId,
            //    UserActionId = useractionId,
            //    Movie = _movieRepository.GetById(movieId),
            //    UserActions = userAction

            //};
            _userActionRepository.Update(userAction);
        }


        public void AddMovieToAction(int actionId, int movieId, string userId)
        {
            _actionMovieRepository.Insert(new UserActionMovies
            {
                MovieId = movieId,
                UserActionId = actionId
            });
        }
        public UserActionsViewModel GetActionById(int id)
        {
            UserActions userActions = _userActionRepository.GetById(id);
            if (userActions == null) throw new Exception("User action does not exists!");
            return _mapper.Map<UserActionsViewModel>(userActions);
        }

        public IEnumerable<UserActionsViewModel> GetAllActions()
        {
            return _mapper.Map<IEnumerable<UserActionsViewModel>>(_userActionRepository.GetAll());
        }

        public IEnumerable<UserActionsViewModel> GetUserActions(string userId)
        {
            return _mapper.Map<IEnumerable<UserActionsViewModel>>(_userActionRepository.GetAll().Where(a => a.UserId == userId));
        }
        public UserActionsViewModel GetCurrentAction(string userId)
        {
            return _mapper.Map<UserActionsViewModel>(_userActionRepository.GetAll().LastOrDefault(u => u.UserId == userId));
        }
        public IEnumerable<UserActionsViewModel> GetActionForMovie(int movieId, string userId)
        {
            var userActionsList = new List<UserActionsViewModel>();
            var actionMovie = _actionMovieRepository.GetAll().Where(x => x.MovieId == movieId);
            foreach (var userAction in _userActionRepository.GetAll())
            {
                if (userAction.UserId == userId)
                {
                    foreach (var movieAction in actionMovie)
                    {

                        if (movieAction.UserActionId == userAction.Id)
                            userActionsList.Add(_mapper.Map<UserActionsViewModel>(userAction));
                    }
                }
            }
            return userActionsList;
        }
        public void CheckActionIfExist(int movieId, string userId)
        {
            var actionMovie = GetActionForMovie(movieId, userId);
            UserActionsViewModel userActions = GetCurrentAction(userId);
            var upVotes = 0;
            var downVotes = 0;
            var downloads = 0;
            var views = 0;
            foreach (var action in actionMovie)
            {
                if (action.Action == WebModels.Enums.ActionsViewModel.view)
                {
                    views++;
                }
                else if (action.Action == WebModels.Enums.ActionsViewModel.upVote)
                {
                    upVotes++;
                }
                else if (action.Action == WebModels.Enums.ActionsViewModel.downVote)
                {
                    downVotes++;
                }
                else
                {
                    downloads++;
                }


            }

            if (upVotes == 0 && userActions.Action == WebModels.Enums.ActionsViewModel.upVote && downVotes == 0)
            {
                AddMovieToAction(userActions.Id, movieId, userId);
            }
            else if (downVotes == 0 && userActions.Action == WebModels.Enums.ActionsViewModel.downVote && upVotes == 0)
            {
                AddMovieToAction(userActions.Id, movieId, userId);
            }
            else if (downloads == 0 && userActions.Action == WebModels.Enums.ActionsViewModel.download)
            {
                AddMovieToAction(userActions.Id, movieId, userId);
            }
            else if (views == 0 && userActions.Action == WebModels.Enums.ActionsViewModel.view)
            {
                AddMovieToAction(userActions.Id, movieId, userId);
            }

            if (upVotes >= 1 && userActions.Action == WebModels.Enums.ActionsViewModel.downVote)
            {
                foreach (var action in actionMovie)
                {
                    if (action.Action == WebModels.Enums.ActionsViewModel.upVote)
                    {


                        UpdateMovieToAction(movieId, WebModels.Enums.ActionsViewModel.downVote, userId, action.Id);
                    }
                }
            }
            if (downVotes >= 1 && userActions.Action == WebModels.Enums.ActionsViewModel.upVote)
            {
                foreach (var action in actionMovie)
                {
                    if (action.Action == WebModels.Enums.ActionsViewModel.downVote)
                    {
                        UpdateMovieToAction(movieId, WebModels.Enums.ActionsViewModel.upVote, userId, action.Id);

                    }
                }
            }

        }
    }
}
