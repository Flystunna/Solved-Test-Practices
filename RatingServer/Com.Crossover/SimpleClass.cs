using System;
using System.Text;

namespace Com.Crossover
{
    public class SimpleClass
    {
        public String CreateRatingString(int rating, int ratingCeiling)
        {
            StringBuilder ratingStr = new StringBuilder();

            if (rating > ratingCeiling)
            {
                throw new ArgumentException("Cannot be over the hard ceiling");
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

            return ratingStr.ToString();
        }
    }
}