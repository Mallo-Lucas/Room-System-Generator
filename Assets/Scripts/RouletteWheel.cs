using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public class RouletteWheel<T>
    {
        public RouletteWheel()
        {
        }

        public RouletteWheel(Dictionary<T, float> items)
        {
            SetCachedDictionary(items);
        }

        public RouletteWheel(List<T> items, List<float> chances)
        {
            SetCachedDictionaryFromLists(items, chances);
        }

        private Dictionary<T, float> _cachedDictionary = new();

        private float _cachedSum;


        public static Ty Run<Ty>(Dictionary<Ty, float> items)
        {
            float max = 0;

            foreach (var item in items)
            {
                max += item.Value;
            }

            var random = UnityEngine.Random.Range(0, max);

            foreach (var item in items)
            {
                random -= item.Value;
                if (random <= 0)
                {
                    return item.Key;
                }
            }

            return default;
        }

        public void SetCachedDictionary(Dictionary<T, float> itemsToCache)
        {
            _cachedDictionary = itemsToCache;

            _cachedSum = 0;
            foreach (var item in _cachedDictionary)
            {
                _cachedSum += item.Value;
            }
        }

        public void SetCachedDictionaryFromLists(List<T> items, List<float> chances)
        {
            _cachedDictionary = new Dictionary<T, float>();
            _cachedSum = 0;

            for (int i = 0; i < items.Count; i++)
            {
                var chance = chances[i];
                _cachedDictionary.Add(items[i], chance);
                _cachedSum += chance;
            }
        }

        public T RunWithCached()
        {
            if (_cachedDictionary == null)
                return default;

            var random = UnityEngine.Random.Range(0, _cachedSum);

            foreach (var item in _cachedDictionary)
            {
                random -= item.Value;
                if (random <= 0)
                {
                    return item.Key;
                }
            }

            return default;
        }

        public T RunWithCachedFilter(List<T> filter)
        {
            if (_cachedDictionary == null)
                return default;

            var random = UnityEngine.Random.Range(0, _cachedSum);
            
            foreach (var item in _cachedDictionary.Where(x=>!filter.Contains(x.Key)))
            {
                random -= item.Value;
                if (random <= 0)
                {
                    return item.Key;
                }
            }

            return default;
        }
    }
}