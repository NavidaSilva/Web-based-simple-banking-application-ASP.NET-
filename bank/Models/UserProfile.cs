using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Xml;

namespace bank.Models
{
    public class UserProfile
    {

        public int userId { get; set; }
        public string? name { get; set; }
        public string?  email { get; set; }
        public string? phone { get; set; }
        public string? password { get; set; }
    }
}
