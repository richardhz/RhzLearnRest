using System;
using System.Collections.Generic;
using System.Text;

namespace RhzLearnRest.Domains.Models.Helpers
{
    public static class VendorMediaTypes
    {
        public static string Hateoas { get; } = "application/vnd.saiba.hateoas+json";
        public static string FriendlyWithoutHateoas { get; } = "application/vnd.saiba.friendly+json";
        public static string FriendlyWithHateoas { get; } = "application/vnd.saiba.friendly.hateoas+json";
        public static string FullWithoutHateoas { get; } = "application/vnd.saiba.full+json";
        public static string FullWithHateoas { get; } = "application/vnd.saiba.full.hateoas+json";
        public static bool WithHateoas(string pattern)
        {
            return pattern.Contains("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool Full(string pattern)
        {
            return pattern.Contains("saiba.full", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
