using Com.Crossover.Services;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Com.Crossover
{
    public class HardClass
    {
        private static readonly int HARD_CACHE;
        private static readonly Utils UTILS;

        static HardClass()
        {
            HARD_CACHE = 22;
            UTILS = new Utils();

            try
            {
                Thread.Sleep(4000);
            }
            catch (ThreadInterruptedException ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }

        private readonly NotificationService notificationService;
        private readonly ExternalRatingApprovalService externalRatingApprovalService;

        public HardClass()
        {

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
            if (rating == HARD_CACHE)
            {
                ratingStr.Append("-CACHED");
            }

            ratingStr.Append(UTILS.GetRatingDecoration());

            notificationService.Notify(rating);

            return ratingStr.ToString();
        }
    }
}