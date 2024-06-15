using Murmur;
using System.Text;

namespace AlgoliaCrawler.Models
{
    public sealed class PageIndex
    {
        private string _objectID;

        public string Title { get; set; }
        public string Url { get; set; }
        public string ObjectID
        {
            get
            {
                if (_objectID == null)
                {
                    var hasher = MurmurHash.Create32();
                    var hashBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(Url));
                    var hash = BitConverter.ToUInt32(hashBytes, 0);
                    _objectID = hash.ToString("X");
                }
                return _objectID;
            }
        }
    }
}
