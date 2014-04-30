using System;
using System.Collections.Generic;
using System.Text;

namespace TechdaysDemo.Models
{
    public class ImageList
    {
        public List<string> error { get; set; }
        public string method { get; set; }
        public List<Image> result { get; set; }
        public string status { get; set; }
    }
}
