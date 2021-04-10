using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Lanchat.Core.Models
{
    /// <summary>
    /// </summary>
    public class Announce : INotifyPropertyChanged
    {
        private string nickname;

        /// <summary>
        ///     Guid for ignoring own broadcasts.
        /// </summary>
        [Required]
        public string Guid { get; init; }

        /// <summary>
        ///     Node nickname.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Nickname
        {
            get => nickname;
            set
            {
                if (value == nickname) return;
                nickname = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Node user nickname.
        /// </summary>
        [JsonIgnore]
        public IPAddress IpAddress { get; set; }

        /// <summary>
        ///     Node actively sends broadcasts.
        /// </summary>
        [JsonIgnore]
        public bool Active { get; set; }

        /// <summary>
        ///     Raised for nickname change.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}