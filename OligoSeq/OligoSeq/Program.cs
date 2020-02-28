namespace OligoSeq
{
    using CSMSL;
    using CSMSL.IO.Thermo;
    using CSMSL.Transcriptomics;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Description of this program: 
    /// Creates a CSV file with possible RNA fragment types and their corresponding m/z.
    /// Output csv file includes Fragment, ChargeState, m/z, intensity values
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The Main method
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/></param>
        internal static void Main(string[] args)
        {
            // Specify file paths, peptide, and charge
            var outputPath = @"insert\output\path\";
            var rawfilePath = @"insert\raw\file\path.raw";
            var outputFileName = Path.GetFileNameWithoutExtension(rawfilePath) + ".csv";

            var massTolerance = new Tolerance("+-10 PPM");

            var oligonucleotide = new RNA("RNA sequence"); // enter G,U,A,C sequence without modifications
            var precursorCharge = 4; //enter isolated precursor charge state in negative mode-ESI

            // add modification at necessary site along RNA sequence (beginning from 5'-terminus)
            Modification methylModification = null;

            if (ModificationDictionary.TryGetModification("2'-Methylation", out methylModification))
            {
                oligonucleotide.AddModification(methylModification, 2);     // i.e., for GmUACUG adds one methyl on the first uradine at position 2

                oligonucleotide.AddModification(methylModification, 5);     
            }

            // calculate the precursor m/z for the oligonucleotide
            var fragmentsInQuestion = FragmentTypes.aBase | FragmentTypes.bBase | FragmentTypes.cBase | FragmentTypes.dBase | FragmentTypes.wBase | FragmentTypes.xBase | FragmentTypes.yBase | FragmentTypes.zBase | FragmentTypes.a | FragmentTypes.b | FragmentTypes.c | FragmentTypes.d | FragmentTypes.w | FragmentTypes.x | FragmentTypes.y | FragmentTypes.z;
            
            // generate a set of theoretical fragments for the oligonucleotide
            var theoreticalFragments = oligonucleotide.Fragment(fragmentsInQuestion); // FragmentTypes.a | FragmentTypes.d | FragmentTypes.w | FragmentTypes.y | FragmentTypes.c

            // steamwriter to write results out
            var writer = new StreamWriter(outputPath + outputFileName);

            //access rawfile and open a connection to it
            var rawfile = new ThermoRawFile(rawfilePath);
            rawfile.Open();

            // get last scan number
            var lastScanNumber = rawfile.LastSpectrumNumber;

            // header of each column
            writer.WriteLine("Fragment, ChargeState, m/z, intensity");

            //get TIC
            double TIC = rawfile.GetTIC(lastScanNumber);
            writer.WriteLine("LastScan TIC, " + TIC);

            foreach (var fragment in theoreticalFragments)
            {

                // go through the different charge states and get the average intensity of that fragment m/z
                for (int z = 1; z <= fragment.Number; z++)
                {
                    //charge state cannot go past the precursor charge
                    if (z >= precursorCharge) { continue; }

                    // subtracting the base from the monoisotopic mass for the a-Base fragment
                    double fragmentMass = fragment.MonoisotopicMass;
                    if (fragment.Type == FragmentTypes.aBase || fragment.Type == FragmentTypes.bBase || fragment.Type == FragmentTypes.cBase || fragment.Type == FragmentTypes.dBase)
                    {
                        if (fragment.Number == z) { continue; }
                        double baseMass = fragment.Parent.NucleicAcidArray[fragment.Number - 1].ChemicalFormula.MonoisotopicMass;
                        fragmentMass = fragmentMass - baseMass;
                    }
                    else if (fragment.Type == FragmentTypes.wBase || fragment.Type == FragmentTypes.xBase || fragment.Type == FragmentTypes.yBase || fragment.Type == FragmentTypes.zBase)
                    {
                        if (fragment.Number == z) { continue; }
                        int lengthOfOlig = fragment.Parent.NucleicAcidArray.Length;
                        double baseMass = fragment.Parent.NucleicAcidArray[lengthOfOlig - fragment.Number].ChemicalFormula.MonoisotopicMass;
                        fragmentMass = fragmentMass - baseMass;
                    }

                    // calculate the fragment m/z 
                    var fragmentMz = (fragmentMass - z * Constants.Proton) / (z);

                    double averageIntensity = 0.0;
                    // get the intensity of the fragement in the last scan
                    var lastScan = rawfile.GetSpectrum(lastScanNumber);
                    var range = new MzRange(fragmentMz, massTolerance);
                    lastScan.TryGetIntensities(range, out averageIntensity);

                    // skip the fragments that are not found within the spectra
                    if (averageIntensity.Equals(double.NaN) || averageIntensity == 0.0)
                    {
                        continue;
                    }

                    //formatting output file: print the fragment name, the charge state, and the m/z value
                    writer.Write(fragment.Type);
                    if (fragment.Number < 10) //formatting so numbers less than 10 will also occupy 2 character lengths, allow for easier sorting of fragments in excel
                    {
                        writer.Write("0" + fragment.Number + ",");
                    }
                    else
                    {
                        writer.Write(fragment.Number + ",");
                    }
                    writer.Write("-" + z + ",");
                    writer.Write(fragmentMz + "," + averageIntensity);

                    //formatting the output file: going to next line
                    writer.WriteLine();

                }

            }

            // trash writer just to be safe.
            writer.Close();
            writer.Dispose();
        }
    }
}
