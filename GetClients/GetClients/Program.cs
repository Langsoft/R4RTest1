using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetClients.Utilities;

namespace GetClients
{
    class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the file path for client input:");

            List<Client> clientList = new List<Client>();
            string filePath = Console.ReadLine();
            FileInfo theInputFile = null;
            const char _Delimiter = ',';
            int entriesFound = 0;

            try
            {
                theInputFile = new FileInfo(filePath);
                using (var textReader = new StreamReader(theInputFile.FullName))
                {
                    string line = textReader.ReadLine();

                    Console.Write("Processing..");
                    while (line != null)
                    {
                        string[] columns = line.Split(_Delimiter);
                        //process each line
                        if (ProcessInputLine(ref columns))
                        {
                            entriesFound++;

                            // Create new Client and add it to the list 
                            Client c = new Client
                            {
                                ClientID = Convert.ToInt32(columns[0]),
                                FirstName = columns[1],
                                LastName = columns[2],
                                Email = columns[3],
                                Country = columns[4]
                            };

                            clientList.Add(c);
                        }
                        else
                        {
                            Console.WriteLine("Processing..");
                        }
                    
                        line = textReader.ReadLine();
                        Console.Write("..");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(FileNotFoundException))
                {
                    Console.WriteLine("Could not find the file " + (theInputFile != null ?  theInputFile.FullName : "."));
                }
                else
                {
                    Console.WriteLine("Problem encountered, Message is: " + ex.Message);
                }
            }

            if (entriesFound != 0)
            {
                // Attempt to add the list of clients to the database
                Console.WriteLine("Attempting to add " + entriesFound + (entriesFound > 1 ? " records " : " record "));
                Console.WriteLine("to the database:\r\n ");
                foreach (Client c in clientList)
                {
                    using (var db = new R4RTestEntities())
                    {
                        // Check if clent exists in db
                        var result = db.R4RClients.Where(x => x.ClientID == c.ClientID).FirstOrDefault();

                        if (result != null)
                        {
                            Console.WriteLine("A client with the id of " + result.ClientID.ToString().Trim());
                            Console.WriteLine("with the name of " + result.first_name.ToString().Trim() + " " + result.last_name.ToString().Trim());
                            Console.WriteLine("already exists in the database.\r\n");
                        }
                        else
                        {
                            try
                            {
                                // Add to db
                                R4RClients client = new R4RClients
                                 {
                                     ClientID = c.ClientID,
                                     first_name = c.FirstName,
                                     last_name = c.LastName,
                                     email = c.Email,
                                     country = c.Country
                                 };

                                db.R4RClients.Add(client);
                                db.SaveChanges();

                                Console.WriteLine();
                                Console.WriteLine(c.ToString() + ". Added.");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    Console.WriteLine("A problem occured when writing items to the db, Message is" + ex.InnerException.Message);
                                }
                                else
                                {
                                    Console.WriteLine("A problem occured when writing items to the db, Message is" + ex.Message);
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("\r\nHit return to exit.");
            Console.ReadLine();
        }

        /// <summary>
        /// Processes each given input line to apply the following rules:
        /// 
        /// •	Only import people from the United Kingdom	
        /// •	All email addresses should be converted to lower case
        /// •	Do not import people with an invalid email addresses, regardless of country
        /// •	If the first or last name is in lower case, correct it so the first letter is upper case - eg lisa becomes Lisa, paul becomes Paul
        /// </summary>
        /// <param name="columns"></param>
        /// <returns>true if valid input line i.e. complies with rules</returns>
        private static bool ProcessInputLine(ref string[] columns)
        {
            // this should be in configuration
            const string _ValidCountry = "united kingdom";
            bool validInput = true;

            try
            {
                columns[1] = Utils.FirstCharToUpper(columns[1]);
                columns[2] = Utils.FirstCharToUpper(columns[2]);
                columns[3] = Utils.ToLowercase(columns[3]);

                if (!Utils.IsEmail(columns[3]))
                {
                    Console.WriteLine("\r\nID:" + columns[0]
                                            + " "
                                            + columns[1]
                                            + " "
                                            + columns[2]);
                    Console .WriteLine("has an invalid email and will not be entered into the database.");

                    validInput = false;
                }

                if (!(Utils.ToLowercase(columns[4]) == _ValidCountry))
                {
                    Console.WriteLine("\r\nID:" + columns[0]
                                            + " "
                                            + columns[1]
                                            + " "
                                            + columns[2]);
                    Console.WriteLine ("is not in the UK and will not be added to the database.");
                    validInput = false;
                }

                return validInput;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
