using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TechJobsConsole
{
    class JobData
    {
        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();
        static List<Dictionary<string, string>> copyOfAllJobs = new List<Dictionary<string, string>>();
        static bool IsDataLoaded = false;

// ---------------------------------------------------------------------------------------------------------------------------------------------------
 /*Bonus Mission (Part-2): this method makes a depp copy of AllJobs; it is used withing LoadData method (Line 134) to generate a copy of AllJobs*/
        public static List<Dictionary<string, string>> CopyAll(List<Dictionary<string, string>> list)
        {
            List<Dictionary<string, string>> copy = new List<Dictionary<string, string>>();

            foreach(Dictionary<string, string> item in list)
            {
                copy.Add(item);
            }
            return copy;
        }
 /*Bonus Mission (Part-2): this method is used to test that modifying the copyAllJobs does not alter Alljobs. it used at Line 133 in the Main; 
  after searching for a term, we can see that the number of elements in Alljobs reamins fixed while the one in copyAllJobs is changing.*/
        /*public static void printing()
        {
            Console.WriteLine(AllJobs.Count);
            Console.WriteLine(copyOfAllJobs.Count);
            copyOfAllJobs.RemoveAt(2);
        }*/
//-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static List<Dictionary<string, string>> FindAll()
        {
            LoadData();
            return copyOfAllJobs;
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        public static List<string> FindAll(string column)
        {
            LoadData();

            List<string> values = new List<string>();

            foreach (Dictionary<string, string> job in copyOfAllJobs)
            {
                string aValue = job[column];

                if (!values.Contains(aValue))
                {
                    values.Add(aValue);
                }
            } 
//---------------------------------------------------------------------------------------------------------------------------------------------------------
// Bonus Mission (Part-1): the list results are sorted here.
            values.Sort();
//---------------------------------------------------------------------------------------------------------------------------------------------------------

            return values;
        }

        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> row in copyOfAllJobs)
            {
 //---------------------------------------------------------------------------------------------------------------------------------------------------------
 // The search is made case insensitive here.
                string aValue = row[column].ToLower();

                if (aValue.Contains(value.ToLower()))
 //---------------------------------------------------------------------------------------------------------------------------------------------------------              
                {
                    jobs.Add(row);
                }
            }

            return jobs;
        }

        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
        {

            if (IsDataLoaded)
            {
                return;
            }

            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] rowArrray = CSVRowToStringArray(line);
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            string[] headers = rows[0];
            rows.Remove(headers);

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                AllJobs.Add(rowDict);
            }

//--------------------------------------------------------------------------------------------------------------------------------------------
//Bonus Mission (Part-2): this line generates a copy of AllJobs
            copyOfAllJobs = CopyAll(AllJobs);
//--------------------------------------------------------------------------------------------------------------------------------------------

            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }


//-------------------------------------------------------------------------------------------------------------------------------------------
// The method FindByValue is created here.
        public static List<Dictionary<string, string>> FindByValue(string value)
        {
            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            List<string> columnsKeys = FindAll()[0].Keys.ToList();
            for(int i=0; i< columnsKeys.Count; i++)
            {
                List<Dictionary<string, string>> listOfEntries = FindByColumnAndValue(columnsKeys[i], value);
                foreach ( Dictionary<string,string> entry in listOfEntries)
                {
                    if (!jobs.Contains(entry))
                    {
                        jobs.Add(entry);
                    }
                }
            }

            return jobs;
        }
//--------------------------------------------------------------------------------------------------------------------------------------------
    }
}
