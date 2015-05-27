using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Seq.App.EventFieldDigest
{
    public class Digest
    {
        private readonly Regex _ignoreMatcher;
        private readonly Regex _sanitizationMatcher;
        private Dictionary<string, int> _collection;
 
        public Digest(string ignorePattern, string sanitizationPattern)
        {
            if (!string.IsNullOrWhiteSpace(ignorePattern))
            {
                _ignoreMatcher = new Regex(ignorePattern);
            }

            if (!string.IsNullOrWhiteSpace(sanitizationPattern))
            {
                _sanitizationMatcher = new Regex(sanitizationPattern);
            }

            _collection = new Dictionary<string, int>();
        }

        public void Add(string prospect)
        {
            if (_ignoreMatcher != null && _ignoreMatcher.IsMatch(prospect)) return;
            var sanitizedProspect = _sanitizationMatcher != null ? _sanitizationMatcher.Replace(prospect, "") : prospect;
            if (sanitizedProspect.Length == 0) return;

            if (_collection.ContainsKey(sanitizedProspect))
            {
                _collection[sanitizedProspect] = _collection[sanitizedProspect] + 1;
            }
            else
            {
                _collection[sanitizedProspect] = 1;
            }
        }

        public IEnumerable<object> GetDigest()
        {
            return _collection.Select(e => new { Url = e.Key, Count = e.Value }).OrderByDescending(e => e.Count);
        }

        public void Clear()
        {
            _collection = new Dictionary<string, int>();
        }
    }
}
