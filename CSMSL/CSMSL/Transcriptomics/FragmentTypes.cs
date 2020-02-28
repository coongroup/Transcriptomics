 // Copyright 2019 Dain R. Brademan
// 
// This file (FragmentTypes.cs) is part of CSMSL.Transcriptomics.
// 
// CSMSL is free software: you can resdistribute it and/or modify it
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

namespace CSMSL.Transcriptomics
{
    [Flags]
    public enum FragmentTypes
    {
        None = 0,
        a = 1 << 0,
        adot = 1 << 1,
        aBase = 1 << 2,
        b = 1 << 3,
        bdot = 1 << 4,
        bBase = 1 << 5,
        c = 1 << 6,
        cdot = 1<<7,
        cBase = 1 << 8,
        d = 1 << 9,
        ddot = 1 << 10,
        dBase = 1<<11,
        dH2O= 1<<12,
        w = 1 << 13,
        wdot = 1 << 14,
        wBase = 1 << 15,
        x = 1 << 16,
        xdot = 1 << 17,
        xBase = 1 << 18,
        y = 1 << 19,
        ydot = 1 << 20,
        yBase = 1 << 21,
        z = 1 << 22,
        zdot = 1 << 23,
        zBase = 1 << 24,
        Internal = 1 << 25,
        All = (1 << 25) - 1, // Handy way of setting all below the 32th bit
        
        
    }

    public static class FragmentTypesExtension
    {
        public static IEnumerable<FragmentTypes> GetIndividualFragmentTypes(this FragmentTypes fragmentTypes)
        {
            if (fragmentTypes == FragmentTypes.None)
                yield break;
            foreach (FragmentTypes site in Enum.GetValues(typeof(FragmentTypes)))
            {
                if (site == FragmentTypes.None || site == FragmentTypes.All || site == FragmentTypes.Internal)
                {
                    continue;
                }
                if ((fragmentTypes & site) == site)
                {
                    yield return site;
                }
            }
        }

        public static Terminus GetTerminus(this FragmentTypes fragmentType)
        {
            // Super handy: http://stackoverflow.com/questions/4624248/c-logical-riddle-with-bit-operations-only-one-bit-is-set
            if (fragmentType == FragmentTypes.None || (fragmentType & (fragmentType - 1)) != FragmentTypes.None)
            {
                throw new ArgumentException("Fragment Type must be a single value to determine the terminus", "fragmentType");
            }
            var returnValue = fragmentType >= FragmentTypes.w ? Terminus.ThreePrime : Terminus.FivePrime;
            return returnValue;
        }

        public static ChemicalFormula GetIonCap(this FragmentTypes fragmentType)
        {
            if (fragmentType == FragmentTypes.None || (fragmentType & (fragmentType - 1)) != FragmentTypes.None)
            {
                throw new ArgumentException("Fragment Type must be a single value to determine the ion cap", "fragmentType");
            }
            
            return FragmentIonCaps[fragmentType];
        }

        private static readonly Dictionary<FragmentTypes, ChemicalFormula> FragmentIonCaps = new Dictionary<FragmentTypes, ChemicalFormula>
        {
            {FragmentTypes.a, new ChemicalFormula("H-1")},
            {FragmentTypes.adot, new ChemicalFormula()},
            {FragmentTypes.b, new ChemicalFormula("OH1")},
            {FragmentTypes.bdot, new ChemicalFormula("OH2")},
            {FragmentTypes.c, new ChemicalFormula("O3P")},     
            {FragmentTypes.cdot, new ChemicalFormula("O3HP")},       
            {FragmentTypes.d, new ChemicalFormula("O4H2P")},
            {FragmentTypes.ddot, new ChemicalFormula("O4H3P")},
            {FragmentTypes.w, new ChemicalFormula("H")},
            {FragmentTypes.wdot, new ChemicalFormula("H2")},
            {FragmentTypes.x, new ChemicalFormula("O-1H-1")},
            {FragmentTypes.xdot, new ChemicalFormula("O-1")},
            {FragmentTypes.y, new ChemicalFormula("O-3P-1")},
            {FragmentTypes.ydot, new ChemicalFormula("O-3HP-1")},
            {FragmentTypes.z, new ChemicalFormula("O-4H-2P-1")},
            {FragmentTypes.zdot, new ChemicalFormula("O-4H-1P-1")},
            //fragment - Base chemical formula is the corresponding fragment chemical formula subtracing 1 H as H is lost when base is removed
            {FragmentTypes.aBase, new ChemicalFormula("H-2")}, // "H-1" -H 
            {FragmentTypes.bBase, new ChemicalFormula("O")}, //"OH1" -H
            {FragmentTypes.cBase, new ChemicalFormula("O3H-1P")}, //"O3P" -H
            {FragmentTypes.dBase, new ChemicalFormula("O4H1P")}, //"O4H2P" -H
            {FragmentTypes.wBase, new ChemicalFormula()}, //"H"-H
            {FragmentTypes.xBase, new ChemicalFormula("O-1H-2")}, //"O-1H-1" -H
            {FragmentTypes.yBase, new ChemicalFormula("O-3H-1P-1")}, //"O-3P-1" -H
            {FragmentTypes.zBase, new ChemicalFormula("O-4H-3P-1")}, //"O-4H-3P-1" -1
            //d-H2O
            {FragmentTypes.dH2O, new ChemicalFormula("O3P")},
        };
    }
}
