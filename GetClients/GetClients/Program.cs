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
        
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the file path for client input:");

            List<Client> clientList = new List<Client>();
            string filePath = Console.ReadLine();
            FileInfo theInputFile = new FileInfo(filePath);
            const char _Delimiter = ',';
            int entriesFound = 0;

            try
            {
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
                    Console.WriteLine("Could not find the file " + theInputFile.FullName);
                }
                else
                {
                    Console.WriteLine("Problem encountered, Message is: " + ex.Message);
                }
            }

            Console.WriteLine("The following " + entriesFound + (entriesFound > 1  ? " records " : " record ")); 
            Console.WriteLine("will be added to the database:\r\n ");
            foreach (Client c in clientList)
            {
                Console.WriteLine(c.ToString());
            }

            Console.WriteLine("\r\nHit return to exit.");
            Console.ReadLine();
            
        }

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
                    Console.WriteLine("ID:" + columns[0]
                                            + " "
                                            + columns[1]
                                            + " "
                                            + columns[2]);
                    Console .WriteLine(" has an invalid email and will not be entered into the database.");

                    validInput = false;
                }

                if (!(Utils.ToLowercase(columns[4]) == _ValidCountry))
                {
                    Console.WriteLine("ID:" + columns[0]
                                            + " "
                                            + columns[1]
                                            + " "
                                            + columns[2]);
                    Console.WriteLine (" is not in the UK and will not be added to the database.");
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
