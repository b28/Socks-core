using CustomServer.Ui.DataContainers;
using CustomServer.Utils.MtCollection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomServer.Ui
{
    public sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {

        }

        public void AddToLog(string logMsg)
        {
            innerLogMessages.Add(new LogMessage { Message = logMsg });
        }

        public void AddToClientList(ConnectedClientInfoRecord infoRecord)
        {
            innerClientList.Add(infoRecord);
        }

        public void RemoveFromClientList(ConnectedClientInfoRecord clientInfoRecord)
        {
            if (innerClientList.Contains(clientInfoRecord))
                innerClientList.Remove(clientInfoRecord);

        }


        public void Message() { }

        private int listenPort;

        public int ListenPort
        {
            get { return listenPort; }
            set
            {
                if (value == listenPort) return;
                listenPort = value;
                OnPropertyChanged(nameof(ListenPort));
            }
        }

        private string listenIp;

        public string ListenIp
        {
            get { return listenIp; }
            set
            {
                if (value == listenIp) return;
                listenIp = value;
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(ListenIp));

            }
        }

        private CollectionMtWithAsyncObservableCollectionReadOnlyCopy<LogMessage> innerLogMessages { get; } =
            new CollectionMtWithAsyncObservableCollectionReadOnlyCopy<LogMessage>();



        public ReadOnlyObservableCollection<LogMessage> LogMessages => new ReadOnlyObservableCollection<LogMessage>(innerLogMessages.ObsColl);

        //public ObservableCollection<LogMessage> LogMessages => innerLogMessages.ObsColl;
        public ObservableCollection<ConnectedClientInfoRecord> ClientsList => innerClientList.ObsColl;

        private CollectionMtWithAsyncObservableCollectionReadOnlyCopy<ConnectedClientInfoRecord> innerClientList =
            new CollectionMtWithAsyncObservableCollectionReadOnlyCopy<ConnectedClientInfoRecord>();





        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class LogMessage
    {
        public LogMessage()
        {
            TimeStamp = DateTime.Now.ToString("");
        }
        public string TimeStamp { get; set; }
        public string Message { get; set; }
    }
}