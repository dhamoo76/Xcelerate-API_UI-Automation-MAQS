using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UI.Data
{
    public class CDSStructureItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string Type { get; set; }
        public string ID { get; set; }
        public List<string> Tags { get; set; }
        public List<Fields> Fields { get; set; }

        public CDSStructureItem() 
        {
            Tags = new List<string>();
            Fields = new List<Fields>();
        }
    }

    public class Fields
    {
        public string Label;
        public string Description;
    }
}
