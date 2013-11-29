using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace LyricsLibNet
{
    public class LyricsFinder
    {
        private static readonly Type[] EmptyTypes = new Type[0];

        private readonly List<ILyricsProvider> _providers;
        private readonly List<LyricsResult> _results;
        private int _requestsPending;
        private bool _started;

        public LyricsFinder(string artist, string trackTitle)
        {
            _results = new List<LyricsResult>();
            _providers = new List<ILyricsProvider>();
            AddProvidersForAssembly(typeof (LyricsFinder).Assembly);

            Artist = artist;
            TrackTitle = trackTitle;
        }

        public string Artist { get; private set; }
        public string TrackTitle { get; private set; }

        public LyricsResult[] Results
        {
            get
            {
                lock (_results)
                {
                    return _results.ToArray();
                }
            }
        }

        public void ClearProviders()
        {
            if (_started)
            {
                throw new InvalidOperationException("Already started.");
            }

            _providers.Clear();
        }

        public void AddProvidersForAssembly(Assembly assembly)
        {
            Type targetInterface = typeof (ILyricsProvider);
            foreach (Type t in assembly.GetTypes().Where(targetInterface.IsAssignableFrom))
            {
                if (t.IsInterface) continue;
                if (t.IsAbstract) continue;
                ConstructorInfo constructor = t.GetConstructor(EmptyTypes);
                var provider = (ILyricsProvider) constructor.Invoke(null);
                AddProvider(provider);
            }
        }

        public void AddProvider(ILyricsProvider provider)
        {
            if (_started)
            {
                throw new InvalidOperationException("Already started.");
            }

            _providers.Add(provider);
        }

        public void Start()
        {
            if (_started)
            {
                throw new InvalidOperationException("Already started.");
            }

            if (_providers.Count == 0)
            {
                throw new InvalidOperationException("No providers registered.");
            }

            _started = true;

            _requestsPending = _providers.Count;

            foreach (ILyricsProvider provider in _providers)
            {
                provider.Query(Artist, TrackTitle, result =>
                    {
                        bool isLast = Interlocked.Decrement(ref _requestsPending) == 0;
                        OnResultAvailable(isLast, result);
                        if (isLast)
                        {
                            OnQueryCompleted();
                        }
                    }, exception =>
                        {
                            bool isLast = Interlocked.Decrement(ref _requestsPending) == 0;
                            OnProviderError(isLast, provider.Name, exception);
                            if (isLast)
                            {
                                OnQueryCompleted();
                            }
                        });
            }
        }

        public event EventHandler<ResultAvailableEventArgs> ResultAvailable;

        private void OnResultAvailable(bool isLast, LyricsResult lyricsResult)
        {
            lock (_results)
            {
                _results.Add(lyricsResult);
            }

            if (ResultAvailable != null)
            {
                ResultAvailable(this, new ResultAvailableEventArgs(isLast, lyricsResult));
            }
        }

        public event EventHandler QueryCompleted;

        private void OnQueryCompleted()
        {
            if (QueryCompleted != null)
            {
                QueryCompleted(this, EventArgs.Empty);
            }
        }

        public event EventHandler<ProviderErrorEventArgs> ProviderError;

        private void OnProviderError(bool isLast, string providerName, Exception exception)
        {
            if (ProviderError != null)
            {
                ProviderError(this, new ProviderErrorEventArgs(isLast, providerName, exception));
            }
        }
    }
}