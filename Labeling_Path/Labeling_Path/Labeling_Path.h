// Labeling_Path.h

#pragma once

using namespace System;

namespace Labeling_Path {

	public ref class Label_Path
	{
		// TODO: Add your methods for this class here.

	public:
		array <float> ^fFractonalSynthesis, ^fBWE_Array;
		float fBWE;

		int Labeling_Path_Fractional_Synthesis(array <float, 2> ^ Fractional_Synthesis_Rate, 
			int Nraw, int Ncol, int NEH, float fBWE);


		float Diff_Fraction_Rates(float Fractional_Synthesis_Rate1, 
			float Fractional_Synthesis_Rate2);

	};
}
