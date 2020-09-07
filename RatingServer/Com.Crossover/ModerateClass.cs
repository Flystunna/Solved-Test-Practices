using Com.Crossover.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Crossover
{
    public class ModerateClass
    {
        private readonly int lastRating;
        private readonly NotificationService notificationService;
        private readonly ExternalRatingApprovalService externalRatingApprovalService;

        public ModerateClass(NotificationService notificationService, ExternalRatingApprovalService externalRatingApprovalService)
        {
            this.notificationService = notificationService;
            this.externalRatingApprovalService = externalRatingApprovalService;
        }


        public String CreateRatingString(int rating, int ratingCeiling)
        {
            StringBuilder ratingStr = new StringBuilder();

            if (rating > ratingCeiling)
            {
                throw new ArgumentException("Cannot be over the hard ceiling");
            }

            if (!externalRatingApprovalService.IsApproved(rating))
            {
                return "NOT-APP";
            }

            if (rating == ratingCeiling)
            {
                ratingStr.Append("TOP+");
            }
            else
            {
                int midCeiling = (int)Math.Floor((ratingCeiling / 2.0));
                if (rating >= midCeiling)
                {
                    ratingStr.Append("HIGH=");
                }
                else if (rating < midCeiling)
                {
                    ratingStr.Append("LOW-");
                }
            }
            ratingStr.Append(rating);
            
            if (rating == lastRating)
            {
                ratingStr.Append("-CACHED");
            }

            this.notificationService.Notify(rating);

            return ratingStr.ToString();
        }
    }
}