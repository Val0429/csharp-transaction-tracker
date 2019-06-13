using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Constant;
using Constant.Configuration;
using Constant.Utility;

namespace Interface
{
    public class DataManagerRepository
    {
        private static DataManagerRepository _instance;

        public static DataManagerRepository Instance
        {
            get
            {
                return _instance ?? (_instance = new DataManagerRepository());
            }
        }

        private DataManagerRepository()
        {
        }


        private readonly Dictionary<string, IDataManager> _repository = new Dictionary<string, IDataManager>();

        public IDataManager this[string name]
        {
            get { return _repository.ContainsKey(name) ? _repository[name] : default(IDataManager); }
        }

        public T Get<T>(string name) where T : IDataManager
        {
            Debug.Assert(_repository.ContainsKey(name) && _repository[name] is T);
            return _repository.ContainsKey(name) ? (T)_repository[name] : default(T);
        }

        public void Initialize(IServer server, ServerCredential sisCredential)
        {
            Server = server;

            Credential = sisCredential;

            var repository = ConfigurationManager.GetSection("activatorRepository") as ActivatorRepostiory;
            if (repository == null)
            {
                return;
            }

            foreach (ActivatorSection activator in repository.Activators)
            {
                if (!_repository.ContainsKey(activator.Name))
                {
                    var dataManager = ActivatorUtility.Create<IDataManager>(activator, Server, Credential);

                    _repository[activator.Name] = dataManager;
                }

                _repository[activator.Name].Initialize();
            }
        }

        public void Load()
        {
            foreach (var dataManager in _repository.Values)
            {
                dataManager.Load();
            }

            OnLoadCompleted(EventArgs.Empty);
        }

        public void Save()
        {
            foreach (var manager in _repository.Values)
            {
                try
                {
                    manager.Save();
                }
                catch (System.Exception ex)
                {
                    Logger.Current.Error(ex);
                }
            }

            OnSaveCompleted(EventArgs.Empty);
        }


        public IServer Server { get; private set; }

        /// <summary>
        /// SIS Credential
        /// </summary>
        public ServerCredential Credential { get; private set; }

        public int Count { get { return _repository.Count; } }

        public IEnumerable<IDataManager> Managers { get { return _repository.Values; } }


        public event EventHandler LoadCompleted;

        private void OnLoadCompleted(EventArgs e)
        {
            var handler = LoadCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler SaveCompleted;

        private void OnSaveCompleted(EventArgs e)
        {
            var handler = SaveCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    class Logger
    {
        private static ILogger _current;
        public static ILogger Current { get { return _current ?? (_current = LoggerManager.Instance.GetLogger()); } }

        private Logger()
        {
        }
    }
}