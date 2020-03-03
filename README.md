# Transcriptomics
Source code for RNA MS analysis.

## Files
### Program.cs 
- Extracts intensities of all fragments present in the raw file
- Note: When running the script for unmodified RNA, comment out the section regarding modifications in Program.cs (line 33-40)

Change these variable values:
- outputPath
- rawfilePath
- oligonucleotide: G, U, A, C sequence without modifications

### Program(InternalFragments).cs 
- Extracts intensities of all internal fragments (fragments without either terminus) present in the raw file
- Utilizes InternalFragMZ_21nt.txt for m/z values needed to extract the corresponding intensity
- Note: To run Program(InternalFragments).cs, rename Program.cs to Program(fragments).cs and rename Program(InternalFragments).cs to Program.cs

Change these variable values:
- outputPath
- rawfilePath
- SteamReader parameter: for Program(InternalFragments).cs only (line 30), to specify the location of the text file

#### InternalFragMZ_21nt.txt
- Text file contains calculated m/z of all possible internal fragments of the 21nt RNA. 
- First row contains the charge state.
- First column contains the monoisotopic mass of an internal fragment
- Second column contains the position of the internal fragment.
