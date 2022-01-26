////////////////////////////////Main GUI Form Application for d2ome////////////////////////////////

////////////////////////////////Sadygov Lab Biochemistry and Molecular Biology, UTMB///////////////

///////////////////////////////////////////////////////////////////////////////////////////////////


#pragma once

#include <fstream>
#include <cstring>
#include <string>
#include <map>
#include <utility>
#include <sstream>
#include "Form2.h"
#include "ctype.h"
//#include "WinUser.h"

//#include "Test.h"
#include "ProteinsExperimentCollectorClasses.h"

#define TIME_COURSE_POINT 15

using namespace std;

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;

using namespace System::Runtime::InteropServices;

using namespace System::IO;
using namespace System::Threading;
using namespace v2;


//using namespace pepXML;                                   //for definitions in mzIdentML
//using namespace ProteinCollector;                         //for definitions in this project
//using namespace IsotopePeaks;                             //for computing isotopes
//using namespace std;
//using namespace PeakDetectionIntegration;
//using namespace XmlMzML;

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace VC_Form_Application {

	/// <summary>
	/// Summary for Form1
	/// </summary>
	public ref class Form1 : public System::Windows::Forms::Form
	{

	public:

		Form1(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
			fbd = gcnew FolderBrowserDialog;
			filename1 = gcnew String("");
			filename2 = gcnew String("");
			filename3 = gcnew String("");
			filename4 = gcnew String("");
			time_course_day = new double[TIME_COURSE_POINT];
			time_course_hour = new double[TIME_COURSE_POINT];

			/*v2::FormLoader^ fl;
			fl= gcnew v2::FormLoader();
			fl.startVisualizerForm();*/



		}

	public:
		int ret;
		int rate_constant_choice;
		String^ elution_window;
		//DisableProcessWindowsGhosting Lib "user32" (); //Disable Not Responding Text
		int x_70 = 70, x_130 = 130, x_250 = 250, x_460 = 460;   //originally = 88

		String^ sExecsFolder;

		short unsigned int MS1_type = 0;    // 0 is Profile in MS1, 1 is Centroid in MS1

	public: static int progress1, progress2, progress3, progress4;



	private: System::Windows::Forms::Label^ Rate_Constant_label;



	private: System::Windows::Forms::Label^ Mass_accuracy_label;
	private: System::Windows::Forms::TextBox^ Mass_Accuracy_textBox;

	private: System::Windows::Forms::Label^ enrichment_label;
	private: System::Windows::Forms::Label^ Information_label;
	private: System::Windows::Forms::Label^ Output_label;
	private: System::Windows::Forms::TextBox^ Output_textBox;
	private: System::Windows::Forms::Button^ Output_browse_button; 
	private: System::Windows::Forms::Button^ Output_visualize_button;
	private: System::Windows::Forms::ComboBox^ Enrichment_comboBox;
	private: System::Windows::Forms::Label^ Elution_label;
	private: System::Windows::Forms::TextBox^ Elution_textBox;
	private: System::Windows::Forms::OpenFileDialog^ OFDmzML;
	private: System::Windows::Forms::OpenFileDialog^ OFDmzID;


	private: System::Windows::Forms::ComboBox^ Rate_Constant_comboBox2;

			 //static int finished;
	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~Form1()
		{
			if (components)
			{
				delete components;
			}

			if (time_course_day)
				delete[]time_course_day;
			if (time_course_hour)
				delete[]time_course_hour;
		}
	private: System::Windows::Forms::Label^ mzML_label;
	private: System::Windows::Forms::TextBox^ mzML_textBox;
	private: System::Windows::Forms::Button^ mzML_Browse;
	private: System::Windows::Forms::Button^ mzML_Add;
	private: System::Windows::Forms::Button^ mzML_Remove;
	private: System::Windows::Forms::TextBox^ mzML_RichtextBox;
	private: System::Windows::Forms::FolderBrowserDialog^ fbd;
	private: String^ filename1;
	private: String^ filename2;
	private: String^ filename3;
	private: String^ filename4;

	private: double* time_course_day;
	private: double* time_course_hour;


	private: System::Windows::Forms::Label^ mzID_label;
	private: System::Windows::Forms::TextBox^ mzID_textBox;
	private: System::Windows::Forms::Button^ mzID_Browse;
	private: System::Windows::Forms::Button^ mzID_Add;
	private: System::Windows::Forms::Button^ mzID_Remove;
	private: System::Windows::Forms::TextBox^ mzID_RichtextBox;






	private: System::Windows::Forms::Button^ Start_button;
	private: System::Windows::Forms::Button^ About_button;
	private: System::Windows::Forms::ComboBox^ comboBox1;
	private: System::Windows::Forms::Label^ label1;



	private: System::Windows::Forms::TextBox^ PeptideConsistency_textBox;
	private: System::Windows::Forms::Label^ PeptideConsistency_Label;

	private: System::Windows::Forms::ComboBox^ MS1_comboBox;
	private: System::Windows::Forms::Label^ MS1_Label;

	private: System::Windows::Forms::Label^ PepScore_Label;
	private: System::Windows::Forms::TextBox^ textBox_PepScore;

	private: System::Windows::Forms::TextBox^ textBox1;
	private: System::Windows::Forms::Label^ T0_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE0;
	private: System::Windows::Forms::Label^ BWE0_label;
	private: System::Windows::Forms::TextBox^ textBox2;
	private: System::Windows::Forms::Label^ T1_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE1;
	private: System::Windows::Forms::Label^ BWE1_label;
	private: System::Windows::Forms::TextBox^ textBox3;
	private: System::Windows::Forms::Label^ T2_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE2;
	private: System::Windows::Forms::Label^ BWE2_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE3;
	private: System::Windows::Forms::Label^ BWE3_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE4;
	private: System::Windows::Forms::Label^ BWE4_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE5;
	private: System::Windows::Forms::Label^ BWE5_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE6;
	private: System::Windows::Forms::Label^ BWE6_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE7;
	private: System::Windows::Forms::Label^ BWE7_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE8;
	private: System::Windows::Forms::Label^ BWE8_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE9;
	private: System::Windows::Forms::Label^ BWE9_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE10;
	private: System::Windows::Forms::Label^ BWE10_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE11;
	private: System::Windows::Forms::Label^ BWE11_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE12;
	private: System::Windows::Forms::Label^ BWE12_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE13;
	private: System::Windows::Forms::Label^ BWE13_label;
	private: System::Windows::Forms::TextBox^ textBox_BWE14;
	private: System::Windows::Forms::Label^ BWE14_label;
	private: System::Windows::Forms::TextBox^ textBox4;
	private: System::Windows::Forms::Label^ T3_label;
	private: System::Windows::Forms::TextBox^ textBox5;
	private: System::Windows::Forms::Label^ T4_label;
	private: System::Windows::Forms::Label^ T14_label;

	private: System::Windows::Forms::TextBox^ textBox6;
	private: System::Windows::Forms::Label^ T13_label;

	private: System::Windows::Forms::TextBox^ textBox7;
	private: System::Windows::Forms::Label^ T12_label;

	private: System::Windows::Forms::TextBox^ textBox8;
	private: System::Windows::Forms::Label^ T11_label;

	private: System::Windows::Forms::TextBox^ textBox9;
	private: System::Windows::Forms::Label^ T10_label;

	private: System::Windows::Forms::TextBox^ textBox10;

	private: System::Windows::Forms::TextBox^ textBox11;
	private: System::Windows::Forms::Label^ T5_label;
	private: System::Windows::Forms::TextBox^ textBox12;
	private: System::Windows::Forms::Label^ T6_label;
	private: System::Windows::Forms::TextBox^ textBox13;
	private: System::Windows::Forms::Label^ T7_label;
	private: System::Windows::Forms::TextBox^ textBox14;
	private: System::Windows::Forms::Label^ T8_label;
	private: System::Windows::Forms::TextBox^ textBox15;
	private: System::Windows::Forms::Label^ T9_label;


	private: System::ComponentModel::IContainer^ components;




	protected:

	protected:

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>

/*
*
*  The following code sets up the positions of the labels and corresponding
*  boxes to enter their values. For time label and corresponding box, there is
*  1 difference, e.g. T0_label matches with the text box textBox1. The position
*  of the text box is +28 in x axis, and the same y-axis coordinate.
*
*
*
*/
#pragma region Windows Form Designer generated code
/// <summary>
/// Required method for Designer support - do not modify
/// the contents of this method with the code editor.
/// </summary>
		void InitializeComponent(void)
		{
			sExecsFolder = gcnew String("");
			sExecsFolder = Directory::GetCurrentDirectory();

			System::ComponentModel::ComponentResourceManager^ resources = (gcnew System::ComponentModel::ComponentResourceManager(Form1::typeid));
			this->mzML_label = (gcnew System::Windows::Forms::Label());
			this->mzML_textBox = (gcnew System::Windows::Forms::TextBox());
			this->mzML_Browse = (gcnew System::Windows::Forms::Button());
			this->mzML_Add = (gcnew System::Windows::Forms::Button());
			this->mzML_Remove = (gcnew System::Windows::Forms::Button());
			this->mzML_RichtextBox = (gcnew System::Windows::Forms::TextBox());
			this->mzID_label = (gcnew System::Windows::Forms::Label());
			this->mzID_textBox = (gcnew System::Windows::Forms::TextBox());
			this->mzID_Browse = (gcnew System::Windows::Forms::Button());
			this->mzID_Add = (gcnew System::Windows::Forms::Button());
			this->mzID_Remove = (gcnew System::Windows::Forms::Button());
			this->mzID_RichtextBox = (gcnew System::Windows::Forms::TextBox());
			this->Start_button = (gcnew System::Windows::Forms::Button());
			this->About_button = (gcnew System::Windows::Forms::Button());
			this->comboBox1 = (gcnew System::Windows::Forms::ComboBox());
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->textBox1 = (gcnew System::Windows::Forms::TextBox());
			this->PeptideConsistency_Label = (gcnew System::Windows::Forms::Label());
			this->PeptideConsistency_textBox = (gcnew System::Windows::Forms::TextBox());
			this->MS1_Label = (gcnew System::Windows::Forms::Label());
			this->MS1_comboBox = (gcnew System::Windows::Forms::ComboBox());
			this->PepScore_Label = (gcnew System::Windows::Forms::Label());
			this->textBox_PepScore = (gcnew System::Windows::Forms::TextBox());
			this->T0_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE0 = (gcnew System::Windows::Forms::TextBox());
			this->BWE0_label = (gcnew System::Windows::Forms::Label());
			this->textBox2 = (gcnew System::Windows::Forms::TextBox());
			this->T1_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE1 = (gcnew System::Windows::Forms::TextBox());
			this->BWE1_label = (gcnew System::Windows::Forms::Label());
			this->textBox3 = (gcnew System::Windows::Forms::TextBox());
			this->T2_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE2 = (gcnew System::Windows::Forms::TextBox());
			this->BWE2_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE3 = (gcnew System::Windows::Forms::TextBox());
			this->BWE3_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE4 = (gcnew System::Windows::Forms::TextBox());
			this->BWE4_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE5 = (gcnew System::Windows::Forms::TextBox());
			this->BWE5_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE6 = (gcnew System::Windows::Forms::TextBox());
			this->BWE6_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE7 = (gcnew System::Windows::Forms::TextBox());
			this->BWE7_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE8 = (gcnew System::Windows::Forms::TextBox());
			this->BWE8_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE9 = (gcnew System::Windows::Forms::TextBox());
			this->BWE9_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE10 = (gcnew System::Windows::Forms::TextBox());
			this->BWE10_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE11 = (gcnew System::Windows::Forms::TextBox());
			this->BWE11_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE12 = (gcnew System::Windows::Forms::TextBox());
			this->BWE12_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE13 = (gcnew System::Windows::Forms::TextBox());
			this->BWE13_label = (gcnew System::Windows::Forms::Label());
			this->textBox_BWE14 = (gcnew System::Windows::Forms::TextBox());
			this->BWE14_label = (gcnew System::Windows::Forms::Label());
			this->textBox4 = (gcnew System::Windows::Forms::TextBox());
			this->T3_label = (gcnew System::Windows::Forms::Label());
			this->textBox5 = (gcnew System::Windows::Forms::TextBox());
			this->T4_label = (gcnew System::Windows::Forms::Label());
			this->T14_label = (gcnew System::Windows::Forms::Label());
			this->textBox6 = (gcnew System::Windows::Forms::TextBox());
			this->T13_label = (gcnew System::Windows::Forms::Label());
			this->textBox7 = (gcnew System::Windows::Forms::TextBox());
			this->T12_label = (gcnew System::Windows::Forms::Label());
			this->textBox8 = (gcnew System::Windows::Forms::TextBox());
			this->T11_label = (gcnew System::Windows::Forms::Label());
			this->textBox9 = (gcnew System::Windows::Forms::TextBox());
			this->T10_label = (gcnew System::Windows::Forms::Label());
			this->textBox10 = (gcnew System::Windows::Forms::TextBox());
			this->textBox11 = (gcnew System::Windows::Forms::TextBox());
			this->T5_label = (gcnew System::Windows::Forms::Label());
			this->textBox12 = (gcnew System::Windows::Forms::TextBox());
			this->T6_label = (gcnew System::Windows::Forms::Label());
			this->textBox13 = (gcnew System::Windows::Forms::TextBox());
			this->T7_label = (gcnew System::Windows::Forms::Label());
			this->textBox14 = (gcnew System::Windows::Forms::TextBox());
			this->T8_label = (gcnew System::Windows::Forms::Label());
			this->textBox15 = (gcnew System::Windows::Forms::TextBox());
			this->T9_label = (gcnew System::Windows::Forms::Label());
			this->Rate_Constant_label = (gcnew System::Windows::Forms::Label());
			this->Rate_Constant_comboBox2 = (gcnew System::Windows::Forms::ComboBox());
			this->Mass_accuracy_label = (gcnew System::Windows::Forms::Label());
			this->Mass_Accuracy_textBox = (gcnew System::Windows::Forms::TextBox());
			this->enrichment_label = (gcnew System::Windows::Forms::Label());
			this->Information_label = (gcnew System::Windows::Forms::Label());
			this->Output_label = (gcnew System::Windows::Forms::Label());
			this->Output_textBox = (gcnew System::Windows::Forms::TextBox());
			this->Output_browse_button = (gcnew System::Windows::Forms::Button());
			this->Output_visualize_button = (gcnew System::Windows::Forms::Button());
			this->Enrichment_comboBox = (gcnew System::Windows::Forms::ComboBox());
			this->Elution_label = (gcnew System::Windows::Forms::Label());
			this->Elution_textBox = (gcnew System::Windows::Forms::TextBox());
			this->SuspendLayout();
			//

			if (true)
			{
				// 
				// Information_label
				// 
				this->Information_label->Anchor = System::Windows::Forms::AnchorStyles::None;
				this->Information_label->AutoSize = true;
				this->Information_label->Location = System::Drawing::Point(200, 5);
				this->Information_label->Name = L"Information_label";
				this->Information_label->Size = System::Drawing::Size(600, 13);
				this->Information_label->TabIndex = 69;

				//Information_label->Font = gcnew Font(;

				this->Information_label->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif",
					13, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point,
					static_cast<System::Byte>(0)));

				this->Information_label->Text = L"mzML and mzid files should be in matched pairs," +
					" in increasing label duration.\n" +
					"Enter labeling durations in T[0-14] and body water enrichment in BWE[0-14] boxes.";
			}
			// 
			// mzML_label
			// 
			this->mzML_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzML_label->AutoSize = true;
			this->mzML_label->Location = System::Drawing::Point(26, 67);
			this->mzML_label->Name = L"mzML_label";
			this->mzML_label->Size = System::Drawing::Size(54, 13);
			this->mzML_label->TabIndex = 0;
			this->mzML_label->Text = L"mzML File";
			this->mzML_label->Click += gcnew System::EventHandler(this, &Form1::label1_Click);
			// 
			// mzML_textBox
			// 
			this->mzML_textBox->AllowDrop = true;
			this->mzML_textBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzML_textBox->Location = System::Drawing::Point(101, 64);
			this->mzML_textBox->Multiline = true;
			this->mzML_textBox->Name = L"mzML_textBox";
			this->mzML_textBox->Size = System::Drawing::Size(283, 20);
			this->mzML_textBox->TabIndex = 1;
			// 
			// mzML_Browse
			// 
			this->mzML_Browse->AllowDrop = true;
			this->mzML_Browse->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzML_Browse->Location = System::Drawing::Point(402, 62);
			this->mzML_Browse->Name = L"mzML_Browse";
			this->mzML_Browse->Size = System::Drawing::Size(75, 23);
			this->mzML_Browse->TabIndex = 2;
			this->mzML_Browse->Text = L"Browse";
			this->mzML_Browse->UseVisualStyleBackColor = true;
			this->mzML_Browse->Click += gcnew System::EventHandler(this, &Form1::mzML_Browse_Click);
			// 
			// mzML_Add
			// 
			this->mzML_Add->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzML_Add->Location = System::Drawing::Point(101, 101);
			this->mzML_Add->Name = L"mzML_Add";
			this->mzML_Add->Size = System::Drawing::Size(75, 23);
			this->mzML_Add->TabIndex = 3;
			this->mzML_Add->Text = L"Add";
			this->mzML_Add->UseVisualStyleBackColor = true;
			this->mzML_Add->Click += gcnew System::EventHandler(this, &Form1::mzML_Add_Click);
			// 
			// mzML_Remove
			// 
			this->mzML_Remove->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzML_Remove->Location = System::Drawing::Point(309, 101);
			this->mzML_Remove->Name = L"mzML_Remove";
			this->mzML_Remove->Size = System::Drawing::Size(75, 23);
			this->mzML_Remove->TabIndex = 4;
			this->mzML_Remove->Text = L"Remove";
			this->mzML_Remove->UseVisualStyleBackColor = true;
			this->mzML_Remove->Click += gcnew System::EventHandler(this, &Form1::mzML_Remove_Click);
			// 
			// mzML_RichtextBox
			// 
			this->mzML_RichtextBox->AllowDrop = true;
			this->mzML_RichtextBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzML_RichtextBox->Location = System::Drawing::Point(105, 139);
			this->mzML_RichtextBox->Multiline = true;
			this->mzML_RichtextBox->Name = L"mzML_RichtextBox";
			this->mzML_RichtextBox->Size = System::Drawing::Size(278, 140);
			this->mzML_RichtextBox->TabIndex = 5;
			this->mzML_RichtextBox->TextChanged += gcnew System::EventHandler(this, &Form1::mzMLtextBox_TextChanged);
			this->mzML_RichtextBox->DragDrop += gcnew System::Windows::Forms::DragEventHandler(this, &Form1::mzML_RichtextBox_DragDrop);
			this->mzML_RichtextBox->DragEnter += gcnew System::Windows::Forms::DragEventHandler(this, &Form1::mzML_RichtextBox_DragEnter);
			// 
			// mzID_label
			// 
			this->mzID_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzID_label->AutoSize = true;
			this->mzID_label->Location = System::Drawing::Point(578, 67);
			this->mzID_label->Name = L"mzID_label";
			this->mzID_label->Size = System::Drawing::Size(50, 13);
			this->mzID_label->TabIndex = 6;
			this->mzID_label->Text = L"mzID File";
			// 
			// mzID_textBox
			// 
			this->mzID_textBox->AllowDrop = true;
			this->mzID_textBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzID_textBox->Location = System::Drawing::Point(652, 65);
			this->mzID_textBox->Multiline = true;
			this->mzID_textBox->Name = L"mzID_textBox";
			this->mzID_textBox->Size = System::Drawing::Size(283, 20);
			this->mzID_textBox->TabIndex = 7;
			// 
			// mzID_Browse
			// 
			this->mzID_Browse->AllowDrop = true;
			this->mzID_Browse->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzID_Browse->Location = System::Drawing::Point(953, 61);
			this->mzID_Browse->Name = L"mzID_Browse";
			this->mzID_Browse->Size = System::Drawing::Size(75, 23);
			this->mzID_Browse->TabIndex = 8;
			this->mzID_Browse->Text = L"Browse";
			this->mzID_Browse->UseVisualStyleBackColor = true;
			this->mzID_Browse->Click += gcnew System::EventHandler(this, &Form1::mzID_Browse_Click);
			// 
			// mzID_Add
			// 
			this->mzID_Add->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzID_Add->Location = System::Drawing::Point(653, 101);
			this->mzID_Add->Name = L"mzID_Add";
			this->mzID_Add->Size = System::Drawing::Size(75, 23);
			this->mzID_Add->TabIndex = 9;
			this->mzID_Add->Text = L"Add";
			this->mzID_Add->UseVisualStyleBackColor = true;
			this->mzID_Add->Click += gcnew System::EventHandler(this, &Form1::mzID_Add_Click);
			// 
			// mzID_Remove
			// 
			this->mzID_Remove->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzID_Remove->Location = System::Drawing::Point(859, 101);
			this->mzID_Remove->Name = L"mzID_Remove";
			this->mzID_Remove->Size = System::Drawing::Size(75, 23);
			this->mzID_Remove->TabIndex = 10;
			this->mzID_Remove->Text = L"Remove";
			this->mzID_Remove->UseVisualStyleBackColor = true;
			this->mzID_Remove->Click += gcnew System::EventHandler(this, &Form1::mzID_Remove_Click);
			// 
			// mzID_RichtextBox
			// 
			this->mzID_RichtextBox->AllowDrop = true;
			this->mzID_RichtextBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzID_RichtextBox->Location = System::Drawing::Point(657, 139);
			this->mzID_RichtextBox->Multiline = true;
			this->mzID_RichtextBox->Name = L"mzID_RichtextBox";
			this->mzID_RichtextBox->Size = System::Drawing::Size(278, 140);
			this->mzID_RichtextBox->TabIndex = 11;
			this->mzID_RichtextBox->DragDrop += gcnew System::Windows::Forms::DragEventHandler(this, &Form1::mzID_RichtextBox_DragDrop);
			this->mzID_RichtextBox->DragEnter += gcnew System::Windows::Forms::DragEventHandler(this, &Form1::mzID_RichtextBox_DragEnter);
			// 
			// Start_button
			// 
			this->Start_button->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Start_button->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point,
				static_cast<System::Byte>(0)));
			this->Start_button->Location = System::Drawing::Point(402, 576);
			this->Start_button->Name = L"Start_button";
			this->Start_button->Size = System::Drawing::Size(203, 56);
			this->Start_button->TabIndex = 18;
			this->Start_button->Text = L"Start";
			this->Start_button->UseVisualStyleBackColor = true;
			this->Start_button->Click += gcnew System::EventHandler(this, &Form1::Start_button_Click);
			// 
			// About_button
			// 
			this->About_button->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->About_button->ImageAlign = System::Drawing::ContentAlignment::BottomLeft;
			this->About_button->Location = System::Drawing::Point(700, 552);
			this->About_button->Name = L"About_button";
			this->About_button->Size = System::Drawing::Size(215, 79);
			this->About_button->TabIndex = 19;
			this->About_button->Text = L"About Heavy Water ";
			this->About_button->UseVisualStyleBackColor = true;
			this->About_button->Click += gcnew System::EventHandler(this, &Form1::button1_Click);
			// 
			// comboBox1
			// 
			this->comboBox1->AllowDrop = true;
			this->comboBox1->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->comboBox1->DropDownStyle = System::Windows::Forms::ComboBoxStyle::DropDownList;
			this->comboBox1->Items->AddRange(gcnew cli::array< System::Object^  >(2) { L"Day", L"Hour" });
			this->comboBox1->Location = System::Drawing::Point(294, 398);
			this->comboBox1->MaxDropDownItems = 2;
			this->comboBox1->Name = L"comboBox1";
			this->comboBox1->Size = System::Drawing::Size(47, 21);
			this->comboBox1->TabIndex = 20;
			// 
			// label1
			// 
			this->label1->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->label1->AutoSize = true;
			this->label1->Location = System::Drawing::Point(167, 401);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(121, 13);
			this->label1->TabIndex = 21;
			this->label1->Text = L"Time Course Information";
			// 
			//
			// 
			// PeptideConsistency_label
			// 
			this->PeptideConsistency_Label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->PeptideConsistency_Label->AutoSize = true;
			this->PeptideConsistency_Label->Location = System::Drawing::Point(x_70 - 28, 388);
			this->PeptideConsistency_Label->Name = L"PeptideConsistency_Label";
			this->PeptideConsistency_Label->Size = System::Drawing::Size(200, 13);
			this->PeptideConsistency_Label->TabIndex = 27;
			this->PeptideConsistency_Label->Text = L"PeptideConsistency";
			//
			// PeptideConsistency_textBox
			// 
			this->PeptideConsistency_textBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->PeptideConsistency_textBox->Location = System::Drawing::Point(x_70 + 80, 386);
			this->PeptideConsistency_textBox->Name = L"PeptideConsistency_textBox";
			this->PeptideConsistency_textBox->Size = System::Drawing::Size(30, 10);
			this->PeptideConsistency_textBox->TabIndex = 26;
			this->PeptideConsistency_textBox->Text = L"4";
			// 
			// MS1_label
			// 
			this->MS1_Label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->MS1_Label->AutoSize = true;
			this->MS1_Label->Location = System::Drawing::Point(220, 388);
			this->MS1_Label->Name = L"MS1_Mode_Label";
			this->MS1_Label->Size = System::Drawing::Size(100, 13);
			this->MS1_Label->TabIndex = 27;
			this->MS1_Label->Text = L"MS1 Data ";
			//
			// MS1_combobox
			// 
			this->MS1_comboBox->AllowDrop = true;
			this->MS1_comboBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->MS1_comboBox->DropDownStyle = System::Windows::Forms::ComboBoxStyle::DropDownList;
			this->MS1_comboBox->FormattingEnabled = true;
			this->MS1_comboBox->Items->AddRange(gcnew cli::array< System::Object^  >(2) { L"Centroid", L"Profile" });
			this->MS1_comboBox->MaxDropDownItems = 2;
			//this->MS1_comboBox->Items->AddRange(gcnew cli::array< System::Object^  >(1) { L"Profile"});
			this->MS1_comboBox->Location = System::Drawing::Point(280, 386);
			this->MS1_comboBox->Name = L"MS1_Mode";
			this->MS1_comboBox->Size = System::Drawing::Size(80, 21);
			this->MS1_comboBox->TabIndex = 63;
			this->MS1_comboBox->SelectedIndexChanged += gcnew System::EventHandler(this, &Form1::MS1_comboBox_SelectedIndexChanged);
			this->MS1_comboBox->Text = L"Centroid";
			//
			// PepScore_label
			// 
			this->PepScore_Label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->PepScore_Label->AutoSize = true;
			this->PepScore_Label->Location = System::Drawing::Point(400, 388);
			this->PepScore_Label->Name = L"PepScore_Label";
			this->PepScore_Label->Size = System::Drawing::Size(100, 13);
			this->PepScore_Label->TabIndex = 27;
			this->PepScore_Label->Text = L"Peptide Score";
			//
			// textBox_PepScore
			//
			this->textBox_PepScore->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_PepScore->Location = System::Drawing::Point(480, 386);
			this->textBox_PepScore->Name = L"PeptideConsistency_textBox";
			this->textBox_PepScore->Size = System::Drawing::Size(35, 10);
			this->textBox_PepScore->TabIndex = 26;
			this->textBox_PepScore->Text = L"20";
			//
			// T0_label
			// 
			this->T0_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T0_label->AutoSize = true;
			this->T0_label->Location = System::Drawing::Point(x_70 - 28, 432);
			this->T0_label->Name = L"T0_label";
			this->T0_label->Size = System::Drawing::Size(20, 13);
			this->T0_label->TabIndex = 27;
			this->T0_label->Text = L"T0";
			//
			// textBox1
			// 
			this->textBox1->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox1->Location = System::Drawing::Point(x_70, 430);
			this->textBox1->Name = L"textBox1";
			this->textBox1->Size = System::Drawing::Size(37, 20);
			this->textBox1->TabIndex = 26;
			// 
			//
			//textBox_BWE0
			//
			this->textBox_BWE0->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE0->Location = System::Drawing::Point(x_130 + 45, 430);
			this->textBox_BWE0->Name = L"testBox_BWE0";
			this->textBox_BWE0->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE0->TabIndex = 26;
			//
			// BWE0_label
			//
			this->BWE0_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE0_label->AutoSize = true;
			this->BWE0_label->Location = System::Drawing::Point(x_130, 432);
			this->BWE0_label->Name = L"BWE0_label";
			this->BWE0_label->Size = System::Drawing::Size(20, 13);
			this->BWE0_label->TabIndex = 27;
			this->BWE0_label->Text = L"BWE0";
			// 
			// T1_label
			// 
			this->T1_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T1_label->AutoSize = true;
			this->T1_label->Location = System::Drawing::Point(x_70 - 28, 461);
			this->T1_label->Name = L"T1_label";
			this->T1_label->Size = System::Drawing::Size(20, 13);
			this->T1_label->TabIndex = 29;
			this->T1_label->Text = L"T1";
			//
			// 
			// textBox2
			// 
			this->textBox2->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox2->Location = System::Drawing::Point(x_70, 458);
			this->textBox2->Name = L"textBox2";
			this->textBox2->Size = System::Drawing::Size(37, 20);
			this->textBox2->TabIndex = 28;
			//
			//textBox_BWE1
			//
			this->textBox_BWE1->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE1->Location = System::Drawing::Point(x_130 + 45, 458);
			this->textBox_BWE1->Name = L"testBox_BWE1";
			this->textBox_BWE1->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE1->TabIndex = 26;
			//
			// BWE1_label
			//
			this->BWE1_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE1_label->AutoSize = true;
			this->BWE1_label->Location = System::Drawing::Point(x_130, 461);
			this->BWE1_label->Name = L"BWE1_label";
			this->BWE1_label->Size = System::Drawing::Size(20, 13);
			this->BWE1_label->TabIndex = 27;
			this->BWE1_label->Text = L"BWE1";
			// 
			// 
			// T2_label
			// 
			this->T2_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T2_label->AutoSize = true;
			this->T2_label->Location = System::Drawing::Point(x_70 - 28, 488);
			this->T2_label->Name = L"T2_label";
			this->T2_label->Size = System::Drawing::Size(20, 13);
			this->T2_label->TabIndex = 31;
			this->T2_label->Text = L"T2";
			//
			// textBox3
			// 
			this->textBox3->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox3->Location = System::Drawing::Point(x_70, 485);
			this->textBox3->Name = L"textBox3";
			this->textBox3->Size = System::Drawing::Size(37, 20);
			this->textBox3->TabIndex = 30;
			//
			//textBox_BWE2
			//
			this->textBox_BWE2->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE2->Location = System::Drawing::Point(x_130 + 45, 483);
			this->textBox_BWE2->Name = L"testBox_BWE2";
			this->textBox_BWE2->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE2->TabIndex = 26;
			//
			// BWE2_label
			//
			this->BWE2_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE2_label->AutoSize = true;
			this->BWE2_label->Location = System::Drawing::Point(x_130, 486);
			this->BWE2_label->Name = L"BWE2_label";
			this->BWE2_label->Size = System::Drawing::Size(20, 13);
			this->BWE2_label->TabIndex = 27;
			this->BWE2_label->Text = L"BWE2";
			// 
			// T3_label
			// 
			this->T3_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T3_label->AutoSize = true;
			this->T3_label->Location = System::Drawing::Point(x_70 - 28, 515);
			this->T3_label->Name = L"T3_label";
			this->T3_label->Size = System::Drawing::Size(20, 13);
			this->T3_label->TabIndex = 33;
			this->T3_label->Text = L"T3";
			// 
			// textBox4
			// 
			this->textBox4->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox4->Location = System::Drawing::Point(x_70, 512);
			this->textBox4->Name = L"textBox4";
			this->textBox4->Size = System::Drawing::Size(37, 20);
			this->textBox4->TabIndex = 32;
			//
			//textBox_BWE3
			//
			this->textBox_BWE3->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE3->Location = System::Drawing::Point(x_130 + 45, 512);
			this->textBox_BWE3->Name = L"textBox_BWE3";
			this->textBox_BWE3->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE3->TabIndex = 26;
			//
			// BWE3_label
			//
			this->BWE3_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE3_label->AutoSize = true;
			this->BWE3_label->Location = System::Drawing::Point(x_130, 515);
			this->BWE3_label->Name = L"BWE3_label";
			this->BWE3_label->Size = System::Drawing::Size(20, 13);
			this->BWE3_label->TabIndex = 27;
			this->BWE3_label->Text = L"BWE3";
			// 
			// T4_label
			// 
			this->T4_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T4_label->AutoSize = true;
			this->T4_label->Location = System::Drawing::Point(x_70 - 28, 542);
			this->T4_label->Name = L"T4_label";
			this->T4_label->Size = System::Drawing::Size(20, 13);
			this->T4_label->TabIndex = 35;
			this->T4_label->Text = L"T4";
			//
			// textBox5
			// 
			this->textBox5->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox5->Location = System::Drawing::Point(x_70, 539);
			this->textBox5->Name = L"textBox5";
			this->textBox5->Size = System::Drawing::Size(37, 20);
			this->textBox5->TabIndex = 34;
			//
			//textBox_BWE4
			//
			this->textBox_BWE4->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE4->Location = System::Drawing::Point(x_130 + 45, 539);
			this->textBox_BWE4->Name = L"textBox_BWE4";
			this->textBox_BWE4->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE4->TabIndex = 26;
			//
			// BWE4_label
			//
			this->BWE4_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE4_label->AutoSize = true;
			this->BWE4_label->Location = System::Drawing::Point(x_130, 542);
			this->BWE4_label->Name = L"BWE4_label";
			this->BWE4_label->Size = System::Drawing::Size(20, 13);
			this->BWE4_label->TabIndex = 27;
			this->BWE4_label->Text = L"BWE4";
			// 
			// T5_label
			// 
			this->T5_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T5_label->AutoSize = true;
			this->T5_label->Location = System::Drawing::Point(x_250, 431);
			this->T5_label->Name = L"T5_label";
			this->T5_label->Size = System::Drawing::Size(20, 13);
			this->T5_label->TabIndex = 42;
			this->T5_label->Text = L"T5";
			//
			// 
			// textBox6
			// 
			this->textBox6->Anchor = System::Windows::Forms::AnchorStyles::None;
			//			this->textBox6->Location = System::Drawing::Point(400, 538);
			//this->textBox6->Location = System::Drawing::Point(x_250, 434);
			this->textBox6->Location = System::Drawing::Point(x_250 + 28, 428);
			this->textBox6->Name = L"textBox6";
			this->textBox6->Size = System::Drawing::Size(37, 20);
			this->textBox6->TabIndex = 49;
			// 
			//
			//textBox_BWE5
			//
			this->textBox_BWE5->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE5->Location = System::Drawing::Point(380, 429);
			this->textBox_BWE5->Name = L"textBox_BWE5";
			this->textBox_BWE5->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE5->TabIndex = 26;
			//
			// BWE5_label
			//
			this->BWE5_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE5_label->AutoSize = true;
			this->BWE5_label->Location = System::Drawing::Point(335, 431);
			this->BWE5_label->Name = L"BWE5_label";
			this->BWE5_label->Size = System::Drawing::Size(20, 13);
			this->BWE5_label->TabIndex = 27;
			this->BWE5_label->Text = L"BWE5";
			// 
			// 
			// T6_label
			// 
			this->T6_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T6_label->AutoSize = true;
			this->T6_label->Location = System::Drawing::Point(x_250, 460);
			this->T6_label->Name = L"T6_label";
			this->T6_label->Size = System::Drawing::Size(20, 13);
			this->T6_label->TabIndex = 44;
			this->T6_label->Text = L"T6";
			// 
			// 
			// textBox7
			// 
			this->textBox7->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox7->Location = System::Drawing::Point(x_250 + 28, 457);
			this->textBox7->Name = L"textBox7";
			this->textBox7->Size = System::Drawing::Size(37, 20);
			this->textBox7->TabIndex = 47;
			//
			//
			//textBox_BWE6
			//
			this->textBox_BWE6->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE6->Location = System::Drawing::Point(380, 456);
			this->textBox_BWE6->Name = L"textBox_BWE6";
			this->textBox_BWE6->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE6->TabIndex = 26;
			//
			// BWE6_label
			//
			this->BWE6_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE6_label->AutoSize = true;
			this->BWE6_label->Location = System::Drawing::Point(335, 457);
			this->BWE6_label->Name = L"BWE6_label";
			this->BWE6_label->Size = System::Drawing::Size(20, 13);
			this->BWE6_label->TabIndex = 27;
			this->BWE6_label->Text = L"BWE6";
			// 
			// 
			// T7_label
			// 
			this->T7_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T7_label->AutoSize = true;
			this->T7_label->Location = System::Drawing::Point(x_250, 487);
			this->T7_label->Name = L"T7_label";
			this->T7_label->Size = System::Drawing::Size(20, 13);
			this->T7_label->TabIndex = 46;
			this->T7_label->Text = L"T7";
			//
			// 
			// textBox8
			// 
			this->textBox8->Anchor = System::Windows::Forms::AnchorStyles::None;
			//this->textBox8->Location = System::Drawing::Point(484, 484);
			this->textBox8->Location = System::Drawing::Point(x_250 + 28, 484);
			this->textBox8->Name = L"textBox8";
			this->textBox8->Size = System::Drawing::Size(37, 20);
			this->textBox8->TabIndex = 45;
			//
			//
			//textBox_BWE7
			//
			this->textBox_BWE7->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE7->Location = System::Drawing::Point(380, 483);
			this->textBox_BWE7->Name = L"textBox_BWE7";
			this->textBox_BWE7->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE7->TabIndex = 26;
			//
			// BWE7_label
			//
			this->BWE7_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE7_label->AutoSize = true;
			this->BWE7_label->Location = System::Drawing::Point(335, 484);
			this->BWE7_label->Name = L"BWE7_label";
			this->BWE7_label->Size = System::Drawing::Size(20, 13);
			this->BWE7_label->TabIndex = 27;
			this->BWE7_label->Text = L"BWE7";
			// 
			// T8_label
			// 
			this->T8_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T8_label->AutoSize = true;
			this->T8_label->Location = System::Drawing::Point(x_250, 514);
			this->T8_label->Name = L"T8_label";
			this->T8_label->Size = System::Drawing::Size(20, 13);
			this->T8_label->TabIndex = 48;
			this->T8_label->Text = L"T8";
			// 
			// 
			// textBox9
			// 
			this->textBox9->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox9->Location = System::Drawing::Point(x_250 + 28, 511);
			this->textBox9->Name = L"textBox9";
			this->textBox9->Size = System::Drawing::Size(37, 20);
			this->textBox9->TabIndex = 43;
			//
			//textBox_BWE8
			//
			this->textBox_BWE8->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE8->Location = System::Drawing::Point(380, 510);
			this->textBox_BWE8->Name = L"textBox_BWE8";
			this->textBox_BWE8->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE8->TabIndex = 26;
			//
			// BWE8_label
			//
			this->BWE8_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE8_label->AutoSize = true;
			this->BWE8_label->Location = System::Drawing::Point(335, 511);
			this->BWE8_label->Name = L"BWE8_label";
			this->BWE8_label->Size = System::Drawing::Size(20, 13);
			this->BWE8_label->TabIndex = 27;
			this->BWE8_label->Text = L"BWE8";
			// 
			// 
			// T9_label
			// 
			this->T9_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T9_label->AutoSize = true;
			this->T9_label->Location = System::Drawing::Point(x_250, 541);
			this->T9_label->Name = L"T9_label";
			this->T9_label->Size = System::Drawing::Size(20, 13);
			this->T9_label->TabIndex = 50;
			this->T9_label->Text = L"T9";
			// 
			// textBox10
			// 
			this->textBox10->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox10->Location = System::Drawing::Point(x_250 + 28, 538);
			this->textBox10->Name = L"textBox10";
			this->textBox10->Size = System::Drawing::Size(37, 20);
			this->textBox10->TabIndex = 41;
			//
			//textBox_BWE9
			//
			this->textBox_BWE9->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE9->Location = System::Drawing::Point(380, 537);
			this->textBox_BWE9->Name = L"textBox_BWE9";
			this->textBox_BWE9->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE9->TabIndex = 26;
			//
			// BWE9_label
			//
			this->BWE9_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE9_label->AutoSize = true;
			this->BWE9_label->Location = System::Drawing::Point(335, 538);
			this->BWE9_label->Name = L"BWE9_label";
			this->BWE9_label->Size = System::Drawing::Size(20, 13);
			this->BWE9_label->TabIndex = 27;
			this->BWE9_label->Text = L"BWE9";
			// 
			//
			// T10_label
			// 
			this->T10_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T10_label->AutoSize = true;
			this->T10_label->Location = System::Drawing::Point(x_460, 431);
			this->T10_label->Name = L"T10_label";
			this->T10_label->Size = System::Drawing::Size(26, 13);
			this->T10_label->TabIndex = 42;
			this->T10_label->Text = L"T10";
			//
			// textBox11
			// 
			this->textBox11->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox11->Location = System::Drawing::Point(x_460 + 28, 429);
			this->textBox11->Name = L"textBox11";
			this->textBox11->Size = System::Drawing::Size(37, 20);
			this->textBox11->TabIndex = 41;
			// 
			//textBox_BWE10
			//
			this->textBox_BWE10->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE10->Location = System::Drawing::Point(590, 428);
			this->textBox_BWE10->Name = L"textBox_BWE10";
			this->textBox_BWE10->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE10->TabIndex = 26;
			//
			// BWE10_label
			//
			this->BWE10_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE10_label->AutoSize = true;
			this->BWE10_label->Location = System::Drawing::Point(545, 430);
			this->BWE10_label->Name = L"BWE10_label";
			this->BWE10_label->Size = System::Drawing::Size(20, 13);
			this->BWE10_label->TabIndex = 27;
			this->BWE10_label->Text = L"BWE10";
			//
			// T11_label
			// 
			this->T11_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T11_label->AutoSize = true;
			this->T11_label->Location = System::Drawing::Point(x_460, 460);
			this->T11_label->Name = L"T11_label";
			this->T11_label->Size = System::Drawing::Size(26, 13);
			this->T11_label->TabIndex = 44;
			this->T11_label->Text = L"T11";
			// 
			// textBox12
			// 
			this->textBox12->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox12->Location = System::Drawing::Point(x_460 + 28, 457);
			this->textBox12->Name = L"textBox12";
			this->textBox12->Size = System::Drawing::Size(37, 20);
			this->textBox12->TabIndex = 43;
			//
			//textBox_BWE11
			//
			this->textBox_BWE11->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE11->Location = System::Drawing::Point(590, 456);
			this->textBox_BWE11->Name = L"textBox_BWE11";
			this->textBox_BWE11->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE11->TabIndex = 26;
			//
			// BWE11_label
			//
			this->BWE11_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE11_label->AutoSize = true;
			this->BWE11_label->Location = System::Drawing::Point(545, 459);
			this->BWE11_label->Name = L"BWE11_label";
			this->BWE11_label->Size = System::Drawing::Size(20, 13);
			this->BWE11_label->TabIndex = 27;
			this->BWE11_label->Text = L"BWE11";
			//
			// 
			// T12_label
			// 
			this->T12_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T12_label->AutoSize = true;
			this->T12_label->Location = System::Drawing::Point(x_460, 487);
			this->T12_label->Name = L"T12_label";
			this->T12_label->Size = System::Drawing::Size(26, 13);
			this->T12_label->TabIndex = 46;
			this->T12_label->Text = L"T12";
			//
			// textBox13
			// 
			this->textBox13->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox13->Location = System::Drawing::Point(x_460 + 28, 484);
			this->textBox13->Name = L"textBox13";
			this->textBox13->Size = System::Drawing::Size(37, 20);
			this->textBox13->TabIndex = 45;
			//
			//
			//textBox_BWE12
			//
			this->textBox_BWE12->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE12->Location = System::Drawing::Point(590, 483);
			this->textBox_BWE12->Name = L"textBox_BWE12";
			this->textBox_BWE12->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE12->TabIndex = 26;
			//
			// BWE12_label
			//
			this->BWE12_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE12_label->AutoSize = true;
			this->BWE12_label->Location = System::Drawing::Point(545, 483);
			this->BWE12_label->Name = L"BWE12_label";
			this->BWE12_label->Size = System::Drawing::Size(20, 13);
			this->BWE12_label->TabIndex = 27;
			this->BWE12_label->Text = L"BWE12";
			//
			//
			// T13_label
			// 
			this->T13_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T13_label->AutoSize = true;
			this->T13_label->Location = System::Drawing::Point(x_460, 514);
			this->T13_label->Name = L"T13_label";
			this->T13_label->Size = System::Drawing::Size(26, 13);
			this->T13_label->TabIndex = 48;
			this->T13_label->Text = L"T13";
			//	
			// textBox14
			// 
			this->textBox14->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox14->Location = System::Drawing::Point(x_460 + 28, 511);
			this->textBox14->Name = L"textBox14";
			this->textBox14->Size = System::Drawing::Size(37, 20);
			this->textBox14->TabIndex = 47;
			// 
			//
			//textBox_BWE13
			//
			this->textBox_BWE13->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE13->Location = System::Drawing::Point(590, 510);
			this->textBox_BWE13->Name = L"textBox_BWE13";
			this->textBox_BWE13->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE13->TabIndex = 26;
			//
			// BWE13_label
			//
			this->BWE13_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE13_label->AutoSize = true;
			this->BWE13_label->Location = System::Drawing::Point(545, 513);
			this->BWE13_label->Name = L"BWE13_label";
			this->BWE13_label->Size = System::Drawing::Size(20, 13);
			this->BWE13_label->TabIndex = 27;
			this->BWE13_label->Text = L"BWE13";
			//
			//
			// 
			// T14_label
			// 
			this->T14_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T14_label->AutoSize = true;
			this->T14_label->Location = System::Drawing::Point(x_460, 541);
			this->T14_label->Name = L"T14_label";
			this->T14_label->Size = System::Drawing::Size(26, 13);
			this->T14_label->TabIndex = 50;
			this->T14_label->Text = L"T14";

			// textBox15
			// 
			this->textBox15->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox15->Location = System::Drawing::Point(x_460 + 28, 538);
			this->textBox15->Name = L"textBox15";
			this->textBox15->Size = System::Drawing::Size(37, 20);
			this->textBox15->TabIndex = 49;
			//
			//
			//textBox_BWE14
			//
			this->textBox_BWE14->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox_BWE14->Location = System::Drawing::Point(590, 537);
			this->textBox_BWE14->Name = L"textBox_BWE14";
			this->textBox_BWE14->Size = System::Drawing::Size(37, 20);
			this->textBox_BWE14->TabIndex = 26;
			//
			// BWE14_label
			//
			this->BWE14_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->BWE14_label->AutoSize = true;
			this->BWE14_label->Location = System::Drawing::Point(545, 540);
			this->BWE14_label->Name = L"BWE14_label";
			this->BWE14_label->Size = System::Drawing::Size(20, 13);
			this->BWE14_label->TabIndex = 27;
			this->BWE14_label->Text = L"BWE14";
			//
			//
			// 
			// Rate_Constant_label
			// 
			this->Rate_Constant_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Rate_Constant_label->AutoSize = true;
			this->Rate_Constant_label->Location = System::Drawing::Point(730, 379);
			this->Rate_Constant_label->Name = L"Rate_Constant_label";
			this->Rate_Constant_label->Size = System::Drawing::Size(114, 13);
			this->Rate_Constant_label->TabIndex = 54;
			this->Rate_Constant_label->Text = L"Rate Constant Method";
			// 
			// Rate_Constant_comboBox2
			// 
			this->Rate_Constant_comboBox2->AllowDrop = true;
			this->Rate_Constant_comboBox2->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Rate_Constant_comboBox2->DropDownStyle = System::Windows::Forms::ComboBoxStyle::DropDownList;
			this->Rate_Constant_comboBox2->FormattingEnabled = true;
			this->Rate_Constant_comboBox2->Items->AddRange(gcnew cli::array< System::Object^  >(2) { L"One Parameter", L"Two Parameter" });
			this->Rate_Constant_comboBox2->Location = System::Drawing::Point(717, 412);
			this->Rate_Constant_comboBox2->Name = L"Rate_Constant_comboBox2";
			this->Rate_Constant_comboBox2->Size = System::Drawing::Size(162, 21);
			this->Rate_Constant_comboBox2->TabIndex = 63;
			this->Rate_Constant_comboBox2->Text = L"One Parameter";
			this->Rate_Constant_comboBox2->SelectedIndexChanged += gcnew System::EventHandler(this, &Form1::Rate_Constant_comboBox2_SelectedIndexChanged);
			// 
			// Mass_accuracy_label
			// 
			this->Mass_accuracy_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Mass_accuracy_label->AutoSize = true;
			this->Mass_accuracy_label->Location = System::Drawing::Point(146, 337);
			this->Mass_accuracy_label->Name = L"Mass_accuracy_label";
			this->Mass_accuracy_label->Size = System::Drawing::Size(112, 13);
			this->Mass_accuracy_label->TabIndex = 64;
			this->Mass_accuracy_label->Text = L"Mass Accuracy (PPM)";
			// 
			// Mass_Accuracy_textBox
			// 
			this->Mass_Accuracy_textBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Mass_Accuracy_textBox->Location = System::Drawing::Point(274, 334);
			this->Mass_Accuracy_textBox->Name = L"Mass_Accuracy_textBox";
			this->Mass_Accuracy_textBox->Size = System::Drawing::Size(41, 20);
			this->Mass_Accuracy_textBox->TabIndex = 65;
			this->Mass_Accuracy_textBox->Text = L"20.0";
			// 
			// enrichment_label
			// 
			this->enrichment_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->enrichment_label->AutoSize = true;
			this->enrichment_label->Location = System::Drawing::Point(39, 330);
			this->enrichment_label->Name = L"enrichment_label";
			this->enrichment_label->Size = System::Drawing::Size(60, 13);
			this->enrichment_label->TabIndex = 69;
			this->enrichment_label->Text = L"Enrichment";
			// 
			// Output_label
			// 
			this->Output_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Output_label->AutoSize = true;
			this->Output_label->Location = System::Drawing::Point(567, 336);
			this->Output_label->Name = L"Output_label";
			this->Output_label->Size = System::Drawing::Size(84, 13);
			this->Output_label->TabIndex = 70;
			this->Output_label->Text = L"Output Directory";
			// 
			// Output_textBox
			// 
			this->Output_textBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Output_textBox->Location = System::Drawing::Point(657, 333);
			this->Output_textBox->Name = L"Output_textBox";
			this->Output_textBox->Size = System::Drawing::Size(283, 20);
			this->Output_textBox->TabIndex = 71;
			// 
			// Output_browse_button
			// 
			this->Output_browse_button->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Output_browse_button->Location = System::Drawing::Point(953, 330);
			this->Output_browse_button->Name = L"Output_browse_button";
			this->Output_browse_button->Size = System::Drawing::Size(75, 23);
			this->Output_browse_button->TabIndex = 72;
			this->Output_browse_button->Text = L"Browse";
			this->Output_browse_button->UseVisualStyleBackColor = true;
			this->Output_browse_button->Click += gcnew System::EventHandler(this, &Form1::Output_browse_button_Click);

			// Output_visualize_button
			// 
			this->Output_visualize_button->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Output_visualize_button->Location = System::Drawing::Point(700, 462);
			this->Output_visualize_button->Name = L"Output_visualize_button";
			this->Output_visualize_button->Size = System::Drawing::Size(215, 79);
			this->Output_visualize_button->TabIndex = 72;
			this->Output_visualize_button->Text = L"Visualization";
			this->Output_visualize_button->UseVisualStyleBackColor = true;
			this->Output_visualize_button->BackColor = Color::CadetBlue;
			this->Output_visualize_button->Click += gcnew System::EventHandler(this, &Form1::Output_visualize_button_Click);
			// 

			// 
			// Enrichment_comboBox
			// 
			this->Enrichment_comboBox->AllowDrop = true;
			this->Enrichment_comboBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Enrichment_comboBox->DropDownStyle = System::Windows::Forms::ComboBoxStyle::DropDownList;
			this->Enrichment_comboBox->FormattingEnabled = true;
			//			this->Enrichment_comboBox->Items->AddRange(gcnew cli::array< System::Object^  >(4) {L"2H", L"15N", L"13C", L"18O"});
			this->Enrichment_comboBox->Items->AddRange(gcnew cli::array< System::Object^  >(1) { L"2H" });
			this->Enrichment_comboBox->Location = System::Drawing::Point(39, 346);
			this->Enrichment_comboBox->Name = L"Enrichment_comboBox";
			this->Enrichment_comboBox->Size = System::Drawing::Size(70, 21);
			this->Enrichment_comboBox->TabIndex = 74;
			this->Enrichment_comboBox->Text = L"2H";
			// 
			// Elution_label
			// 
			this->Elution_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Elution_label->AutoSize = true;
			this->Elution_label->Location = System::Drawing::Point(335, 337);
			this->Elution_label->Name = L"Elution_label";
			this->Elution_label->Size = System::Drawing::Size(103, 13);
			this->Elution_label->TabIndex = 75;
			this->Elution_label->Text = L"Elution window (min)";
			// 
			// Elution_textBox
			// 
			this->Elution_textBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Elution_textBox->Location = System::Drawing::Point(445, 333);
			this->Elution_textBox->Name = L"Elution_textBox";
			this->Elution_textBox->Size = System::Drawing::Size(41, 20);
			this->Elution_textBox->TabIndex = 76;
			this->Elution_textBox->Text = L"1.0";
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->AutoScroll = true;
			this->AutoSize = true;
			this->BackColor = System::Drawing::SystemColors::Control;
			this->ClientSize = System::Drawing::Size(1067, 642);
			this->Controls->Add(this->Elution_textBox);
			this->Controls->Add(this->Elution_label);
			this->Controls->Add(this->Enrichment_comboBox);
			this->Controls->Add(this->Output_browse_button);
			this->Controls->Add(this->Output_visualize_button);
			this->Controls->Add(this->Output_textBox);
			this->Controls->Add(this->Output_label);
			this->Controls->Add(this->enrichment_label);
			this->Controls->Add(this->Information_label);
			this->Controls->Add(this->Mass_Accuracy_textBox);
			this->Controls->Add(this->Mass_accuracy_label);
			this->Controls->Add(this->Rate_Constant_comboBox2);
			this->Controls->Add(this->Rate_Constant_label);
			this->Controls->Add(this->T9_label);
			this->Controls->Add(this->T14_label);
			this->Controls->Add(this->textBox15);
			this->Controls->Add(this->textBox6);
			this->Controls->Add(this->T8_label);
			this->Controls->Add(this->T13_label);
			this->Controls->Add(this->textBox14);
			this->Controls->Add(this->textBox7);
			this->Controls->Add(this->T7_label);
			this->Controls->Add(this->T12_label);
			this->Controls->Add(this->textBox13);
			this->Controls->Add(this->textBox8);
			this->Controls->Add(this->T6_label);
			this->Controls->Add(this->T11_label);
			this->Controls->Add(this->textBox12);
			this->Controls->Add(this->textBox9);
			this->Controls->Add(this->T5_label);
			this->Controls->Add(this->T10_label);
			this->Controls->Add(this->textBox11);
			this->Controls->Add(this->textBox10);
			this->Controls->Add(this->T4_label);
			this->Controls->Add(this->textBox5);
			this->Controls->Add(this->T3_label);
			this->Controls->Add(this->textBox4);
			this->Controls->Add(this->T2_label);
			this->Controls->Add(this->textBox3);
			this->Controls->Add(this->T1_label);
			this->Controls->Add(this->textBox2);
			this->Controls->Add(this->T0_label);
			this->Controls->Add(this->textBox1);
			this->Controls->Add(this->BWE0_label);
			this->Controls->Add(this->textBox_BWE0);
			this->Controls->Add(this->PeptideConsistency_Label);
			this->Controls->Add(this->PeptideConsistency_textBox);
			this->Controls->Add(this->MS1_Label);
			this->Controls->Add(this->MS1_comboBox);
			this->Controls->Add(this->PepScore_Label);
			this->Controls->Add(this->textBox_PepScore);
			//			this->Controls->Add(this->label1);
			this->Controls->Add(this->BWE1_label);
			this->Controls->Add(this->textBox_BWE1);
			this->Controls->Add(this->BWE2_label);
			this->Controls->Add(this->textBox_BWE2);
			this->Controls->Add(this->BWE3_label);
			this->Controls->Add(this->textBox_BWE3);
			this->Controls->Add(this->BWE4_label);
			this->Controls->Add(this->textBox_BWE4);
			this->Controls->Add(this->BWE5_label);
			this->Controls->Add(this->textBox_BWE5);
			this->Controls->Add(this->BWE6_label);
			this->Controls->Add(this->textBox_BWE6);
			this->Controls->Add(this->BWE7_label);
			this->Controls->Add(this->textBox_BWE7);
			this->Controls->Add(this->BWE8_label);
			this->Controls->Add(this->textBox_BWE8);
			this->Controls->Add(this->BWE9_label);
			this->Controls->Add(this->textBox_BWE9);
			this->Controls->Add(this->BWE10_label);
			this->Controls->Add(this->textBox_BWE10);
			this->Controls->Add(this->BWE11_label);
			this->Controls->Add(this->textBox_BWE11);
			this->Controls->Add(this->BWE12_label);
			this->Controls->Add(this->textBox_BWE12);
			this->Controls->Add(this->BWE13_label);
			this->Controls->Add(this->textBox_BWE13);
			this->Controls->Add(this->BWE14_label);
			this->Controls->Add(this->textBox_BWE14);
			//			this->Controls->Add(this->comboBox1);
			this->Controls->Add(this->About_button);
			this->Controls->Add(this->Start_button);
			this->Controls->Add(this->mzID_RichtextBox);
			this->Controls->Add(this->mzID_Remove);
			this->Controls->Add(this->mzID_Add);
			this->Controls->Add(this->mzID_Browse);
			this->Controls->Add(this->mzID_textBox);
			this->Controls->Add(this->mzID_label);
			this->Controls->Add(this->mzML_RichtextBox);
			this->Controls->Add(this->mzML_Remove);
			this->Controls->Add(this->mzML_Add);
			this->Controls->Add(this->mzML_Browse);
			this->Controls->Add(this->mzML_textBox);
			this->Controls->Add(this->mzML_label);
			this->Icon = (cli::safe_cast<System::Drawing::Icon^>(resources->GetObject(L"$this.Icon")));
			this->Name = L"Form1";
			this->RightToLeftLayout = true;
			this->Text = L"d2ome : GUI";
			this->Load += gcnew System::EventHandler(this, &Form1::Form1_Load);
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	private: System::Void label1_Click(System::Object^ sender, System::EventArgs^ e)
	{

	}
	private: System::Void mzML_Browse_Click(System::Object^ sender, System::EventArgs^ e)
	{
		/*fbd->ShowDialog();
		mzML_textBox->Text = fbd->SelectedPath;*/

		int countfile = 0;
		String^ prev = gcnew String("");
		//
		OFDmzML = gcnew OpenFileDialog;
		OFDmzML->Multiselect = true;
		OFDmzML->DefaultExt = ".mzML";
		//OFDmzID->Filter = "*.mzML";
		OFDmzML->Filter = "mzML files (*.mzML)|*.mzML";
		OFDmzML->ShowDialog();

		if (OFDmzML->FileNames->Length > 1)
		{
			//for (countfile=0; countfile<OFDmzML->FileNames->Length; countfile++)
			for (countfile = OFDmzML->FileNames->Length - 1; countfile >= 0; countfile--)
			{
				if (!OFDmzML->FileNames[countfile]->EndsWith(".mzML", StringComparison::OrdinalIgnoreCase))
				{
					MessageBox::Show(OFDmzML->FileNames[countfile] + " is not an mzML file");

					return;
				}

				//MessageBox::Show(OFDmzML->FileNames[countfile]);
				prev = OFDmzML->FileNames[countfile];

				//filename1 = mzML_textBox->Text + "\r\n";

				filename1 = mzML_textBox->Text;

				filename1 = prev + "\r\n" + filename1;

				mzML_textBox->Text = filename1;
			}

		}
		else
		{
			if (!OFDmzML->FileName->EndsWith(".mzML", StringComparison::OrdinalIgnoreCase))
			{
				MessageBox::Show(OFDmzML->FileName + " is not an mzML file");

				return;
			}

			mzML_textBox->Text = OFDmzML->FileName;
		}

	}
	private: System::Void mzML_Add_Click(System::Object^ sender, System::EventArgs^ e)
	{
		String^ prev = gcnew String("");
		//mzML_RichtextBox->Text = mzML_textBox->Text;
		if (mzML_textBox->Text != "" && OFDmzML->FileNames->Length == 1)
		{
			prev = mzML_RichtextBox->Text;
			filename1 = mzML_textBox->Text + "\r\n";
			filename1 = prev + filename1;
			mzML_RichtextBox->Text = filename1;
			mzML_textBox->Clear();
		}
		else if (mzML_textBox->Text != "")
		{
			mzML_RichtextBox->Text = mzML_textBox->Text;
			mzML_textBox->Clear();
		}
	}
			 ////private:System::Void mzML_textBox_DragDrop(System::Object ^  sender,System::Windows::Forms::DragEventArgs ^  e)
			 //   {
			 //		  int i;
			 //		  String ^s;
			 //
			 //	   // Get start position to drop the text.
			 //	   i = mzML_textBox->SelectionStart;
			 //	   s = mzML_textBox->Text->Substring(i);
			 //	   mzML_textBox->Text = mzML_textBox->Text->Substring(0,i);
			 //
			 //	   // Drop the text on to the RichTextBox.
			 //	   String ^str = String::Concat(mzML_textBox->Text, e->Data->GetData(DataFormats->Text)->ToString()); 
			 //	   mzML_textBox->Text = String::Concat(str, s);
			 //   }

	private: System::Void Form1_Load(System::Object^ sender, System::EventArgs^ e) {
	}
	private: System::Void mzML_Remove_Click(System::Object^ sender, System::EventArgs^ e)
	{
		String^ s = mzML_RichtextBox->SelectedText;
		mzML_RichtextBox->SelectedText = "";

		if (s != "")
		{

			filename1 = filename1->Replace(s + "\r\n", "");
			mzML_RichtextBox->Text = filename1;
		}



	}
	private: System::Void mzMLtextBox_TextChanged(System::Object^ sender, System::EventArgs^ e) {
	}
	private: System::Void mzID_Browse_Click(System::Object^ sender, System::EventArgs^ e)
	{

		int countfile = 0;
		String^ prev = gcnew String("");

		OFDmzID = gcnew OpenFileDialog;
		OFDmzID->Multiselect = true;
		OFDmzID->Filter = "mzid files (*.mzid)|*.mzid";
		OFDmzID->ShowDialog();


		if (OFDmzID->FileNames->Length > 1)
		{
			//for (countfile=0; countfile<OFDmzID->FileNames->Length; countfile++)
			for (countfile = OFDmzID->FileNames->Length - 1; countfile >= 0; countfile--)
			{
				prev = OFDmzID->FileNames[countfile];

				//filename2 = mzID_textBox->Text + "\r\n";
				filename2 = mzID_textBox->Text;

				filename2 = prev + "\r\n" + filename2;

				mzID_textBox->Text = filename2;
			}
		}
		else
		{
			mzID_textBox->Text = OFDmzID->FileName;
		}
	}



	private: System::Void mzID_Add_Click(System::Object^ sender, System::EventArgs^ e)
	{

		String^ prev = gcnew String("");

		// MessageBox::Show("TexBox " + mzID_textBox->Text);

		if (mzID_textBox->Text != "" && OFDmzID->FileNames->Length == 1)
		{
			prev = mzID_RichtextBox->Text;

			filename2 = mzID_textBox->Text + "\r\n";

			//MessageBox::Show(filename2 + " " + prev);

			filename2 = prev + filename2;

			mzID_RichtextBox->Text = filename2;

			mzID_textBox->Clear();
		}
		else if (mzID_textBox->Text != "")
		{
			mzID_RichtextBox->Text = mzID_textBox->Text;

			//MessageBox::Show("TextMedssage");

			mzID_textBox->Clear();
		}

		//MessageBox::Show("Text is " + filename2);
	}

	private: System::Void mzID_Remove_Click(System::Object^ sender, System::EventArgs^ e)
	{

		String^ s = mzID_RichtextBox->SelectedText;
		mzID_RichtextBox->SelectedText = "";

		if (s != "")
		{

			filename2 = filename2->Replace(s + "\r\n", "");
			mzID_RichtextBox->Text = filename2;
		}

	}

			 //


	private: System::Void mzML_RichtextBox_DragEnter(System::Object^ sender,
		System::Windows::Forms::DragEventArgs^ e)
	{
		if (e->Data->GetDataPresent(DataFormats::Text))
			e->Effect = DragDropEffects::Copy;
		else
			e->Effect = DragDropEffects::None;
	}


	private: System::Void mzML_RichtextBox_DragDrop(System::Object^ sender,
		System::Windows::Forms::DragEventArgs^ e)
	{
		int i;
		String^ s;
		String^ str;

		// Get start position to drop the text.
		i = mzML_RichtextBox->SelectionStart;
		s = mzML_RichtextBox->Text->Substring(i);
		mzML_RichtextBox->Text = mzML_RichtextBox->Text->Substring(0, i);

		// Drop the text on to the RichTextBox.
	   // Console::WriteLine("Test", e->Data->GetData(DataFormats::Text)->ToString());
		str = String::Concat(mzML_RichtextBox->Text, e->Data->GetData(DataFormats::Text)->ToString());
		mzML_RichtextBox->Text = String::Concat(str, s);
	}



	private: System::Void mzID_RichtextBox_DragEnter(System::Object^ sender,
		System::Windows::Forms::DragEventArgs^ e)
	{
		Console::WriteLine(e->Data->GetDataPresent(DataFormats::Text));
		if (e->Data->GetDataPresent(DataFormats::Text))
			e->Effect = DragDropEffects::Copy;
		else
			e->Effect = DragDropEffects::None;
	}


	private: System::Void mzID_RichtextBox_DragDrop(System::Object^ sender,
		System::Windows::Forms::DragEventArgs^ e)
	{
		int i;
		String^ s;
		String^ str;

		// Get start position to drop the text.
		i = mzID_RichtextBox->SelectionStart;
		s = mzID_RichtextBox->Text->Substring(i);
		mzID_RichtextBox->Text = mzID_RichtextBox->Text->Substring(0, i);

		// Drop the text on to the RichTextBox.
	   // Console::WriteLine("Test", e->Data->GetData(DataFormats::Text)->ToString());
		str = String::Concat(mzID_RichtextBox->Text, e->Data->GetData(DataFormats::Text)->ToString());
		mzID_RichtextBox->Text = String::Concat(str, s);
	}

			 //



	private: void ThreadProcUnsafe()
	{
		char* s1, *s2;

		char szCommandLine[2046];

		String^ sCommandFile = gcnew String("");

		String^ stOutPutFolder = Directory::GetCurrentDirectory();

		stringstream ss;

		ss << rate_constant_choice;

		string st2 = ss.str();

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(elution_window).ToPointer();
		string elution = string(s1);

		s2 = (char*)(void*)Marshal::StringToHGlobalAnsi(filename4).ToPointer();

		string output_dir = string(s2);

		int i;

		//string st1 = "A:\\current\\fresh_set_codes\\d2ome.exe files.txt";
		//string st3 = st1;

	//	const char * c = st3.c_str();
		//ret = system("Caller_mzIdentML.exe files.txt");

		sCommandFile = sExecsFolder + "\\d2ome.exe " + "files.txt";

		szCommandLine[0] = '\0';

		for (i = 0; i < sCommandFile->Length; i++)
		{
			szCommandLine[i] = sCommandFile[i];
		}

		szCommandLine[i] = '\0';

		//MessageBox::Show("CommandLine =  " + sCommandFile + "\n");

		//ret = system(c);

		ret = system(szCommandLine);

		String^ text = "", ^ stError;

		if (ret == 0)
		{
			MessageBox::Show("Finished... Please check the for the results in " + stOutPutFolder + " folder.");

			if (this->Start_button->InvokeRequired)
				this->Start_button->Invoke(gcnew UpdateTextCallback(this, &Form1::UpdateText), gcnew cli::array<Object^>{text});
		}
		else if (-10 == ret)
		{
			//printf("Ret %d\n", ret);

			stError = "Program terminates with error.\n";

			if (1 == this->MS1_type)
			{
				stError = stError + "The specified MS1 data type is Centroid \n";

				stError = stError + "It does not match with the MS1 type in mzML file\n";
			}


			MessageBox::Show(stError);
			if (this->Start_button->InvokeRequired)
				this->Start_button->Invoke(gcnew UpdateTextCallback(this, &Form1::UpdateText), gcnew cli::array<Object^>{text});
			//Start_button->Enabled = true;

			return;
		}
		else
		{
			//printf("Ret %d\n", ret);

			stError = "Program terminates with error.\n";

			//stError = stError + "Please make sure you convert all the input files correctly.\n";

			//stError = stError + "Make sure that there are no *.csv files in the output folder\n";

			//stError = stError + ret;

			MessageBox::Show(stError);
			if (this->Start_button->InvokeRequired)
				this->Start_button->Invoke(gcnew UpdateTextCallback(this, &Form1::UpdateText), gcnew cli::array<Object^>{text});
			//Start_button->Enabled = true;
		}
	}
	public: delegate void UpdateTextCallback(String^ text); //Delegate functiondelegate void UpdateTextCallback();
	public: void UpdateText(String^ text)
	{

		// Set the textbox text.
		//String ^str = gcnew String(text.c_str());

		this->Start_button->Enabled = true;


	}
			//private: System::Void Quant_File_Browse_Click(System::Object^  sender, System::EventArgs^  e) 
			//		 {
			//
			//			  /*fbd->ShowDialog();				  
			//			  Quant_File_textBox->Text = fbd->SelectedPath;*/
			//			  OpenFileDialog^ openFileDialog1 = gcnew OpenFileDialog;
			//			  openFileDialog1->ShowDialog();
			//			 /* Quant_File_textBox->Text = openFileDialog1->FileName;
			//			  filename3 = Quant_File_textBox->Text;*/
			//
			//		 }
			//private: System::Void Output_Dir_Browse_Click(System::Object^  sender, System::EventArgs^  e) 
			//		 {
			//
			//			 fbd->ShowDialog();				  
			//			 Output_Dir_textBox->Text = fbd->SelectedPath;
			//			 filename4 = Output_Dir_textBox->Text;
			//
			//		 }




			//public: void ShowString(String ^text)                         ///////////////This function cllas another thread to update the status bar///////////////////
			//{  if(this->ProgressBarText->InvokeRequired)
			//	  this->ProgressBarText->Invoke(gcnew UpdateTextCallback(this, &Form1::UpdateText),gcnew array<Object^>{text});													
			//	else
			//	{
			//		if(text!="")
			//		{
			//			this->progressBar1->Value += 1;
			//			this->ProgressBarText->Text = text;
			//			this->ProgressBarText->Refresh();
			//			System::Threading::Thread::Sleep(1000);
			//		}
			//		else
			//		{
			//			 this->progressBar1->Value += 0;
			//			 this->ProgressBarText->Refresh();
			//			 
			//		}
			//	}
			//}
			//public: delegate void UpdateTextCallback(String ^text); //Delegate functiondelegate void UpdateTextCallback();
			//public: void UpdateText(String ^text)
			//{
			//
			//	// Set the textbox text.
			//	//String ^str = gcnew String(text.c_str());
			//	
			//		this->progressBar1->Value += 1;
			//		this->ProgressBarText->Text = text;
			//		this->ProgressBarText->Refresh();
			//		System::Threading::Thread::Sleep(1000);
			//	
			//
			//}



			/*
			*
			*    This function creates the files.txt and quant.state
			*
			*
			*
			*/

			////////////////////////////////////////////////////////GUI Action class definition//////////////////////////////////////////////////////////////////////
	private: System::Void Start_button_Click(System::Object^ sender, System::EventArgs^ e)
	{
		//Read_Params(filename3);

		//Caller_mzML(filename1 + filename2);

		char* s1, *s2, *s3;

		int mzML_counter, mzID_counter, time_hour_counter, i, time_counter;

		cli::array<System::String^>^ args = gcnew cli::array<String^>(1);

		cli::array<String^, 1>^ FileList;

		std::string text_field_name[TIME_COURSE_POINT];//={"textBox1->Text, textBox2->Text, textBox3->Text, textBox4->Text, textBox5->Text, textBox6->Text, textBox7->Text, textBox8->Text, textBox9->Text, textBox10->Text,textBox11->Text, textBox12->Text, textBox13->Text, textBox14->Text, textBox15->Text"};

		List <String^>^ sBWE = gcnew List <String^>;

		FILE* fp_files_txt, *fp_quant_state;

		String^ sTemp = gcnew String("");

		float fElutionWindow = 1.0, fMassAccuracy = 20., fPepScore = 18.;

		bool bQuantFiles = false;

		int iPeptideConsistency = 4, MS1_Type = 0;   //MS1_Type = 0, MS1 data is profile, MS1_Type = 1, MS1 Data is centroid


		//sExecsFolder = gcnew String("");

		//sExecsFolder = Directory::GetCurrentDirectory();

		//MessageBox::Show("Execs is " + sExecsFolder);

		//Initialization of time course data
		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox1->Text).ToPointer();
		text_field_name[0] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox2->Text).ToPointer();
		text_field_name[1] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox3->Text).ToPointer();
		text_field_name[2] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox4->Text).ToPointer();
		text_field_name[3] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox5->Text).ToPointer();
		text_field_name[4] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox6->Text).ToPointer();
		text_field_name[5] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox7->Text).ToPointer();
		text_field_name[6] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox8->Text).ToPointer();
		text_field_name[7] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox9->Text).ToPointer();
		text_field_name[8] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox10->Text).ToPointer();
		text_field_name[9] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox11->Text).ToPointer();
		text_field_name[10] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox12->Text).ToPointer();
		text_field_name[11] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox13->Text).ToPointer();
		text_field_name[12] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox14->Text).ToPointer();
		text_field_name[13] = s1;

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox15->Text).ToPointer();
		text_field_name[14] = s1;

		///////----------------------  Fill in the BWE   ------------//////////////////////
		sTemp = gcnew String("");

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE0->Text).ToPointer();

		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE1->Text).ToPointer();
		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE2->Text).ToPointer();
		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE3->Text).ToPointer();
		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE4->Text).ToPointer();
		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}
		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE5->Text).ToPointer();
		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE6->Text).ToPointer();
		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE7->Text).ToPointer();
		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE8->Text).ToPointer();
		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE9->Text).ToPointer();

		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE10->Text).ToPointer();

		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE11->Text).ToPointer();

		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE12->Text).ToPointer();

		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE13->Text).ToPointer();

		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_BWE14->Text).ToPointer();

		if (s1 != NULL)
		{
			sTemp = gcnew String(s1);

			if (sTemp->Length > 0)
			{
				sBWE->Add(gcnew String(s1));
			}

		}

		for (i = 0; i < sBWE->Count; i++)
		{
			if ((float)(Convert::ToDouble(sBWE[i])) < 0.0 ||
				(float)(Convert::ToDouble(sBWE[i])) > 1.0)
			{
				MessageBox::Show("sBWE[" + i + "] = " + sBWE[i] +
					" should be non-negative and less than 1.0\n");

				return;
			}
		}
		//////---------------------- Finished:: Fill in the BWE   ------------//////////////////////


			//if(!System::Object::ReferenceEquals(filename1,nullptr) && !System::Object::ReferenceEquals(filename2,nullptr) && !System::Object::ReferenceEquals(filename3,nullptr))						 			
		   //if(filename1 == "")
		if (filename1 == "" || filename2 == "" || filename4 == "")
		{
			if (filename1 == "")
				MessageBox::Show("no mzML files.\n");
			else if (filename2 == "")
				MessageBox::Show("No mzid files.\n");
			else if (filename4 == "")
				MessageBox::Show("No output folder.\n");
		}
		else
		{
			Directory::SetCurrentDirectory(filename4);

			FileList = Directory::GetFiles(filename4);


			for (i = 0; i < FileList->Length; i++)
			{
				if (FileList[i]->EndsWith(".Quant.csv") ||
					FileList[i]->EndsWith(".RateConst.csv") ||
					FileList[i]->EndsWith(".csv"))
				{
					bQuantFiles = true;

					break;
				}
			}

			if (bQuantFiles)
			{
				//MessageBox::Show("There are either *.Quant.csv or *.RateConst.csv files in:\r\n " +
				MessageBox::Show("There are *.csv files in the output folder:  " +
					filename4 + "\n\r" + "They may interfere with output files.\r\n" +
					"Remove the csv files from the folder and run the program again.\n\r");

				return;
			}

			time_hour_counter = 0;

			for (i = 0; i < TIME_COURSE_POINT; i++)
			{
				s2 = (char*)text_field_name[i].c_str();

				if (std::strcmp(s2, ""))
				{
					time_course_hour[time_hour_counter] = atof(s2);
					time_hour_counter++;
				}
			}

			time_counter = time_hour_counter;

			fp_files_txt = fopen("files.txt", "w");

			if (NULL == fp_files_txt)
			{
				MessageBox::Show("Cannot write to files.txt. Exiting ...\n");

				return;
			}

			String^ delimStr = "\r\n";

			cli::array<Char>^ delimiter = delimStr->ToCharArray();

			cli::array<String^>^ mzML, ^ mzID;

			mzML = filename1->Split(delimiter);

			mzID = filename2->Split(delimiter);

			mzML_counter = mzID_counter = 0;


			if (sBWE->Count != mzML->Length / 2 || sBWE->Count != mzID->Length / 2)
			{
				MessageBox::Show("Mismatching numbers of mzML and mzID files = " + mzML->Length / 2 +
					" " + mzID->Length / 2 + " \n" +
					filename2 + " " + filename1 + "\n\r and number of BWE values = " + "\n\r" +
					sBWE->Count + "\n");

				return;
			}
			else
			{
				sTemp = gcnew String("");

				for (i = 0; i < sBWE->Count; i++)
				{
					if (i == 0)
					{
						sTemp = sTemp + " " + mzML[i * 2] + " " + mzID[i * 2] + " " + sBWE[i] + "\n";
					}
					else
					{
						sTemp = sTemp + mzML[i * 2] + " " + mzID[i * 2] + " " + sBWE[i] + "\n";
					}

				}

				MessageBox::Show("mzML and corresponding mzID files, and BWE values\n" +
					sTemp + "\n");
			}

			for (int word = 0; word < mzML->Length - 1; word += 2)
			{
				s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(mzML[word]).ToPointer();

				s2 = (char*)(void*)Marshal::StringToHGlobalAnsi(mzID[word]).ToPointer();

				s3 = (char*)text_field_name[word / 2].c_str();

				fprintf(fp_files_txt, "%s\t%s\t%s\t%s\n", s3, s1, s2, sBWE[word / 2]);

				if (strlen(s1) > 0)
					mzML_counter++;

				if (strlen(s2) > 0)
					mzID_counter++;
			}

			fclose(fp_files_txt);

			///////--------------- Fills in the quant.state file;   -----------------/////////////

			fp_quant_state = fopen("quant.state", "w");

			if (NULL == fp_quant_state)
			{
				MessageBox::Show("Cannot write to quant.state. Exiting ...\n");

				return;

			}

			s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(Mass_Accuracy_textBox->Text).ToPointer();

			sTemp = gcnew String("");

			if (s1 != NULL)
			{
				sTemp = gcnew String(s1);
			}

			if (sTemp->Length == 0 || (float)(Convert::ToDouble(sTemp)) <= 0.0)
			{
				MessageBox::Show("Invalid Mass Accuracy. " + sTemp +
					" Please, enter mass accuracy\n");

				return;
			}

			fMassAccuracy = (float)(Convert::ToDouble(sTemp));

			s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(Elution_textBox->Text).ToPointer();

			sTemp = gcnew String("");

			if (s1 != NULL)
			{
				sTemp = gcnew String(s1);
			}

			if (sTemp->Length == 0 || (float)(Convert::ToDouble(sTemp)) <= 0.0)
			{
				MessageBox::Show("Invalid Elution Window. " + sTemp +
					"Please, re-enter Elution Window\n");
				return;
			}

			fElutionWindow = (float)(Convert::ToDouble(sTemp));

			s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(textBox_PepScore->Text).ToPointer();

			sTemp = gcnew String("");

			if (s1 != NULL)
			{
				sTemp = gcnew String(s1);
			}

			if (sTemp->Length == 0 || (float)(Convert::ToDouble(sTemp)) <= 0.0)
			{
				MessageBox::Show("Invalid Peptide Identification Score. " + sTemp +
					"The code expects a non-negative value\n");
				return;
			}

			fPepScore = (float)(Convert::ToDouble(sTemp));

			if (MS1_comboBox->Text == "Profile")
				MS1_Type = 0;
			else if (MS1_comboBox->Text == "Centroid")
				MS1_Type = 1;

			this->MS1_type = MS1_Type;

			s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(PeptideConsistency_textBox->Text).ToPointer();

			sTemp = gcnew String("");

			if (s1 != NULL)
			{
				sTemp = gcnew String(s1);
			}

			if (sTemp->Length == 0 || (float)(Convert::ToInt32(sTemp)) <= 0)
			{
				MessageBox::Show("Invalid Peptide Consistency. " + sTemp +
					"Please, re-enter. Peptide Consistency value of 4 or higher is suggested\n");
				return;
			}

			iPeptideConsistency = (float)(Convert::ToInt32(sTemp));

			rate_constant_choice = 0;

			if (Rate_Constant_comboBox2->Text == "One Parameter")
				rate_constant_choice = 1;
			else if (Rate_Constant_comboBox2->Text == "Two Parameter")
				rate_constant_choice = 2;

			if (rate_constant_choice != 1 && rate_constant_choice != 2)
			{
				MessageBox::Show("Invalid Rate Constant. " + rate_constant_choice +
					"  Please, re-enter Rate Constant\n");
				return;
			}

			fprintf(fp_quant_state, "mass_accuracy = %5.1f ppm  ", fMassAccuracy);

			fprintf(fp_quant_state, "// mass accuracy: either in ppm or Da\n");

			fprintf(fp_quant_state, "MS1_Type = %d", MS1_Type);

			fprintf(fp_quant_state, "	// data type of MS1, 1 - centroid, 0 - profile \n");

			//fprintf(fp_quant_state, "protein_score       = 40     //minimum protein score\n");

			fprintf(fp_quant_state, "protein_score       = 40     //minimum protein score\n");

			fprintf(fp_quant_state, "peptide_score = %5.1f ", fPepScore);

			fprintf(fp_quant_state, "	// minimum peptide score, ion score in Mascot, default is 18\n");

			fprintf(fp_quant_state, "peptide_expectation = 0.05     // maximum peptide expectation in Mascot\n");

			fprintf(fp_quant_state, "elutiontimewindow   = %5.1f ", fElutionWindow);

			fprintf(fp_quant_state, " // time window  (mins) to search for elution peak. ");
			fprintf(fp_quant_state, "From the time that highest scoring MS2 was triggered\n");

			fprintf(fp_quant_state, "protein_consistency = %d", iPeptideConsistency);

			fprintf(fp_quant_state, "   // minimum number of experiments for protein consistency\n");

			fprintf(fp_quant_state, "peptide_consistency = %d", iPeptideConsistency);

			fprintf(fp_quant_state, "   //mininum number of experiments for a peptide consistency\n");

			fprintf(fp_quant_state, "NParam_RateConst_Fit = %d", rate_constant_choice);

			fprintf(fp_quant_state, "	// The model for fitting rate constant. Values are 1, and 2\n");

			fclose(fp_quant_state);


			///////////--------------- Finished filling out the quant.state file;   -----------------/////////////


					//if (comboBox1->Text == "")
						//MessageBox::Show("Please choose either Hour or Day in the Time course information\r\n");

					//else
			{

				if (time_hour_counter != mzML_counter)
					MessageBox::Show("Number of mzML files and time course value is not same.\r\n Please enter same number of mzML files and time course value.\r\n");
				else
				{
					if ((mzML_counter != mzID_counter))
						MessageBox::Show("Number of mzMLs " + mzML_counter + " and mzID files " +
							mzID_counter + " is not same.\r\n");
					else
					{
						try
						{

							//this->progressBar1->Refresh();		
							//this->ProgressBarText->Text = "Starting....";  


							//DisableProcessWindowsGhosting(); //Disable the ghosting or halting of the features
							rate_constant_choice = 1;
							if (Rate_Constant_comboBox2->Text == "One Parameter")
								rate_constant_choice = 1;
							else if (Rate_Constant_comboBox2->Text == "Two Parameter")
								rate_constant_choice = 2;

							//elution window value

							elution_window = this->Elution_textBox->Text;

							this->Start_button->Enabled = false;

							//this->Progress_label->Text=""+ progressBar1->Value.ToString();

							args[0] = gcnew String("files.txt");


							//int ret = system("Caller_mzIdentML.exe files.txt");
							/*this->demoThread = gcnew Thread(gcnew ThreadStart(this,&Form1::ThreadProcUnsafe));
							this->demoThread->Start();*/

							/*ThreadStart^ threadDelegate = gcnew ThreadStart(system("Caller_mzIdentML.exe files.txt"));*/
							Thread^ newThread = gcnew Thread(gcnew ThreadStart(this, &Form1::ThreadProcUnsafe));

							newThread->Start();
							/*while (newThread->IsAlive) ;
							if(ret == 0)
							{
								MessageBox::Show("Finished... Please check the installation  directory for the output files\n");
								this->Start_button->Enabled = true;
							}
							else
							{
								MessageBox::Show("Program terminates with an error.\n Please make sure you convert all the input files correctly.\n");
								 this->Start_button->Enabled = true;
							}*/
						}
						catch (...)
						{
							MessageBox::Show("Program terminates with an error.\n Please make sure you provide all the files correctly.\n");
							this->Start_button->Enabled = true;
						}

					}
				}

			}
		}

	}   // private: System::Void Start_button_Click


	private: System::Void button1_Click(System::Object^ sender, System::EventArgs^ e)
	{
		Form2^ f = gcnew Form2();
		f->ShowDialog();



	}

			 //private: System::Void timer1_Tick(System::Object^  sender, System::EventArgs^  e) 
			 //		 {
			 //			
			 //
			 //			  if(this->progressBar1->Value == this->progressBar1->Maximum)
			 //			 {
			 //				 this->timer1->Stop();
			 //				 MessageBox::Show("Finished... Please check the output directory for files\n");
			 //				 this->Start_button->Enabled = true;
			 //				 this->progressBar1->Value = 0;
			 //				 this->ProgressBarText->Text="";
			 //				 this->progressBar1->Refresh();
			 //				 //this->ControlBox = true;
			 //			 }
			 //
			 //
			 //		 }
	private: System::Void label2_Click(System::Object^ sender, System::EventArgs^ e) {
	}
	private: System::Void Enrichment_radioButton_CheckedChanged(System::Object^ sender, System::EventArgs^ e) {
	}
	private: System::Void Mass_Accuracy_radioButton_CheckedChanged(System::Object^ sender, System::EventArgs^ e) {
	}
	private: System::Void MPE_radio_button_CheckedChanged(System::Object^ sender, System::EventArgs^ e) {
	}
	private: System::Void No_curve_radioButton_CheckedChanged(System::Object^ sender, System::EventArgs^ e) {
	}
	private: System::Void NNLS_radioButton_CheckedChanged(System::Object^ sender, System::EventArgs^ e) {
	}
	private: System::Void Rate_Constant_comboBox2_SelectedIndexChanged(System::Object^ sender, System::EventArgs^ e) {
	}
	private: System::Void MS1_comboBox_SelectedIndexChanged(System::Object^ sender, System::EventArgs^ e) {
	}

	private: System::Void Output_browse_button_Click(System::Object^ sender, System::EventArgs^ e)
	{

		fbd->ShowDialog();
		Output_textBox->Text = fbd->SelectedPath;
		filename4 = Output_textBox->Text;
	}

	private: System::Void Output_visualize_button_Click(System::Object^ sender, System::EventArgs^ e)
	{
		v2::FormLoader fl;
		fl.startVisualizerForm(Output_textBox->Text);
	}

	};

}

