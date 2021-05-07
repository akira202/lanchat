using System.ComponentModel;
using System.IO;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     Class representing single transfer request.
    /// </summary>
    public class FileTransferRequest : INotifyPropertyChanged
    {
        private long partsTransferred;
        internal bool Accepted { get; set; }

        /// <summary>
        ///     Path when file will be saved or when is sending from.
        /// </summary>
        public string FilePath { get; internal init; }

        /// <summary>
        ///     File name.
        /// </summary>
        public string FileName => Path.GetFileName(FilePath);

        /// <summary>
        ///     Size of file in parts.
        /// </summary>
        public long Parts { get; internal init; }

        /// <summary>
        ///     Already transferred parts counter.
        /// </summary>
        public long PartsTransferred
        {
            get => partsTransferred;
            internal set
            {
                if (partsTransferred == value)
                {
                    return;
                }

                partsTransferred = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Transfer progress in percent.
        /// </summary>
        public long Progress => 100 * PartsTransferred / Parts;

        /// <summary>
        ///     Raised for <see cref="PartsTransferred" /> update.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}