using System;
using System.Collections.Generic;
using System.Text;

namespace NPCE_Client.Model
{
    public class Documento
    {
       
        public int Id { get; set; }

        public string FileName { get; set; }

        public long Size { get; set; }

        public string Extension { get; set; }

        public byte[] Content { get; set; }
       
        public string Tag { get; set; }

    }
}
