namespace OligoSeq
{
    using CSMSL;
    using CSMSL.IO.Thermo;
    using CSMSL.Transcriptomics;
    using System.Collections.Generic;
    using System.IO;
    using System;
    using CSMSL.Chemistry; //added to use the ChemicalFormula function to obtain the mass of an oxygen
    using System.Linq; // https://stackoverflow.com/questions/1654209/how-to-access-index-in-ienumerable-object-in-c
   
    internal class Program
    {
        /// <summary>
        /// The Main method
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/></param>
        internal static void Main(string[] args)
        {
            /// list of variables to edit (in order of appearance in this file):
            /// outputpath, folderpath, massTolerance, sequence, precursorCharge, RNAorDNA
            /// fragmentsInQuestion (can change definition depending on which fragments are of interest for analysis)
            /// list of function calls that can be edited: 
            /// oligonucleotide.AddModification for adding modifications to the oligo sequence

            // setting some variables to specify file paths, peptide, and charge
            var outputPath = @"insert\output\path\";
            var folderPath = @"insert\folderWithRawFile\path\"; //folder with the raw files
            
            // obtain all the file path names into a string array and iterate through all of the files in the for loop
            string[] filePaths = Directory.GetFiles(folderPath, "*.raw");

            // go through all raw files in the folder
            for (int i = 0; i < filePaths.Length; i++)
            {
                // might need to change the Tolerance depending on the theoretical and experimental mass
                var massTolerance = new Tolerance("+-10 PPM");// tolerance can also be in dalton units: new Tolerance("+- .03 Da")

                // sequence of the oligonucleotides using the bases of ATCG
                var sequence = "UACAGCAUCGGCCUGGACAU";  // 20 nt: UACAGCAUCGGCCUGGACAU (PS mod on location 1-20)
                                                        // 6 nt: GUACUG (methylation on location 2)
                var precursorCharge = 10; // magnitude of the charge. if charge is z= -30, then input 30 for the value

                var RNAorDNA = "RNA"; // set this to either a string of: "DNA" or "RNA"

                var oligonucleotide = new RNA(""); // assigned empty string to RNA which will later be updated based on whether the sequence is of a DNA or RNA

                var rawfilePath = filePaths[i];
                var outputFileName = Path.GetFileNameWithoutExtension(rawfilePath) + "_output.csv";

                if ((RNAorDNA.ToUpper()).Equals("RNA"))
                {
                    oligonucleotide = new RNA(sequence);
                }
                else if ((RNAorDNA.ToUpper()).Equals("DNA")) // converting the sequence to corresponding DNA "bases" (B,D,V,H) in the code
                {
                    var intermed_sequence = sequence.Replace('A', 'B');
                    intermed_sequence = intermed_sequence.Replace('C', 'D');
                    intermed_sequence = intermed_sequence.Replace('T', 'V');
                    intermed_sequence = intermed_sequence.Replace('G', 'H');
                    sequence = intermed_sequence;
                    oligonucleotide = new RNA(sequence);
                }
                
                // add phosphorothioate modification
                // 20nt sequence UACAGCAUCGGCCUGGACAU has PS mod on location 1-20
                Modification PhosphorothioateMod = null;
                if (ModificationDictionary.TryGetModification("Phosphorothioate", out PhosphorothioateMod))
                {
                    oligonucleotide.AddModification(PhosphorothioateMod, 1); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 2); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 3); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 4); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 5); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 6); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 7); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 8); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 9); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 10); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 11); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 12); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 13); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 14); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 15); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 16); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 17); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 18); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 19); //set this modification to the 5' nucleotide of the PS bond
                    oligonucleotide.AddModification(PhosphorothioateMod, 20); //set this modification to the 5' nucleotide of the PS bond
                    
                }

                /*
                // add methylation modification
                // 6nt sequence GUACUG has methyl mod on location 2
                Modification methylModification = null;
                if (ModificationDictionary.TryGetModification("2'-Methylation", out methylModification))
                {
                    //oligonucleotide.AddModification(methylModification, 2);
                }
                */

                /*
                // add fluoro modification 
                Modification fluoroModification = null;
                if (ModificationDictionary.TryGetModification("2'-Fluorine", out fluoroModification))
                {
                    //oligonucleotide.AddModification(fluoroModification, 6);
                }
                */

                /* 
                // yW modification
                 Modification yWModification = null;
                 if (ModificationDictionary.TryGetModification("yW", out yWModification))
                 {
                     oligonucleotide.AddModification(yWModification, 1);     // yW (on a guanosine)
                 }
                */


                // calculating the precursor m/z for the oligonucleotide
                var fragmentsInQuestion = FragmentTypes.a | FragmentTypes.b | FragmentTypes.c | FragmentTypes.d | FragmentTypes.w | FragmentTypes.x | FragmentTypes.y | FragmentTypes.z | FragmentTypes.aBase | FragmentTypes.bBase | FragmentTypes.cBase | FragmentTypes.dBase | FragmentTypes.wBase | FragmentTypes.xBase | FragmentTypes.yBase | FragmentTypes.zBase;
                // FragmentTypes.a | FragmentTypes.b | FragmentTypes.c | FragmentTypes.d | FragmentTypes.w | FragmentTypes.x | FragmentTypes.y | FragmentTypes.z;
                // -base fragments:
                // FragmentTypes.aBase | FragmentTypes.bBase | FragmentTypes.cBase | FragmentTypes.dBase | FragmentTypes.wBase | FragmentTypes.xBase | FragmentTypes.yBase | FragmentTypes.zBase |

                /// Note to User: there should be no need to edit any of the code below here///
                
                // difference between Sulfer and Phosphorous for phosphothioate modification calculation
                ChemicalFormula oxygen = new ChemicalFormula("O");
                ChemicalFormula sulfer = new ChemicalFormula("S");
                var SO_mass_diff = sulfer.MonoisotopicMass - oxygen.MonoisotopicMass;

                // generate a set of theoretical fragments for the oligonucleotide
                var theoreticalFragments = oligonucleotide.Fragment(fragmentsInQuestion); 

                // steamwriter to write results out
                var writer = new StreamWriter(outputPath + outputFileName);

                // access rawfile and open a connection to it
                var rawfile = new ThermoRawFile(rawfilePath);
                rawfile.Open();

                // get last scan number
                var lastScanNumber = rawfile.LastSpectrumNumber;

                // header of each column
                writer.WriteLine("Fragment, ChargeState, m/z, distance from 5' terminus, intensity");
                
                // get TIC
                double TIC = rawfile.GetTIC(lastScanNumber);
                writer.WriteLine("LastScan TIC, " + TIC);

                // all modifications on the oligonucleotide, after "using System.Linq", we can use ienumerableVariable.ElementAt(index) to access the item at the index
                var allmodslist = oligonucleotide.GetModifications(); 

                // obtain mass and intensity of each fragment with various charge states 
                foreach (var fragment in theoreticalFragments)
                {
                    //  go through the different charge state and get the average intensity of that fragment m/z
                    for (int z = 1; z <= fragment.Number; z++)
                    {
                        // charge state cannot exceed the precursor charge
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

                        // editing the fragmentMass for fragments that are affected by a phosphorothioate modification
                        if (fragment.Type == FragmentTypes.a || fragment.Type == FragmentTypes.b || fragment.Type == FragmentTypes.aBase || fragment.Type == FragmentTypes.bBase)
                        {
                            var positionMod = allmodslist.ElementAt(fragment.Number);
                            if (positionMod != null)
                            {
                                var modName = positionMod.ToString();
                                if (modName.Equals("Phosphorothioate"))
                                {
                                    fragmentMass = fragmentMass - SO_mass_diff;
                                }
                            }
                        }
                        else if (fragment.Type == FragmentTypes.w || fragment.Type == FragmentTypes.x || fragment.Type == FragmentTypes.wBase || fragment.Type == FragmentTypes.xBase)
                        {
                            var PSindex = sequence.Length - fragment.Number;
                            if (PSindex != 0)
                            {
                                var positionMod = allmodslist.ElementAt(sequence.Length - fragment.Number);
                                if (positionMod != null)
                                {
                                    var modName = positionMod.ToString();
                                    if (modName.Equals("Phosphorothioate"))
                                    {
                                        fragmentMass = fragmentMass + SO_mass_diff;
                                    }

                                }

                            }

                        }

                        // calculating the fragment m/z for larger fragments, need to account for additional peaks in isotopic envelope
                        List<double> masses = new List<double>();

                        double fragmentMz = (fragmentMass - z * Constants.Proton) / (z);
                        double fragmentMz1 = (fragmentMass - z * Constants.Proton + Constants.C13C12Difference * 1) / (z);
                        double fragmentMz2 = (fragmentMass - z * Constants.Proton + Constants.C13C12Difference * 2) / (z);
                        double fragmentMz3 = (fragmentMass - z * Constants.Proton + Constants.C13C12Difference * 3) / (z);
                        double fragmentMz4 = (fragmentMass - z * Constants.Proton + Constants.C13C12Difference * 4) / (z);
                        double fragmentMz5 = (fragmentMass - z * Constants.Proton + Constants.C13C12Difference * 5) / (z);
                        double fragmentMz6 = (fragmentMass - z * Constants.Proton + Constants.C13C12Difference * 6) / (z);
                        double fragmentMz7 = (fragmentMass - z * Constants.Proton + Constants.C13C12Difference * 7) / (z);
                        double fragmentMz8 = (fragmentMass - z * Constants.Proton + Constants.C13C12Difference * 8) / (z);
                        double fragmentMz9 = (fragmentMass - z * Constants.Proton + Constants.C13C12Difference * 9) / (z);

                        if (z == 1)
                        {
                            masses.Add(fragmentMz);
                            masses.Add(fragmentMz1);
                            masses.Add(fragmentMz2);
                        }
                        else if (z == 2)
                        {
                            masses.Add(fragmentMz);
                            masses.Add(fragmentMz1);
                            masses.Add(fragmentMz2);
                            masses.Add(fragmentMz3);
                        }
                        else if (z == 3)
                        {
                            masses.Add(fragmentMz);
                            masses.Add(fragmentMz1);
                            masses.Add(fragmentMz2);
                            masses.Add(fragmentMz3);
                            masses.Add(fragmentMz4);
                            masses.Add(fragmentMz5);
                        }
                        else if (z == 4)
                        {
                            masses.Add(fragmentMz);
                            masses.Add(fragmentMz1);
                            masses.Add(fragmentMz2);
                            masses.Add(fragmentMz3);
                            masses.Add(fragmentMz4);
                            masses.Add(fragmentMz5);
                            masses.Add(fragmentMz6);

                        }
                        else if (z >= 5)
                        {
                            masses.Add(fragmentMz);
                            masses.Add(fragmentMz1);
                            masses.Add(fragmentMz2);
                            masses.Add(fragmentMz3);
                            masses.Add(fragmentMz4);
                            masses.Add(fragmentMz5);
                            masses.Add(fragmentMz6);
                            masses.Add(fragmentMz7);
                            masses.Add(fragmentMz8);
                            masses.Add(fragmentMz9);
                        }

                        double averageIntensity = 0.0;

                        // get the intensity of the fragement in the last scan
                        var lastScan = rawfile.GetSpectrum(lastScanNumber);

                        double qiuwenIntensity = 0.0;

                        foreach (var isotope in masses)
                        {
                            var range = new MzRange(isotope, massTolerance);

                            // get charge of peak
                            var minMz = range.Minimum;
                            var maxMz = range.Maximum;
                            var allMasses = lastScan.GetMasses();

                            lastScan.TryGetIntensities(range, out averageIntensity);

                            int massIndex = Array.BinarySearch(allMasses, minMz);

                            if (massIndex < 0)
                            {
                                massIndex = ~massIndex;
                            }

                            List<int> indexArray = new List<int>();
                            var allIntensities = lastScan.GetIntensities();

                            while (massIndex < allMasses.Length && allMasses[massIndex] <= maxMz)
                            {
                                indexArray.Add(lastScan.GetCharge(massIndex));
                                qiuwenIntensity += allIntensities[massIndex];
                                indexArray.Add(massIndex); 
                                massIndex++;
                            }

                            // skip the fragments that are not found within the spectra
                            if (averageIntensity.Equals(double.NaN) || averageIntensity == 0.0)
                            {
                                continue;
                            }

                            // CHARGE-BASED FILTER: if the charge of the actual fragment/peaks does not align with the predicted fragment, skip this fragment
                            if (!indexArray.Contains(z))
                            {
                                continue;
                            }

                            // formatting output file: print the fragment name, the charge state (which is the fragment number), and the m/z value
                            writer.Write(fragment.Type);
                            if (fragment.Number < 10) // numbers less than 10 will also occupy 2 character lengths, allow for easier sorting of fragments in excel for numbers 1-99
                            {
                                writer.Write("0" + fragment.Number + ",");
                            }
                            else
                            {
                                writer.Write(fragment.Number + ",");
                            }
                            writer.Write("-" + z + ",");

                            writer.Write(isotope + ",");

                            if (fragment.Type == FragmentTypes.a || fragment.Type == FragmentTypes.b || fragment.Type == FragmentTypes.c || fragment.Type == FragmentTypes.d ||
                                fragment.Type == FragmentTypes.aBase || fragment.Type == FragmentTypes.bBase || fragment.Type == FragmentTypes.cBase || fragment.Type == FragmentTypes.dBase)
                            {
                                writer.Write(fragment.Number + ",");
                            }
                            else
                            {
                                int distanceFrom5prime = sequence.Length - fragment.Number;
                                writer.Write(distanceFrom5prime + ",");
                            }
                            writer.Write(averageIntensity + ",");

                            // formatting the output file: going to next line
                            writer.WriteLine();
                        }

                    }

                }
                // trash writer just to be safe.
                writer.Close();
                writer.Dispose();
            }


        }
    }
}
