using System;
using System.Linq;

namespace EavSandbox.Web.DataAccess.EntityFramework
{
    public class Include
    {
        public string Value { get; set; }

        public Include Next { get; set; }

        public Include(string value, Include next = null)
        {
            Value = value;
            Next = next;
        }

        public static Include[] Parse(string includes)
        {
            Include ParseSegments(string[] parts, Include include = null)
            {
                include = new Include(parts[0]);

                if (parts.Length > 1)
                    include.Next = ParseSegments(parts.Skip(1).ToArray(), include);

                return include;
            }

            if (string.IsNullOrWhiteSpace(includes))
                return Array.Empty<Include>();

            return includes.Split(',').Select(s => ParseSegments(s.Split('.'))).ToArray();
        }
    }
}
