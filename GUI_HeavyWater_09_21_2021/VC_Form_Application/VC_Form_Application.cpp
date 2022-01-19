// VC_Form_Application.cpp : main project file.

#include "stdafx.h"
#include "Form1.h"
//#include "Caller_mzIdentML.h"
using namespace VC_Form_Application;

[STAThreadAttribute]
int main(cli::array<System::String ^> ^args)
{
	 
	
	// Enabling Windows XP visual effects before any controls are created
	Application::EnableVisualStyles();
	Application::SetCompatibleTextRenderingDefault(false); 
	
	// Create the main window and run it
	Application::Run(gcnew VC_Form_Application::Form1());
	
	return 0;
}
