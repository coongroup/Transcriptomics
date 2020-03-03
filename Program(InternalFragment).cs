using CSMSL;
using CSMSL.IO.Thermo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OligoSeq
{   /// <summary>
    /// Description of this program: 
    /// Creates a CSV file with possible RNA internal fragments and their corresponding m/z. 
    /// Output csv file includes Fragment, ChargeState, m/z, intensity values.
    /// m/z values of internal fragments are read from the textfile InternalFragmentMZ.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //Specify raw file path and output path
            var outputPath = @"insert\output\path\";
            var rawfilePath = @"insert\raw\file\path\rawfileName.raw";
           
            var outputFileName = Path.GetFileNameWithoutExtension(rawfilePath) + "(InternalFragments).csv";
            var massTolerance = new Tolerance("+-10 PPM");

            //Text file contains the different types of internal fragments and their corresponding mz with different charges
            //m/z values in this file is used to extract their corresponding intensity in the raw file
            StreamReader reader = new StreamReader(@"insert\file\path\to\textfile\" + "InternalFragMZ_21nt.txt");

            // steamwriter to write results out
            var writer = new StreamWriter(outputPath + outputFileName);

            //access rawfile and open a connection to it
            var rawfile = new ThermoRawFile(rawfilePath);
            rawfile.Open();

            // get last scan number
            var lastScanNumber = rawfile.LastSpectrumNumber;

            // variable to keep track of the total internal fragment intensities
            var totalInternalFragment = 0.0;
            
            // headers for the csv file
            writer.WriteLine("Fragment, ChargeState, m/z, intensity");
           
            // referenced https://stackoverflow.com/questions/23225973/parsing-tab-delimited-text-files to parse from text file
            char[] delimiter = new char[] { '\t' };

            // charge of a fragment m/z is in the first row of the textfile
            string[] chargeRow = reader.ReadLine().Split(delimiter);

            var lastScan = rawfile.GetSpectrum(lastScanNumber);

            while (reader.Peek() > 0)
            { 
                string[] currentRow = reader.ReadLine().Split(delimiter);
                string fragmentType = currentRow[1];
                //go through all the internal fragment m/z in each row, m/z values start in column 2
                for (int i=2; i < currentRow.Length; i++)
                {
                    double fragmentMz;
                    //break if get to end of the row and there are no more m/z values left
                    if(!Double.TryParse(currentRow[i], out fragmentMz))
                    {
                        break;
                    }

                    var fragmentCharge = chargeRow[i];

                    //get the average of the internal fragment
                    double tempIntensities = 0;
                    //get the intensity of the fragment in the last scan
                    var range = new MzRange(fragmentMz, massTolerance);
                    lastScan.TryGetIntensities(range, out tempIntensities);

                    //skip the fragments that are not found within the spectra
                    if (tempIntensities.Equals(double.NaN)|| tempIntensities.Equals(0.0))
                    {
                        continue;
                    }

                    //sum up all the internal fragment intensities
                    totalInternalFragment += tempIntensities;
                    
                    //print to csv file
                    writer.WriteLine(fragmentType +","+ fragmentCharge + "," + fragmentMz + "," + tempIntensities);
                }
            }
            writer.Close();
            writer.Dispose();
        }
    }
}
