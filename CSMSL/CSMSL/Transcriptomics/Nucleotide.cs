﻿// Copyright 2019 Dain R. Brademan
// Modified 2021 Qiuwen Quan
// 
// This file (Nucleotide.cs) is part of CSMSL.Transcriptomics.
// 
// CSMSL is free software: you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// CSMSL is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
// License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with CSMSL. If not, see <http://www.gnu.org/licenses/>.

using CSMSL.Chemistry;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CSMSL.Transcriptomics
{
    public class Nucleotide : INucleotide
    {
        #region The Four Common RNA and DNA Bases
        /*
        public static Nucleotide Adenosine { get; private set; }

        public static Nucleotide Cytidine { get; private set; }

        public static Nucleotide Guanosine { get; private set; }

        public static Nucleotide Uridine { get; private set; }
        */
        public static Nucleotide AdenineBase { get; private set; }
        public static Nucleotide CytosineBase { get; private set; }
        public static Nucleotide GuanineBase { get; private set; }
        public static Nucleotide UracilBase { get; private set; }

        //nucleotides for DNA: modified December 3, 2021
        public static Nucleotide DeoxyAdenineBase { get; private set; } //For DNA
        public static Nucleotide DeoxyCytosineBase { get; private set; } //For DNA
        public static Nucleotide DeoxyGuanineBase { get; private set; } //For DNA
        public static Nucleotide DeoxyThymineBase { get; private set; } //For DNA

        #endregion The Four Common RNA and DNA Bases

        private static readonly Dictionary<string, Nucleotide> Residues;

        private static readonly Nucleotide[] ResiduesByLetter;

        public static Nucleotide AddResidue(string name, char oneLetterAbbreviation, string threeLetterAbbreviation, string chemicalFormula)
        {
            var residue = new Nucleotide(name, oneLetterAbbreviation, threeLetterAbbreviation, chemicalFormula);
            AddResidueToDictionary(residue);
            return residue;
        }

        /// <summary>
        /// Get the residue based on the residues's symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static Nucleotide GetResidue(string symbol)
        {
            return symbol.Length == 1 ? ResiduesByLetter[symbol[0]] : Residues[symbol];
        }

        /// <summary>
        /// Gets the resdiue based on the residue's one-character symbol
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public static Nucleotide GetResidue(char letter)
        {
            return ResiduesByLetter[letter];
        }

        public static bool TryGetResidue(char letter, out Nucleotide residue)
        {
            residue = null;
            if (letter > 'z' || letter < 0)
                return false;
            residue = ResiduesByLetter[letter];
            return residue != null;
        }

        public static bool TryGetResidue(string symbol, out Nucleotide residue)
        {
            return Residues.TryGetValue(symbol, out residue);
        }

        /// <summary>
        /// Construct the actual nucleic acids
        /// </summary>
        static Nucleotide()
        {
            Residues = new Dictionary<string, Nucleotide>(66);
            ResiduesByLetter = new Nucleotide['z' + 1]; //Make it big enough for all the Upper and Lower characters
            
            /* previous code by DRB
            Adenosine = AddResidue("Adenosine", 'A', "Ade", "C10H12N5O6P");
            Cytidine = AddResidue("Cytidine", 'C', "Cyt", "C9H12N3O7P");
            Guanosine = AddResidue("Guanosine", 'G', "Gua", "C10H12N5O7P");
            Uridine = AddResidue("Uridine", 'U', "Ura", "C9H11N2O8P");
            */

            // actual base chemical formula after bonding with the sugar
            // the sugar and phosphate has a chemical formula of C5H8O6P1
            AdenineBase= AddResidue("Adenine", 'A', "Ade", "C5H4N5");
            CytosineBase = AddResidue("Cytosine", 'C', "Cyt", "C4H4N3O1");
            GuanineBase= AddResidue("Guanine", 'G', "Gua", "C5H4N5O1");
            UracilBase= AddResidue("Uracil", 'U', "Ura", "C4H3N2O2");

            // DNA bases which have the same mass as the ones above
            // however, naming to deoxy- to distinguish DNA nucleotide mass calculation from RNA
            DeoxyAdenineBase = AddResidue("DeoxyAdenine", 'B', "dAde", "C5H4N5");//for DNA
            DeoxyCytosineBase = AddResidue("DeoxyCytosine", 'D', "dCyt", "C4H4N3O1");//for DNA
            DeoxyGuanineBase = AddResidue("DeoxyGuanine", 'H', "dGua", "C5H4N5O1");//for DNA
            DeoxyThymineBase = AddResidue("DeoxyThymine", 'V', "dThy", "C5H5N2O2");//for DNA
        }

        private static void AddResidueToDictionary(Nucleotide residue)
        {
            Residues.Add(residue.Letter.ToString(CultureInfo.InvariantCulture), residue);
            Residues.Add(residue.Name, residue);
            Residues.Add(residue.Symbol, residue);
            ResiduesByLetter[residue.Letter] = residue;
            ResiduesByLetter[Char.ToLower(residue.Letter)] = residue;
        }

        internal Nucleotide(string name, char oneLetterAbbreviation, string threeLetterAbbreviation, string chemicalFormula)
            : this(name, oneLetterAbbreviation, threeLetterAbbreviation, new ChemicalFormula(chemicalFormula))
        {
        }
        
        internal Nucleotide(string name, char oneLetterAbbreviation, string threeLetterAbbreviation, ChemicalFormula chemicalFormula)
        {
            Name = name;
            Letter = oneLetterAbbreviation;
            Symbol = threeLetterAbbreviation;
            ChemicalFormula = chemicalFormula;

            // calculation for monoisoptic mass of DNA and RNA
            if (Name.Equals("DeoxyAdenine")|| Name.Equals("DeoxyCytosine") || Name.Equals("DeoxyGuanine") || Name.Equals("DeoxyThymine"))
            {
                sugarAndPhosphate = new ChemicalFormula("C5H8O5P1"); //DNA sugar phosphate backbone (one less oxygen than RNA)
            }
            else
            {
                sugarAndPhosphate = new ChemicalFormula("C5H8O6P1"); //RNA sugar phosphate backbone
            }
            
            MonoisotopicMass = ChemicalFormula.MonoisotopicMass+sugarAndPhosphate.MonoisotopicMass;
        }

        public ChemicalFormula ChemicalFormula { get; private set; }

        private ChemicalFormula sugarAndPhosphate;

        public char Letter { get; private set; }

        public ModificationSites Site { get; private set; }

        public double MonoisotopicMass { get; private set; }

        public string Name { get; private set; }

        public string Symbol { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2})", Letter, Symbol, Name);
        }
    }
}
