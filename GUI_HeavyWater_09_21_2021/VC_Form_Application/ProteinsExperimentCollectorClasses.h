#pragma once

#include <vector>

using namespace System;
using namespace System::Collections::Generic;

using namespace std;


//function definitions
void ReadFileNames(cli::array<System::String ^> ^inputs, List <String ^> ^asmzML, List <String ^> ^asmzID);




namespace ProteinCollector
{
	// a peptide entry
	public ref class PeptideHolder
	{
		public:
		Byte nCharge;
		Byte nDuplicity;
		double FirstScore;
		double deltaScore;
		double SeqMass;
		double SpecMass;
		String^ Peptide;
		String^ Protein;
		float SecondScore;
		int RankSecondScore;
		long int nScan;
		double dRetTime;
		float fStartElution, fEndElution;

		long int StartElution;
		long int ElutionDuration;


		short int nRank;
		
		String ^sProtId;     //example "DBSeq_1_FIBB_HUMAN"
		String ^sPepID;     // "peptide_4_1"
		String ^sEvidence;   // <PeptideEvidence id="PE_2_4_LV106_HUMAN_0_47_51

		cli::array <double, 2> ^dIsotopes;

		bool bQuant, bPeptidePassed;        // if set true, will read peptide abundance from MS1

		double dIonscore, dHomscore, dIdenscore, dExpect;

		List <int> ^ ModLocations;  //peptide modification locations
		List <double> ^dModMasses;  //modification masses, <Modification location="4" residues="M" monoisotopicMassDelta="15.994915">
	};
/*
*  a reference class that will hold protein results of a database
*  search. Every instance of this class will hold results from a s
*  single experiment. A list defined on this class will hold results
*  from multiple experiments
*
*/
	public ref class ProteinSet   //holds information about a single protein
		{
		public:
			double ProteinScore, ProteinMass;
			double SeqCoverage;    //sequence coverage
			int nSeqLength, nDistinctSequences;
			String^ accession, ^description;       //protein description from Mascot
			int nSpectralCount;

			bool bProteinPassed;

			List <PeptideHolder ^> ^Peptides;
		};

	public ref class ProteinCollection   //holder for all proteins from a single experiment
	{
	public:
		System::Collections::Generic::List<ProteinSet^> ^ ProteinsList;

		String ^sExperimentFile, ^smzML;
	};

	public ref class ExperimentCollection   //holder for all proteins from mulitple experiments
	{
	public:
		System::Collections::Generic::List<ProteinCollection^> ^ ExperimentsList;
	};

	public ref class SingleList   //holder for a single IntegerList
	{
	public:
		
		List <int> ^ IntegerList;

		List <double> ^DoubleList;

		List <String ^> ^StringList;
	};


	public ref class GeneralListCollector
	{
	public:
		List <SingleList ^> ^aList;

	};

	public ref class ProteinList
	{
	public:
		String ^Accession, ^Protein;

		bool bProteinPassed;

		List <int> ^ ProteinInExperiments;
	};


	public ref class PeptideList
	{
	public:

		PeptideHolder ^Peptide;

		bool bPeptidePassed;

		List <int> ^ PeptideInExperiments;
	};
} //name psace ProteinCollector

typedef struct _OneSet
{
	int Value;
} OneSet;

typedef vector <OneSet> ListCollector;
