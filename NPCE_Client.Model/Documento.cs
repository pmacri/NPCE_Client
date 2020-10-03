using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NPCE_Client.Model
{
    public class Documento
    {
        public Documento()
        {
            Servizi = new Collection<Servizio>();
        }
       
        public int Id { get; set; }

        public string FileName { get; set; }

        public long Size { get; set; }

        public string Extension { get; set; }

        public byte[] Content { get; set; }
       
        public string Tag { get; set; }

        public virtual ICollection<Servizio> Servizi { get; set; }

    }
}
