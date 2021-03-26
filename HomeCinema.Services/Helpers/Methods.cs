using HomeCinema.WebModels.Enums;
using HomeCinema.WebModels.WebModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeCinema.Services.Helpers
{
   public class Methods
    {
      public double PearsoneCorrelation(double[] userOneBehavior, double[] userTwoBehavior)
        {
            double averageOne = 0.0;
            double averageTwo = 0.0;
            int count = 0;
            for (var i = 0; i < userOneBehavior.Length; i++)
            {
                if (userOneBehavior[i] != 0 && userTwoBehavior[i] != 0)
                {
                    averageOne += userOneBehavior[i];
                    averageTwo += userTwoBehavior[i];
                    count++;
                }

            }
            averageOne /= count;
            averageTwo /= count;
            double sum = 0.0;
            double squares1 = 0.0;
            double squares2 = 0.0;
            for (var i = 0; i < userOneBehavior.Length; i++)
            {
                if (userOneBehavior[i] != 0 && userTwoBehavior[i] != 0)
                {
                    sum += (userOneBehavior[i] - averageOne) * (userTwoBehavior[i] - averageTwo);
                    squares1 += Math.Pow(userOneBehavior[i] - averageOne, 2);
                    squares2 += Math.Pow(userTwoBehavior[i] - averageTwo, 2);
                }
            }
            return sum / Math.Sqrt(squares1 * squares2);
        }
        public double Rating(List<UserActionsViewModel> actions)
        {
            double downVoteWeight = -0.5;
            double upVoteWeight = 1.0;
            double viewWeight = 3.0;
            double downloadWeight = 0.5;
            double minWeight = 0.1;
            double maxWeight = 5.0;
            int up = actions.Count(x => x.Action == ActionsViewModel.upVote);
            int down = actions.Count(x => x.Action == ActionsViewModel.downVote);
            int view = actions.Count(x => x.Action == ActionsViewModel.view);
            int download = actions.Count(x => x.Action == ActionsViewModel.download);
            double rating = up * upVoteWeight + down * downVoteWeight + view * viewWeight + download * downloadWeight;
            return Math.Min(maxWeight, Math.Max(minWeight, rating));
        }
    }
}
