using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLabelEditor
{
    internal class RTStudy
    {
        public string path { get; set; }
        public string patientID { get; set; }
        public string studyDate { get; set; }
        public string patientName { get; set; }
        public List<string> labels { get; set; }
        public List<string> ct { get; set; }
    }

}
