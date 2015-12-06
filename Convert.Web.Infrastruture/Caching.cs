using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convert.Infrastruture.Caching;
using Convert.Infrastruture.Logging;
using System.Runtime.Caching;
using System.Threading;

namespace Convert.Web.Infrastruture
{
    public class Caching : ICache
    {
        private readonly ILogger _logger;
        private MemoryCache _cache;

        public Caching(ILogger logger)
        {
            _logger = logger;
        }

        private MemoryCache Cache
        {
            get
            {
                if (_cache == null) 
                {
                    _cache = new MemoryCache("Convert");
                }
                return _cache;
            }
        }

        public ILogger Logger
        {
            get { return _logger; }
        }

        public object GetObject(string key)
        {
            try
            {
                object cachedItem = Cache[key];
                return cachedItem;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return null;
        }

        public object AddObject(string key, DateTime expireDate, object item)
        {
            try
            {
                object itemInCache = Cache.AddOrGetExisting(key, item, expireDate);
                if (itemInCache == null)
                {
                    return item;
                }
                return itemInCache;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

            return item;
        }


        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

        ~Caching()
        {
            if (_cacheLock != null)
            {
                _cacheLock.Dispose();
            }
        }

        private void LogException(Exception ex)
        {
            Logger.LogException(ex);
        }

    }

}
