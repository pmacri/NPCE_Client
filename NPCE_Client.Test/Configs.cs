using System;
using System.Collections.Generic;
using System.Text;

namespace NPCE_Client.Test
{
    public class Configs
    {


        public Configs(Environment env)
        {
            switch (env)
            {
                case Environment.BTS1023:
                    UrlEntryPoint = "http://172.21.21.4/NPCE_EntryPoint/WsCE.svc";
                    PostaEvoConnectionString = "metadata=res://*/PostaEvoModel.csdl|res://*/PostaEvoModel.ssdl|res://*/PostaEvoModel.msl;provider=System.Data.SqlClient;provider connection string='data source=BTS2013\\I1;initial catalog=PostaEvo;user id=sa;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework'";
                    IsLocal = true;
                    break;

                case Environment.Bts2016:
                    UrlEntryPoint = "http://biz2016-dev/NPCE_EntryPoint/WsCE.svc";
                    PostaEvoConnectionString = "metadata=res://*/PostaEvoModel.csdl|res://*/PostaEvoModel.ssdl|res://*/PostaEvoModel.msl;provider=System.Data.SqlClient;provider connection string='data source=BTS2013\\I1;initial catalog=PostaEvo;user id=sa;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework'";
                    IsLocal = true;
                    break;

                case Environment.Certificazione:
                    UrlEntryPoint = "http://10.60.25.228/NPCE_EntryPoint/WsCE.svc";
                    PostaEvoConnectionString = "metadata=res://*/PostaEvoModel.csdl|res://*/PostaEvoModel.ssdl|res://*/PostaEvoModel.msl;provider=System.Data.SqlClient;provider connection string='data source=10.60.26.213\\CNPCESQLINST07;initial catalog=PostaEvo;user id=tmp;password=Qwerty12;MultipleActiveResultSets=True;App=EntityFramework'";
                    PathDocument = @"\\FSCERT4-a127.retecert.postecert\ShareFS\InputDocument\ROL_fcbaad81-509c-4359-9d95-78e6124cb744.doc";
                    HashMD5Document = "AB8EF323B64C85C8DFCCCD4356E4FB9B";
                    PathCov = @"\\FSCERT4-a127.retecert.postecert\ShareFS\inputdocument\7cac0000-ce00-16d8-0000-00000001973a.cov";
                    HashMD5Cov = "5FBA263B3420664720BB6A15F92ED247";
                    InputDocumentSharePath = "\\\\FSCERT4-a127.retecert.postecert\\ShareFS\\inputdocument";
                    break;

                case Environment.Collaudo:
                    UrlEntryPoint = "http://10.60.19.36/NPCE_EntryPoint/WsCE.svc";
                    PathDocument = @"\\FSSVIL-b451.rete.testposte\ShareFS\inputdocument\20201127\80ac0000-3d01-c952-0000-000000019a68-879C931DE4FC8E1A48B284747C2B1C99.docx";
                    HashMD5Document = "879C931DE4FC8E1A48B284747C2B1C99";
                    PostaEvoConnectionString = "metadata=res://*/PostaEvoModel.csdl|res://*/PostaEvoModel.ssdl|res://*/PostaEvoModel.msl;provider=System.Data.SqlClient;provider connection string='data source=10.203.164.176,52770;initial catalog=PostaEvo;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework'";

                    PathCov = @"\\FSSVIL-b451.rete.testposte\ShareFS\inputdocument\20201127\80ac0000-3d01-c952-0000-000000019a68.cov";
                    HashMD5Cov = "5FBA263B3420664720BB6A15F92ED247";

                    InputDocumentSharePath = "\\\\FSSVIL-b451.rete.testposte\\ShareFS\\InputDocument";

                    break;
                case Environment.Staging:
                    UrlEntryPoint = "http://10.60.17.154/NPCE_EntryPoint/WsCE.svc";

                    PathDocument = @"\\FSSVIL-b451.rete.testposte\ShareFS\inputdocument\20201127\80ac0000-3d01-c952-0000-000000019a68-879C931DE4FC8E1A48B284747C2B1C99.docx";
                    HashMD5Document = "879C931DE4FC8E1A48B284747C2B1C99";


                    PathCov = @"\\FSSVIL-b451.rete.testposte\ShareFS\inputdocument\20201127\80ac0000-3d01-c952-0000-000000019a68.cov";
                    HashMD5Cov = "5FBA263B3420664720BB6A15F92ED247";
                    PostaEvoConnectionString = "metadata=res://*/PostaEvoModel.csdl|res://*/PostaEvoModel.ssdl|res://*/PostaEvoModel.msl;provider=System.Data.SqlClient;provider connection string='data source=10.203.164.176,52770;initial catalog=PostaEvo;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework'";
                    //PostaEvoConnectionString = "metadata=res://*/PostaEvoModel.csdl|res://*/PostaEvoModel.ssdl|res://*/PostaEvoModel.msl;provider=System.Data.SqlClient;provider connection string='data source=tpcesqlinst02.rete.testposte\tpcesqlinst02;initial catalog=PostaEvo;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework'";
                    break;

                default:
                    break;
            }
        }

        public string UrlEntryPoint { get; set; }

        public string PostaEvoConnectionString { get; set; }

        public bool IsLocal { get; set; }

        public string PathDocument { get; set; }

        public string HashMD5Document { get; set; }

        public string PathCov { get; set; }

        public string HashMD5Cov { get; set; }

        public string InputDocumentSharePath { get; set; }
    }

}
