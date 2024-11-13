using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDeskThing.Core.Models
{
    public class Album
    {
        public string Name { get; set; } = string.Empty;
        public List<Artist> Artists { get; set; } = [];
        public string ImageUrl { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
    }
}
