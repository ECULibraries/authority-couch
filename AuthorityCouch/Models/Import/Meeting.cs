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
                "Diary of a public man",
                "Mecklenburg Declaration of Independence",
                "Camp Dodger",
                "Crisp, Lucy Cherry, 1899-1977. Brief testament : verse",
                "Crisp, Lucy Cherry, 1899-1977. History of the North Carolina State Art Society",
                "Crisp, Lucy Cherry, 1899-1977. Spring fever and other dialect verse",
                "Crisp, Lucy Cherry, 1899-1977. Story of Nancy North",
                "Daily Reflector (Greenville, N.C.)",
                "Jefferies, Susan Herring, 1902-1980. Papa wore no halo",
                "Land of Plenty",
                "North Carolina farmer (Raleigh, N.C. : 1876)",
                "Pitch a boogie woogie (Motion picture)",
                "Rebel",
                "Semi-weekly Raleigh register",
                "Shipmate (Annapolis, Md.)"
            });
            NewAsRels = new List<string[]>
            {
                //rid, sid
                new string[2] {"528", "5942"},
                new string[2] {"339", "5943"},
                new string[2] {"1373", "5944"},
                new string[2] {"490", "5945"},
                new string[2] {"490", "5946"},
                new string[2] {"490", "5947"},
                new string[2] {"2147", "5947"},
                new string[2] {"490", "5948"},
                new string[2] {"641", "5949"},
                new string[2] {"575", "5950"},
                new string[2] {"441", "5951"},
                new string[2] {"698", "5952"},
                new string[2] {"861", "5953"},
                new string[2] {"437", "5954"},
                new string[2] {"943", "5955"},
                new string[2] {"1002", "5956"}
            };
            Data = new List<string[]>
            {
                new string[2] {"Diary of a public man", "http://archivesspace.ecu.edu/subjects/5942"},
                new string[2] {"Mecklenburg Declaration of Independence", "http://archivesspace.ecu.edu/subjects/5943"},
                new string[2] {"Camp Dodger", "http://archivesspace.ecu.edu/subjects/5944"},
                new string[2] {"Crisp, Lucy Cherry, 1899-1977. Brief testament : verse", "http://archivesspace.ecu.edu/subjects/5945"},
                new string[2] {"Crisp, Lucy Cherry, 1899-1977. History of the North Carolina State Art Society", "http://archivesspace.ecu.edu/subjects/5946"},
                new string[2] {"Crisp, Lucy Cherry, 1899-1977. Spring fever and other dialect verse", "http://archivesspace.ecu.edu/subjects/5947"},
                new string[2] {"Crisp, Lucy Cherry, 1899-1977. Story of Nancy North", "http://archivesspace.ecu.edu/subjects/5948"},
                new string[2] {"Daily Reflector (Greenville, N.C.)", "http://archivesspace.ecu.edu/subjects/5949"},
                new string[2] {"Jefferies, Susan Herring, 1902-1980. Papa wore no halo", "http://archivesspace.ecu.edu/subjects/5950"},
                new string[2] {"Land of Plenty", "http://archivesspace.ecu.edu/subjects/5951"},
                new string[2] {"North Carolina farmer (Raleigh, N.C. : 1876)", "http://archivesspace.ecu.edu/subjects/5952"},
                new string[2] {"Pitch a boogie woogie (Motion picture)", "http://archivesspace.ecu.edu/subjects/5953"},
                new string[2] {"Rebel", "http://archivesspace.ecu.edu/subjects/5954"},
                new string[2] {"Semi-weekly Raleigh register", "http://archivesspace.ecu.edu/subjects/5955"},
                new string[2] {"Shipmate (Annapolis, Md.)", "http://archivesspace.ecu.edu/subjects/5956"}
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