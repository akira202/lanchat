using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading.Tasks;
using Lanchat.Core.API;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core
{
    /// <summary>
    ///     Connected user.
    /// </summary>
    public class Node : IDisposable, INotifyPropertyChanged, INodeState
    {
        private readonly IConfig config;

        internal readonly PublicKeyEncryption PublicKeyEncryption;

        /// <see cref="FileReceiver" />
        public readonly FileReceiver FileReceiver;

        /// <see cref="FileSender" />
        public readonly FileSender FileSender;

        internal readonly bool IsSession;

        /// <see cref="Messaging" />
        public readonly Messaging Messaging;

        /// <see cref="INetworkElement" />
        public readonly INetworkElement NetworkElement;

        /// <see cref="NetworkOutput" />
        public readonly NetworkOutput NetworkOutput;

        /// <see cref="Resolver" />
        public readonly Resolver Resolver;

        internal bool HandshakeReceived;
        private string nickname;
        private string previousNickname;
        private Status status;
        internal readonly SymmetricEncryption SymmetricEncryption;

        internal Node(INetworkElement networkElement, IConfig config, bool isSession)
        {
            this.config = config;
            IsSession = isSession;
            NetworkElement = networkElement;
            NetworkOutput = new NetworkOutput(NetworkElement, this);
            PublicKeyEncryption = new PublicKeyEncryption();
            SymmetricEncryption = new SymmetricEncryption(PublicKeyEncryption);
            Messaging = new Messaging(NetworkOutput, SymmetricEncryption);
            FileReceiver = new FileReceiver(NetworkOutput, SymmetricEncryption, config);
            FileSender = new FileSender(NetworkOutput, SymmetricEncryption);

            Resolver = new Resolver(this);
            Resolver.RegisterHandler(new HandshakeHandler(this));
            Resolver.RegisterHandler(new KeyInfoHandler(this));
            Resolver.RegisterHandler(new ConnectionControlHandler(this));
            Resolver.RegisterHandler(new StatusUpdateHandler(this));
            Resolver.RegisterHandler(new NicknameUpdateHandler(this));
            Resolver.RegisterHandler(new MessageHandler(Messaging));
            Resolver.RegisterHandler(new FilePartHandler(FileReceiver));
            Resolver.RegisterHandler(new FileTransferControlHandler(FileReceiver, FileSender));

            NetworkElement.Disconnected += OnDisconnected;
            NetworkElement.DataReceived += Resolver.OnDataReceived;
            NetworkElement.SocketErrored += (s, e) => SocketErrored?.Invoke(s, e);

            if (IsSession) SendHandshake();

            // Check is connection established successful after timeout.
            Task.Delay(5000).ContinueWith(_ =>
            {
                if (!Ready) OnCannotConnect();
            });
        }

        /// <summary>
        ///     Node user nickname.
        /// </summary>
        public string Nickname
        {
            get => $"{nickname}#{ShortId}";
            set
            {
                if (value == nickname) return;
                previousNickname = nickname;
                nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        /// <summary>
        ///     Nickname before last change.
        /// </summary>
        public string PreviousNickname => $"{previousNickname}#{ShortId}";

        /// <summary>
        ///     Short ID.
        /// </summary>
        public string ShortId => Id.GetHashCode().ToString().Substring(1, 4);

        /// <summary>
        ///     Node user status.
        /// </summary>
        public Status Status
        {
            get => status;
            set
            {
                if (value == status) return;
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        /// <summary>
        ///     Dispose node. For safe disconnect use <see cref="Disconnect" /> instead.
        /// </summary>
        public void Dispose()
        {
            NetworkElement.Close();
            PublicKeyEncryption.Dispose();
            FileSender.Dispose();
            FileReceiver.CancelReceive();
            GC.SuppressFinalize(this);
        }
        

        /// <summary>
        ///     ID of TCP client or session.
        /// </summary>
        public Guid Id => NetworkElement.Id;

        /// <summary>
        ///     Node ready. If set to false node won't send or receive messages.
        /// </summary>
        public bool Ready { get; internal set; }

        /// <summary>
        ///     Invoked for properties like nickname or status.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Node successful connected and ready to data exchange.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        ///     Node disconnected. Cannot reconnect.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        ///     TCP session or client returned error.
        /// </summary>
        public event EventHandler<SocketError> SocketErrored;

        internal event EventHandler CannotConnect;

        /// <summary>
        ///     Disconnect from node.
        /// </summary>
        public void Disconnect()
        {
            NetworkOutput.SendPrivilegedData(new ConnectionControl
            {
                Status = ConnectionControlStatus.RemoteClose
            });
            Dispose();
        }

        private void OnDisconnected(object sender, EventArgs _)
        {
            if (Ready)
                Disconnected?.Invoke(this, EventArgs.Empty);
            else
                OnCannotConnect();
        }

        internal void SendHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = config.Nickname,
                Status = config.Status,
                PublicKey = PublicKeyEncryption.ExportKey()
            };

            NetworkOutput.SendPrivilegedData(handshake);
        }

        internal void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void OnCannotConnect()
        {
            CannotConnect?.Invoke(this, EventArgs.Empty);
        }
    }
}