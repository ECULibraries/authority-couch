using System.Collections.Generic;

namespace AuthorityCouch.Models.Import
{
    public class Meeting
    {
        public List<string> NewAs { get; set; }
        public List<string[]> NewAsRels { get; set; }
        public List<string[]> Data { get; set; }
        public List<string[]> Relations { get; set; }

        public Meeting()
        {
            NewAs = new List<string>(new string[]
            {

"United States--History--Revolutionary period, 1775-1783--Naval operations",
"United States--History--War of 1812--Pictorial works",
"Washington, George, 1732-1799--Travel--North Carolina--Greenville",
"Women authors, American--Mississippi--Jackson--20th century",
"Women authors, American--Virginia--Charlottesville--20th century",
"Women--Political activity--North Carolina--Pitt County",
"World War, 1939-1945--Campaigns--Japan--Pictorial works",
"World War, 1939-1945--Campaigns--Pacific Area--Maps",
"World War, 1939-1945--Campaigns--Pacific Ocean--Photographs",
"World War, 1939-1945--Campaigns--Solomon Islands--Maps",
"World War, 1939-1945--Hospitals--India--History"


            });
            NewAsRels = new List<string[]>
            {
                //rid, sid
               
                new string[2] {"1500", "3924"},
                new string[2] {"1816", "3954"},
                new string[2] {"1585", "3998"},
                new string[2] {"2020", "4000"},
                new string[2] {"1342", "4033"},
                new string[2] {"1821", "4051"},
                new string[2] {"1222", "4081"},
                new string[2] {"1283", "4081"},
                new string[2] {"1456", "4081"},
                new string[2] {"1526", "4081"},
                new string[2] {"1759", "4086"},
                new string[2] {"1827", "4086"},
                new string[2] {"1235", "4092"},
                new string[2] {"1275", "4093"},
                new string[2] {"1541", "4093"},
                new string[2] {"1545", "4093"},
                new string[2] {"1546", "4093"},
                new string[2] {"1610", "4093"},
                new string[2] {"1759", "4099"},
                new string[2] {"1752", "4111"},
                new string[2] {"1229", "4135"},
                new string[2] {"1133", "4179"},
                new string[2] {"1228", "4188"},
                new string[2] {"1792", "4196"},
                new string[2] {"1416", "4227"},
                new string[2] {"1269", "4240"},
                new string[2] {"1821", "4240"},
                new string[2] {"1824", "4253"},
                new string[2] {"1842", "4341"},
                new string[2] {"1763", "4352"},
                new string[2] {"1234", "4362"},
                new string[2] {"1887", "4366"},
                new string[2] {"2017", "4372"},
                new string[2] {"1238", "4381"},
                new string[2] {"2013", "4381"},
                new string[2] {"1449", "4400"},
                new string[2] {"1760", "4414"},
                new string[2] {"1212", "4421"}
            };
            Data = new List<string[]>
            {
                new string[2] {"3bd545c1c309a78af7f2d6875903878f","http://archivesspace.ecu.edu/resources/1834"},
                new string[2] {"3bd545c1c309a78af7f2d68759065795","http://archivesspace.ecu.edu/resources/1460"},
                new string[2] {"3bd545c1c309a78af7f2d68759084402","http://archivesspace.ecu.edu/resources/1186"},
                new string[2] {"3bd545c1c309a78af7f2d68759084402","http://archivesspace.ecu.edu/resources/1270"},
                new string[2] {"3bd545c1c309a78af7f2d68759084402","http://archivesspace.ecu.edu/resources/1726"},
                new string[2] {"3bd545c1c309a78af7f2d68759084402","http://archivesspace.ecu.edu/resources/1833"},
                new string[2] {"3bd545c1c309a78af7f2d687590fd581","http://archivesspace.ecu.edu/resources/1741"},
                new string[2] {"3bd545c1c309a78af7f2d687591a35d1","http://archivesspace.ecu.edu/resources/1557"}



            };
            Relations = new List<string[]>
            {
                new string[3] {"http://archivesspace.ecu.edu/subjects/5942","meeting","http://archivesspace.ecu.edu/resources/528"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5943","meeting","http://archivesspace.ecu.edu/resources/339"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5944","meeting","http://archivesspace.ecu.edu/resources/1373"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5945","meeting","http://archivesspace.ecu.edu/resources/490"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5946","meeting","http://archivesspace.ecu.edu/resources/490"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5947","meeting","http://archivesspace.ecu.edu/resources/490"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5947","meeting","http://archivesspace.ecu.edu/resources/2147"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5948","meeting","http://archivesspace.ecu.edu/resources/490"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5949","meeting","http://archivesspace.ecu.edu/resources/641"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5950","meeting","http://archivesspace.ecu.edu/resources/575"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5951","meeting","http://archivesspace.ecu.edu/resources/441"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5952","meeting","http://archivesspace.ecu.edu/resources/698"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5953","meeting","http://archivesspace.ecu.edu/resources/861"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5954","meeting","http://archivesspace.ecu.edu/resources/437"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5955","meeting","http://archivesspace.ecu.edu/resources/943"},
                new string[3] {"http://archivesspace.ecu.edu/subjects/5956","meeting","http://archivesspace.ecu.edu/resources/1002"}

            };



        }
    }
}