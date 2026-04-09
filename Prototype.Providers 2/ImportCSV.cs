using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.IO;

namespace Prototype.Providers
{
    public class ImportCSV : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ImportCSV()
        {
            EncodingFile = Encoding.Default;
            HasColumn = false;
        }

        public Encoding EncodingFile { get; set; }
        public bool HasColumn { get; set; }

        public DataTable GetCSVData(string CSVPath)
        {
            DataTable dtResult = new DataTable("dtCSV");

            try
            {
                ArrayList alResult;
                string strLineText;
                bool CheckHaveColumn = false;

                // Create an instance of StreamReader to read from a file.
                using (StreamReader objReader = new StreamReader(CSVPath, EncodingFile))
                {
                    while ((strLineText = objReader.ReadLine()) != null)
                    {
                        alResult = CSVParser(strLineText);

                        if (HasColumn && !CheckHaveColumn)
                        {
                            SetColumn(ref dtResult, alResult);
                            CheckHaveColumn = true;
                        }
                        else
                        {
                            AddRecord(ref dtResult, alResult);
                        }
                    }
                }

                return dtResult;
            }
            catch
            {
                // Let the user know what went wrong.
                throw new Exception("The file could not be read. It should be a csv file with only four columns.");
            }

        }

        private void SetColumn(ref DataTable dtResult, ArrayList alResult)
        {
            foreach (string colField in alResult)
            {
                dtResult.Columns.Add(colField.Trim());
            }

        }
        private void AddRecord(ref DataTable dtResult, ArrayList alResult)
        {
            DataRow drRow = dtResult.NewRow();
            int maxCol = dtResult.Columns.Count;

            if (alResult.Count == maxCol)
            {
                for (int i = 0; i < maxCol; i++)
                {
                    drRow[i] = alResult[i];
                }
            }
            else
            {
                for (int i = 0; i < maxCol; i++)
                {
                    drRow[i] = "error";
                }
            }

            dtResult.Rows.Add(drRow);
        }

        //=== Modify by Bank 30/12/2014 ===
        //===================================
        //private ArrayList CSVParser(string strInputString)
        //{
        //    string[] str = strInputString.Split('\t');
        //    ArrayList arr = new ArrayList();
        //    foreach (var item in str)
        //    {
        //        arr.Add(item);
        //    }
        //    return arr;
        //}

        private ArrayList CSVParser(string strInputString)
        {
            int intCounter = 0, intLenght;
            StringBuilder strElem = new StringBuilder();
            ArrayList alParsedCsv = new ArrayList();
            intLenght = strInputString.Length;
            strElem = strElem.Append("");
            int intCurrState = 0;
            int[][] aActionDecider = new int[9][];

            //Build the state array
            aActionDecider[0] = new int[4] { 2, 0, 1, 5 };
            aActionDecider[1] = new int[4] { 6, 0, 1, 5 };
            aActionDecider[2] = new int[4] { 4, 3, 3, 6 };
            aActionDecider[3] = new int[4] { 4, 3, 3, 6 };
            aActionDecider[4] = new int[4] { 2, 8, 6, 7 };
            aActionDecider[5] = new int[4] { 5, 5, 5, 5 };
            aActionDecider[6] = new int[4] { 6, 6, 6, 6 };
            aActionDecider[7] = new int[4] { 5, 5, 5, 5 };
            aActionDecider[8] = new int[4] { 0, 0, 0, 0 };

            for (intCounter = 0; intCounter < intLenght; intCounter++)
            {
                intCurrState = aActionDecider[intCurrState][GetInputID(strInputString[intCounter])];
                //take the necessary action depending upon the state returned
                PerformAction(ref intCurrState, strInputString[intCounter], ref strElem, ref alParsedCsv);
            }
            intCurrState = aActionDecider[intCurrState][3];
            PerformAction(ref intCurrState, '\0', ref strElem, ref alParsedCsv);
            return alParsedCsv;
        }
        private int GetInputID(char chrInput)
        {
            if (chrInput == '"')
            {
                return 0;
            }
            else if (chrInput == ',')
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        private void PerformAction(ref int intCurrState, char chrInputChar, ref StringBuilder strElem, ref ArrayList alParsedCsv)
        {
            string strTemp = null;
            switch (intCurrState)
            {
                case 0:
                    //Seperate out value to array list
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(strTemp);
                    strElem = new StringBuilder();
                    break;
                case 1:
                case 3:
                case 4:
                    //accumulate the character
                    strElem.Append(chrInputChar);
                    break;
                case 5:
                    //End of line reached. Seperate out value to array list
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(strTemp);
                    break;
                case 6:
                    //Erroneous input. Reject line.
                    alParsedCsv.Clear();
                    break;
                case 7:
                    //wipe ending " and Seperate out value to array list
                    strElem.Remove(strElem.Length - 1, 1);
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(strTemp);
                    strElem = new StringBuilder();
                    intCurrState = 5;
                    break;
                case 8:
                    //wipe ending " and Seperate out value to array list
                    strElem.Remove(strElem.Length - 1, 1);
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(strTemp);
                    strElem = new StringBuilder();
                    //goto state 0
                    intCurrState = 0;
                    break;
            }
        }
    }
}
