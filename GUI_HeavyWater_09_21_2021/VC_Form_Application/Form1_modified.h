////////////////////////////////Main GUI Form Application//////////////////////////////////////////

////////////////////////////////Mahbubur Rahman////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////


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
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;

using namespace System::Runtime::InteropServices;

using namespace System::IO;
using namespace System::Threading;


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
			
		}

	public: 
        int ret;	
		int isotope_deconv_choice;
		String ^elution_window;
		//DisableProcessWindowsGhosting Lib "user32" (); //Disable Not Responding Text

	public: static int progress1, progress2, progress3, progress4;

	private: System::Windows::Forms::Label^  Isotope_Incorporatioon_label;

	private: System::Windows::Forms::Label^  Mass_accuracy_label;
	private: System::Windows::Forms::TextBox^  Mass_Accuracy_textBox;

	private: System::Windows::Forms::Label^  enrichment_label;
	private: System::Windows::Forms::Label^  Output_label;
	private: System::Windows::Forms::TextBox^  Output_textBox;
	private: System::Windows::Forms::Button^  Output_browse_button;
	private: System::Windows::Forms::ComboBox^  Enrichment_comboBox;
	private: System::Windows::Forms::Label^  Elution_label;
	private: System::Windows::Forms::TextBox^  Elution_textBox;

	private: System::Windows::Forms::ComboBox^  Isotope_comboBox2;

	public: 

	public: 

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

			if(time_course_day)
				delete []time_course_day;
			if(time_course_hour)
				delete []time_course_hour;
		}
	private: System::Windows::Forms::Label^  mzML_label;
	private: System::Windows::Forms::TextBox^  mzML_textBox;
	private: System::Windows::Forms::Button^  mzML_Browse;
	private: System::Windows::Forms::Button^  mzML_Add;
	private: System::Windows::Forms::Button^  mzML_Remove;
	private: System::Windows::Forms::TextBox^  mzML_RichtextBox;
	private: System::Windows::Forms::FolderBrowserDialog ^ fbd;
	private: String ^filename1;
	private: String ^filename2;
	private: String ^filename3;
    private: String ^filename4;
    
	private: double *time_course_day;
	private: double *time_course_hour;
		 
	private: System::Windows::Forms::Label^  mzID_label;
	private: System::Windows::Forms::TextBox^  mzID_textBox;
	private: System::Windows::Forms::Button^  mzID_Browse;
	private: System::Windows::Forms::Button^  mzID_Add;
	private: System::Windows::Forms::Button^  mzID_Remove;
	private: System::Windows::Forms::TextBox^  mzID_RichtextBox;

	private: System::Windows::Forms::Button^  Start_button;
	private: System::Windows::Forms::Button^  About_button;
	private: System::Windows::Forms::ComboBox^  comboBox1;
	private: System::Windows::Forms::Label^  label1;

	private: System::Windows::Forms::TextBox^  textBox1;
	private: System::Windows::Forms::Label^  T0_label;
	private: System::Windows::Forms::TextBox^  textBox2;
	private: System::Windows::Forms::Label^  T1_label;
	private: System::Windows::Forms::TextBox^  textBox3;
	private: System::Windows::Forms::Label^  T2_label;
	private: System::Windows::Forms::TextBox^  textBox4;
	private: System::Windows::Forms::Label^  T3_label;
	private: System::Windows::Forms::TextBox^  textBox5;
	private: System::Windows::Forms::Label^  T4_label;
	private: System::Windows::Forms::Label^  T14_label;

	private: System::Windows::Forms::TextBox^  textBox6;
	private: System::Windows::Forms::Label^  T13_label;

	private: System::Windows::Forms::TextBox^  textBox7;
	private: System::Windows::Forms::Label^  T12_label;

	private: System::Windows::Forms::TextBox^  textBox8;
	private: System::Windows::Forms::Label^  T11_label;

	private: System::Windows::Forms::TextBox^  textBox9;
	private: System::Windows::Forms::Label^  T10_label;

	private: System::Windows::Forms::TextBox^  textBox10;

	private: System::Windows::Forms::TextBox^  textBox11;
	private: System::Windows::Forms::Label^  T5_label;
	private: System::Windows::Forms::TextBox^  textBox12;
	private: System::Windows::Forms::Label^  T6_label;
	private: System::Windows::Forms::TextBox^  textBox13;
	private: System::Windows::Forms::Label^  T7_label;
	private: System::Windows::Forms::TextBox^  textBox14;
	private: System::Windows::Forms::Label^  T8_label;
	private: System::Windows::Forms::TextBox^  textBox15;
	private: System::Windows::Forms::Label^  T9_label;

	private: System::ComponentModel::IContainer^  components;




	protected: 

	protected: 

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>


#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			System::ComponentModel::ComponentResourceManager^  resources = (gcnew System::ComponentModel::ComponentResourceManager(Form1::typeid));
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
			this->T0_label = (gcnew System::Windows::Forms::Label());
			this->textBox2 = (gcnew System::Windows::Forms::TextBox());
			this->T1_label = (gcnew System::Windows::Forms::Label());
			this->textBox3 = (gcnew System::Windows::Forms::TextBox());
			this->T2_label = (gcnew System::Windows::Forms::Label());
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
			this->Isotope_Incorporatioon_label = (gcnew System::Windows::Forms::Label());
			this->Isotope_comboBox2 = (gcnew System::Windows::Forms::ComboBox());
			this->Mass_accuracy_label = (gcnew System::Windows::Forms::Label());
			this->Mass_Accuracy_textBox = (gcnew System::Windows::Forms::TextBox());
			this->enrichment_label = (gcnew System::Windows::Forms::Label());
			this->Output_label = (gcnew System::Windows::Forms::Label());
			this->Output_textBox = (gcnew System::Windows::Forms::TextBox());
			this->Output_browse_button = (gcnew System::Windows::Forms::Button());
			this->Enrichment_comboBox = (gcnew System::Windows::Forms::ComboBox());
			this->Elution_label = (gcnew System::Windows::Forms::Label());
			this->Elution_textBox = (gcnew System::Windows::Forms::TextBox());
			this->SuspendLayout();
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
			this->mzML_textBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzML_textBox->Location = System::Drawing::Point(101, 64);
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
//			this->mzML_RichtextBox->EnableAutoDragDrop = true;
			this->mzML_RichtextBox->Location = System::Drawing::Point(105, 139);
			this->mzML_RichtextBox->Name = L"mzML_RichtextBox";
			this->mzML_RichtextBox->Size = System::Drawing::Size(278, 140);
			this->mzML_RichtextBox->TabIndex = 5;
			this->mzML_RichtextBox->Text = L"";
			this->mzML_RichtextBox->DragDrop += gcnew System::Windows::Forms::DragEventHandler(this, &Form1::mzML_RichtextBox_DragDrop);
			this->mzML_RichtextBox->DragEnter += gcnew System::Windows::Forms::DragEventHandler(this, &Form1::mzML_RichtextBox_DragEnter);
			this->mzML_RichtextBox->TextChanged += gcnew System::EventHandler(this, &Form1::mzMLtextBox_TextChanged);
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
			this->mzID_textBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->mzID_textBox->Location = System::Drawing::Point(652, 65);
			this->mzID_textBox->Name = L"mzID_textBox";
			this->mzID_textBox->Size = System::Drawing::Size(283, 20);
			this->mzID_textBox->TabIndex = 7;
			// 
			// mzID_Browse
			// 
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
//			this->mzID_RichtextBox->EnableAutoDragDrop = true;
			this->mzID_RichtextBox->Location = System::Drawing::Point(657, 139);
			this->mzID_RichtextBox->Name = L"mzID_RichtextBox";
			this->mzID_RichtextBox->Size = System::Drawing::Size(278, 140);
			this->mzID_RichtextBox->TabIndex = 11;
			this->mzID_RichtextBox->Text = L"";
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
			this->About_button->Location = System::Drawing::Point(700, 462);
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
			this->comboBox1->Items->AddRange(gcnew cli::array< System::Object^  >(2) {L"Day", L"Hour"});
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
			// textBox1
			// 
			this->textBox1->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox1->Location = System::Drawing::Point(88, 430);
			this->textBox1->Name = L"textBox1";
			this->textBox1->Size = System::Drawing::Size(37, 20);
			this->textBox1->TabIndex = 26;
			// 
			// T0_label
			// 
			this->T0_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T0_label->AutoSize = true;
			this->T0_label->Location = System::Drawing::Point(41, 432);
			this->T0_label->Name = L"T0_label";
			this->T0_label->Size = System::Drawing::Size(20, 13);
			this->T0_label->TabIndex = 27;
			this->T0_label->Text = L"T0";
			// 
			// textBox2
			// 
			this->textBox2->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox2->Location = System::Drawing::Point(88, 458);
			this->textBox2->Name = L"textBox2";
			this->textBox2->Size = System::Drawing::Size(37, 20);
			this->textBox2->TabIndex = 28;
			// 
			// T1_label
			// 
			this->T1_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T1_label->AutoSize = true;
			this->T1_label->Location = System::Drawing::Point(41, 461);
			this->T1_label->Name = L"T1_label";
			this->T1_label->Size = System::Drawing::Size(20, 13);
			this->T1_label->TabIndex = 29;
			this->T1_label->Text = L"T1";
			// 
			// textBox3
			// 
			this->textBox3->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox3->Location = System::Drawing::Point(88, 485);
			this->textBox3->Name = L"textBox3";
			this->textBox3->Size = System::Drawing::Size(37, 20);
			this->textBox3->TabIndex = 30;
			// 
			// T2_label
			// 
			this->T2_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T2_label->AutoSize = true;
			this->T2_label->Location = System::Drawing::Point(41, 488);
			this->T2_label->Name = L"T2_label";
			this->T2_label->Size = System::Drawing::Size(20, 13);
			this->T2_label->TabIndex = 31;
			this->T2_label->Text = L"T2";
			// 
			// textBox4
			// 
			this->textBox4->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox4->Location = System::Drawing::Point(88, 512);
			this->textBox4->Name = L"textBox4";
			this->textBox4->Size = System::Drawing::Size(37, 20);
			this->textBox4->TabIndex = 32;
			// 
			// T3_label
			// 
			this->T3_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T3_label->AutoSize = true;
			this->T3_label->Location = System::Drawing::Point(41, 515);
			this->T3_label->Name = L"T3_label";
			this->T3_label->Size = System::Drawing::Size(20, 13);
			this->T3_label->TabIndex = 33;
			this->T3_label->Text = L"T3";
			// 
			// textBox5
			// 
			this->textBox5->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox5->Location = System::Drawing::Point(88, 539);
			this->textBox5->Name = L"textBox5";
			this->textBox5->Size = System::Drawing::Size(37, 20);
			this->textBox5->TabIndex = 34;
			// 
			// T4_label
			// 
			this->T4_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T4_label->AutoSize = true;
			this->T4_label->Location = System::Drawing::Point(41, 542);
			this->T4_label->Name = L"T4_label";
			this->T4_label->Size = System::Drawing::Size(20, 13);
			this->T4_label->TabIndex = 35;
			this->T4_label->Text = L"T4";
			// 
			// T14_label
			// 
			this->T14_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T14_label->AutoSize = true;
			this->T14_label->Location = System::Drawing::Point(437, 541);
			this->T14_label->Name = L"T14_label";
			this->T14_label->Size = System::Drawing::Size(26, 13);
			this->T14_label->TabIndex = 50;
			this->T14_label->Text = L"T14";
			// 
			// textBox6
			// 
			this->textBox6->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox6->Location = System::Drawing::Point(484, 538);
			this->textBox6->Name = L"textBox6";
			this->textBox6->Size = System::Drawing::Size(37, 20);
			this->textBox6->TabIndex = 49;
			// 
			// T13_label
			// 
			this->T13_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T13_label->AutoSize = true;
			this->T13_label->Location = System::Drawing::Point(437, 514);
			this->T13_label->Name = L"T13_label";
			this->T13_label->Size = System::Drawing::Size(26, 13);
			this->T13_label->TabIndex = 48;
			this->T13_label->Text = L"T13";
			// 
			// textBox7
			// 
			this->textBox7->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox7->Location = System::Drawing::Point(484, 511);
			this->textBox7->Name = L"textBox7";
			this->textBox7->Size = System::Drawing::Size(37, 20);
			this->textBox7->TabIndex = 47;
			// 
			// T12_label
			// 
			this->T12_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T12_label->AutoSize = true;
			this->T12_label->Location = System::Drawing::Point(437, 487);
			this->T12_label->Name = L"T12_label";
			this->T12_label->Size = System::Drawing::Size(26, 13);
			this->T12_label->TabIndex = 46;
			this->T12_label->Text = L"T12";
			// 
			// textBox8
			// 
			this->textBox8->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox8->Location = System::Drawing::Point(484, 484);
			this->textBox8->Name = L"textBox8";
			this->textBox8->Size = System::Drawing::Size(37, 20);
			this->textBox8->TabIndex = 45;
			// 
			// T11_label
			// 
			this->T11_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T11_label->AutoSize = true;
			this->T11_label->Location = System::Drawing::Point(437, 460);
			this->T11_label->Name = L"T11_label";
			this->T11_label->Size = System::Drawing::Size(26, 13);
			this->T11_label->TabIndex = 44;
			this->T11_label->Text = L"T11";
			// 
			// textBox9
			// 
			this->textBox9->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox9->Location = System::Drawing::Point(484, 457);
			this->textBox9->Name = L"textBox9";
			this->textBox9->Size = System::Drawing::Size(37, 20);
			this->textBox9->TabIndex = 43;
			// 
			// T10_label
			// 
			this->T10_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T10_label->AutoSize = true;
			this->T10_label->Location = System::Drawing::Point(437, 431);
			this->T10_label->Name = L"T10_label";
			this->T10_label->Size = System::Drawing::Size(26, 13);
			this->T10_label->TabIndex = 42;
			this->T10_label->Text = L"T10";
			// 
			// textBox10
			// 
			this->textBox10->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox10->Location = System::Drawing::Point(484, 429);
			this->textBox10->Name = L"textBox10";
			this->textBox10->Size = System::Drawing::Size(37, 20);
			this->textBox10->TabIndex = 41;
			// 
			// textBox11
			// 
			this->textBox11->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox11->Location = System::Drawing::Point(280, 429);
			this->textBox11->Name = L"textBox11";
			this->textBox11->Size = System::Drawing::Size(37, 20);
			this->textBox11->TabIndex = 41;
			// 
			// T5_label
			// 
			this->T5_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T5_label->AutoSize = true;
			this->T5_label->Location = System::Drawing::Point(233, 431);
			this->T5_label->Name = L"T5_label";
			this->T5_label->Size = System::Drawing::Size(20, 13);
			this->T5_label->TabIndex = 42;
			this->T5_label->Text = L"T5";
			// 
			// textBox12
			// 
			this->textBox12->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox12->Location = System::Drawing::Point(280, 457);
			this->textBox12->Name = L"textBox12";
			this->textBox12->Size = System::Drawing::Size(37, 20);
			this->textBox12->TabIndex = 43;
			// 
			// T6_label
			// 
			this->T6_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T6_label->AutoSize = true;
			this->T6_label->Location = System::Drawing::Point(233, 460);
			this->T6_label->Name = L"T6_label";
			this->T6_label->Size = System::Drawing::Size(20, 13);
			this->T6_label->TabIndex = 44;
			this->T6_label->Text = L"T6";
			// 
			// textBox13
			// 
			this->textBox13->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox13->Location = System::Drawing::Point(280, 484);
			this->textBox13->Name = L"textBox13";
			this->textBox13->Size = System::Drawing::Size(37, 20);
			this->textBox13->TabIndex = 45;
			// 
			// T7_label
			// 
			this->T7_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T7_label->AutoSize = true;
			this->T7_label->Location = System::Drawing::Point(233, 487);
			this->T7_label->Name = L"T7_label";
			this->T7_label->Size = System::Drawing::Size(20, 13);
			this->T7_label->TabIndex = 46;
			this->T7_label->Text = L"T7";
			// 
			// textBox14
			// 
			this->textBox14->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox14->Location = System::Drawing::Point(280, 511);
			this->textBox14->Name = L"textBox14";
			this->textBox14->Size = System::Drawing::Size(37, 20);
			this->textBox14->TabIndex = 47;
			// 
			// T8_label
			// 
			this->T8_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T8_label->AutoSize = true;
			this->T8_label->Location = System::Drawing::Point(233, 514);
			this->T8_label->Name = L"T8_label";
			this->T8_label->Size = System::Drawing::Size(20, 13);
			this->T8_label->TabIndex = 48;
			this->T8_label->Text = L"T8";
			// 
			// textBox15
			// 
			this->textBox15->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->textBox15->Location = System::Drawing::Point(280, 538);
			this->textBox15->Name = L"textBox15";
			this->textBox15->Size = System::Drawing::Size(37, 20);
			this->textBox15->TabIndex = 49;
			// 
			// T9_label
			// 
			this->T9_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->T9_label->AutoSize = true;
			this->T9_label->Location = System::Drawing::Point(233, 541);
			this->T9_label->Name = L"T9_label";
			this->T9_label->Size = System::Drawing::Size(20, 13);
			this->T9_label->TabIndex = 50;
			this->T9_label->Text = L"T9";
			// 
			// Isotope_Incorporatioon_label
			// 
			this->Isotope_Incorporatioon_label->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Isotope_Incorporatioon_label->AutoSize = true;
			this->Isotope_Incorporatioon_label->Location = System::Drawing::Point(735, 379);
			this->Isotope_Incorporatioon_label->Name = L"Isotope_Incorporatioon_label";
			this->Isotope_Incorporatioon_label->Size = System::Drawing::Size(114, 13);
			this->Isotope_Incorporatioon_label->TabIndex = 54;
			this->Isotope_Incorporatioon_label->Text = L"Isotope Deconvolution";
			// 
			// Isotope_comboBox2
			// 
			this->Isotope_comboBox2->AllowDrop = true;
			this->Isotope_comboBox2->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Isotope_comboBox2->DropDownStyle = System::Windows::Forms::ComboBoxStyle::DropDownList;
			this->Isotope_comboBox2->FormattingEnabled = true;
			this->Isotope_comboBox2->Items->AddRange(gcnew cli::array< System::Object^  >(3) {L"No Curve", L"MPE", L"NNLS"});
			this->Isotope_comboBox2->Location = System::Drawing::Point(717, 412);
			this->Isotope_comboBox2->Name = L"Isotope_comboBox2";
			this->Isotope_comboBox2->Size = System::Drawing::Size(162, 21);
			this->Isotope_comboBox2->TabIndex = 63;
			this->Isotope_comboBox2->SelectedIndexChanged += gcnew System::EventHandler(this, &Form1::Isotope_comboBox2_SelectedIndexChanged);
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
			this->Mass_Accuracy_textBox->Text = L"0.0";
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
			// 
			// Enrichment_comboBox
			// 
			this->Enrichment_comboBox->AllowDrop = true;
			this->Enrichment_comboBox->Anchor = System::Windows::Forms::AnchorStyles::None;
			this->Enrichment_comboBox->DropDownStyle = System::Windows::Forms::ComboBoxStyle::DropDownList;
			this->Enrichment_comboBox->FormattingEnabled = true;
			this->Enrichment_comboBox->Items->AddRange(gcnew cli::array< System::Object^  >(4) {L"2H", L"15N", L"13C", L"18O"});
			this->Enrichment_comboBox->Location = System::Drawing::Point(39, 346);
			this->Enrichment_comboBox->Name = L"Enrichment_comboBox";
			this->Enrichment_comboBox->Size = System::Drawing::Size(70, 21);
			this->Enrichment_comboBox->TabIndex = 74;
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
			this->Elution_textBox->Text = L"2.0";
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
			this->Controls->Add(this->Output_textBox);
			this->Controls->Add(this->Output_label);
			this->Controls->Add(this->enrichment_label);
			this->Controls->Add(this->Mass_Accuracy_textBox);
			this->Controls->Add(this->Mass_accuracy_label);
			this->Controls->Add(this->Isotope_comboBox2);
			this->Controls->Add(this->Isotope_Incorporatioon_label);
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
			this->Controls->Add(this->label1);
			this->Controls->Add(this->comboBox1);
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
			this->Icon = (cli::safe_cast<System::Drawing::Icon^  >(resources->GetObject(L"$this.Icon")));
			this->Name = L"Form1";
			this->RightToLeftLayout = true;
			this->Text = L"Heavywater : User Interface";
			this->Load += gcnew System::EventHandler(this, &Form1::Form1_Load);
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	private: System::Void label1_Click(System::Object^  sender, System::EventArgs^  e) 
			 {

			 }
	private: System::Void mzML_Browse_Click(System::Object^  sender, System::EventArgs^  e) 
			 {
				  /*fbd->ShowDialog();				  
				  mzML_textBox->Text = fbd->SelectedPath;*/
				  OpenFileDialog^ openFileDialog1 = gcnew OpenFileDialog;
				  openFileDialog1->ShowDialog();
				  openFileDialog1->Multiselect = true;
				  mzML_textBox->Text = openFileDialog1->FileName;

			 }
private: System::Void mzML_Add_Click(System::Object^  sender, System::EventArgs^  e) 
		 {
			 String ^prev = gcnew String("");
			 //mzML_RichtextBox->Text = mzML_textBox->Text;
			 if(mzML_textBox->Text!="")
			 {
				 prev = mzML_RichtextBox->Text;
				 filename1 = mzML_textBox->Text + "\r\n";
				 filename1 = prev+filename1;
				 mzML_RichtextBox->Text = filename1;			 
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

private: System::Void Form1_Load(System::Object^  sender, System::EventArgs^  e) {
		 }
private: System::Void mzML_Remove_Click(System::Object^  sender, System::EventArgs^  e) 
		 {
				String ^s = mzML_RichtextBox->SelectedText;
				mzML_RichtextBox->SelectedText = "";

				if (s != "")
				{
                             
					filename1 = filename1->Replace(s + "\r\n", "");
					mzML_RichtextBox->Text = filename1;
				}
			 


		 }
private: System::Void mzMLtextBox_TextChanged(System::Object^  sender, System::EventArgs^  e) {
		 }
private: System::Void mzID_Browse_Click(System::Object^  sender, System::EventArgs^  e) 
		 {

			 /*fbd->ShowDialog();				  
			 mzID_textBox->Text = fbd->SelectedPath;*/
			 OpenFileDialog^ openFileDialog1 = gcnew OpenFileDialog;
			 openFileDialog1->ShowDialog();
			 openFileDialog1->Multiselect = true;
			 mzID_textBox->Text = openFileDialog1->FileName;

		 }

//Added by JA 2016.08.05
private: System::Void mzML_RichtextBox_DragEnter(System::Object ^  sender,
      System::Windows::Forms::DragEventArgs ^  e)
   {
      if (e->Data->GetDataPresent(DataFormats::Text))
         e->Effect = DragDropEffects::Copy;
      else
         e->Effect = DragDropEffects::None;
   }

//Added by JA 2016.08.05
private: System::Void mzML_RichtextBox_DragDrop(System::Object ^  sender,
      System::Windows::Forms::DragEventArgs ^  e)
   {
      int i;
      String ^s;
	  String ^str;

   // Get start position to drop the text.
   i = mzML_RichtextBox->SelectionStart;
   s = mzML_RichtextBox->Text->Substring(i);
   mzML_RichtextBox->Text = mzML_RichtextBox->Text->Substring(0,i);

   // Drop the text on to the RichTextBox.
  // Console::WriteLine("Test", e->Data->GetData(DataFormats::Text)->ToString());
   str = String::Concat(mzML_RichtextBox->Text, e->Data->GetData(DataFormats::Text)->ToString()); 
   mzML_RichtextBox->Text = String::Concat(str, s);
   }


//Added by JA 2016.08.08
private: System::Void mzID_RichtextBox_DragEnter(System::Object ^  sender,
      System::Windows::Forms::DragEventArgs ^  e)
   {
	   Console::WriteLine(e->Data->GetDataPresent(DataFormats::Text));
      if (e->Data->GetDataPresent(DataFormats::Text))
         e->Effect = DragDropEffects::Copy;
      else
         e->Effect = DragDropEffects::None;
   }

//Added by JA 2016.08.08
private: System::Void mzID_RichtextBox_DragDrop(System::Object ^  sender,
      System::Windows::Forms::DragEventArgs ^  e)
   {
      int i;
      String ^s;
	  String ^str;

   // Get start position to drop the text.
   i = mzID_RichtextBox->SelectionStart;
   s = mzID_RichtextBox->Text->Substring(i);
   mzID_RichtextBox->Text = mzID_RichtextBox->Text->Substring(0,i);

   // Drop the text on to the RichTextBox.
  // Console::WriteLine("Test", e->Data->GetData(DataFormats::Text)->ToString());
   str = String::Concat(mzID_RichtextBox->Text, e->Data->GetData(DataFormats::Text)->ToString()); 
   mzID_RichtextBox->Text = String::Concat(str, s);
   }








private: System::Void mzID_Add_Click(System::Object^  sender, System::EventArgs^  e) 
		 {

			  String ^prev = gcnew String("");
			 //mzML_RichtextBox->Text = mzML_textBox->Text;
			 if(mzID_textBox->Text!="")
			 {
				 prev = mzID_RichtextBox->Text;
				 filename2 = mzID_textBox->Text + "\r\n";
				 filename2 = prev+filename2;
				 mzID_RichtextBox->Text = filename2;			 
				 mzID_textBox->Clear();
			 }

		 }
private: System::Void mzID_Remove_Click(System::Object^  sender, System::EventArgs^  e) 
		 {

			    String ^s = mzID_RichtextBox->SelectedText;
				mzID_RichtextBox->SelectedText = "";

				if (s != "")
				{
                             
					filename2 = filename2->Replace(s + "\r\n", "");
					mzID_RichtextBox->Text = filename2;
				}

		 }

private: void ThreadProcUnsafe()
    {
		
		char *s1,*s2;
		string st1 = "Caller_mzIdentML.exe files.txt";

		stringstream ss;
		ss << isotope_deconv_choice;
		string st2 = ss.str();

		s1 =  (char*)(void*)Marshal::StringToHGlobalAnsi(elution_window).ToPointer();
		string elution = string(s1);
		
		s2 =  (char*)(void*)Marshal::StringToHGlobalAnsi(filename4).ToPointer();
		string output_dir = string(s2);

		string st3 = st1+" "+st2+" "+elution+" "+output_dir;
		const char * c = st3.c_str();
        //ret = system("Caller_mzIdentML.exe files.txt");
		ret = system(c);
		String ^text ="";
		if(ret == 0)
		{
			MessageBox::Show("Finished... Please check the output directory for the output files\n");
			if(this->Start_button->InvokeRequired)
				 this->Start_button->Invoke(gcnew UpdateTextCallback(this, &Form1::UpdateText),gcnew array<Object^>{text});	
		}
		else
		{
            MessageBox::Show("Program terminates with an error.\n Please make sure you convert all the input files correctly.\n");
			if(this->Start_button->InvokeRequired)
				 this->Start_button->Invoke(gcnew UpdateTextCallback(this, &Form1::UpdateText),gcnew array<Object^>{text});	
			//Start_button->Enabled = true;
		}
    }
public: delegate void UpdateTextCallback(String ^text); //Delegate functiondelegate void UpdateTextCallback();
public: void UpdateText(String ^text)
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





////////////////////////////////////////////////////////GUI Action class definition//////////////////////////////////////////////////////////////////////
private: System::Void Start_button_Click(System::Object^  sender, System::EventArgs^  e) 
		 {
			 //Read_Params(filename3);
			 
			 //Caller_mzML(filename1 + filename2);
			 
			 char *s1, *s2;
			 int mzML_counter, mzID_counter, time_hour_counter, time_day_counter,i, time_counter;
			 array<System::String ^> ^args = gcnew array<String^>(1);
			 std::string text_field_name[TIME_COURSE_POINT];//={"textBox1->Text, textBox2->Text, textBox3->Text, textBox4->Text, textBox5->Text, textBox6->Text, textBox7->Text, textBox8->Text, textBox9->Text, textBox10->Text,textBox11->Text, textBox12->Text, textBox13->Text, textBox14->Text, textBox15->Text"};
			 
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

			 //////////////////////////////////////////////////////////////////////////////////////////////////////
		    




			 

			 //if(!System::Object::ReferenceEquals(filename1,nullptr) && !System::Object::ReferenceEquals(filename2,nullptr) && !System::Object::ReferenceEquals(filename3,nullptr))						 			
			if(filename1 == "" || filename2 == ""||filename4== "")
			{
				      MessageBox::Show("Please provide necessary filenames, output directory, time scale information and then press start.\n");
			}
			else	    
			{

							ofstream out;
							 out.open("files.txt");
							 if(out==NULL)
							 {
								 MessageBox::Show("Cannot create input mzMl and mzid file. Please try again\n");
							 }


							 String^ delimStr = "\r\n";
							 array<Char>^ delimiter = delimStr->ToCharArray( );
							 array<String^>^ mzML, ^mzID;
							 mzML = filename1->Split( delimiter );
							 mzID = filename2->Split( delimiter );
							 mzML_counter = mzID_counter = 0;
							
							 
							 for (int word=0; word<mzML->Length-1; word+=2)
							 {
								 s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(mzML[word]).ToPointer();
								 s2 = (char*)(void*)Marshal::StringToHGlobalAnsi(mzID[word]).ToPointer();
								 if(strlen(s1) > 0 && strlen(s2) > 0)
								 {
									out<<s1<<"\n"<<s2<<"\n";
									
									
								 }
								 if(strlen(s1) > 0)
									 mzML_counter++;
								 if(strlen(s2) > 0)
									 mzID_counter++;
							 } 

							 out.close();

							if(comboBox1->Text=="")
								MessageBox::Show("Please choose either Hour or Day in the Time course information\r\n");

							else
							{   
								  time_hour_counter = time_day_counter = 0;
								  
								  if(comboBox1->Text=="Hour")
								  {									 
									  
									  for(i = 0; i < TIME_COURSE_POINT; i++)
									  {									  
											s2 =  (char*)text_field_name[i].c_str();

										     if(std::strcmp(s2,""))
											 {
												//s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(text_field_name[i]).ToPointer();
												//s1 = (char*)text_field_name[i].c_str();
												time_course_hour[time_hour_counter] = atof(s2);
												time_hour_counter++;
											 }
									  }
									  time_counter = time_hour_counter;
								  }
								  if(comboBox1->Text=="Day")
								  {


									  for(i = 0; i < TIME_COURSE_POINT; i++)
									  {									  
											s1 =  (char*)text_field_name[i].c_str();

										     if(std::strcmp(s1,""))
											 {
												//s1 = (char*)(void*)Marshal::StringToHGlobalAnsi(text_field_name[i]).ToPointer();
												//s1 = (char*)text_field_name[i].c_str();
												time_course_day[time_day_counter] = atof(s1);
												time_day_counter++;
											 }
									  }
									  time_counter = time_day_counter;



								  }
								  
								  if(comboBox1->Text=="Hour" && time_hour_counter != mzML_counter)
									 MessageBox::Show("Number of mzML files and time course value is not same.\r\n Please enter same number of mzML files and time course value.\r\n");							  																			  
								
								   else if(comboBox1->Text=="Day" && time_day_counter != mzML_counter)
											  MessageBox::Show("Number of mzML files and time course value is not same.\r\n Please enter same number of mzML files and time course value.\r\n");							  
								   else
								   {
											 if(mzML_counter != mzID_counter)
												 MessageBox::Show("Number of mzML and mzID files is not same.\r\n Please enter same number of mzML and mzID files.\r\n");
											 else
											 {

												  try
												  {							
                                                    
													//this->progressBar1->Refresh();		
													//this->ProgressBarText->Text = "Starting....";  
													
                                                    
													//DisableProcessWindowsGhosting(); //Disable the ghosting or halting of the features
													 isotope_deconv_choice = 0;
													 if(Isotope_comboBox2->Text == "MPE")
														 isotope_deconv_choice = 1;
													 else if(Isotope_comboBox2->Text == "NNLS")
													     isotope_deconv_choice = 2;

													 //elution window value

													 elution_window = this->Elution_textBox->Text;

													this->Start_button->Enabled = false;
													
													//this->Progress_label->Text=""+ progressBar1->Value.ToString();
												  
													args[0] = gcnew String("files.txt");
													
													
													//int ret = system("Caller_mzIdentML.exe files.txt");
													/*this->demoThread = gcnew Thread(gcnew ThreadStart(this,&Form1::ThreadProcUnsafe));
													this->demoThread->Start();*/

													/*ThreadStart^ threadDelegate = gcnew ThreadStart(system("Caller_mzIdentML.exe files.txt"));*/
													Thread^ newThread = gcnew Thread(gcnew ThreadStart(this,&Form1::ThreadProcUnsafe));
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
												 catch(...)
												 {
														MessageBox::Show("Program terminates with an error.\n Please make sure you provide all the files correctly.\n");
														this->Start_button->Enabled = true;
												 }		

											 }
									}


								  
								  
							}
			}

		 }
private: System::Void button1_Click(System::Object^  sender, System::EventArgs^  e) 
		 {
			 Form2 ^f = gcnew Form2();
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
private: System::Void label2_Click(System::Object^  sender, System::EventArgs^  e) {
		 }
private: System::Void Enrichment_radioButton_CheckedChanged(System::Object^  sender, System::EventArgs^  e) {
		 }
private: System::Void Mass_Accuracy_radioButton_CheckedChanged(System::Object^  sender, System::EventArgs^  e) {
		 }
private: System::Void MPE_radio_button_CheckedChanged(System::Object^  sender, System::EventArgs^  e) {
		 }
private: System::Void No_curve_radioButton_CheckedChanged(System::Object^  sender, System::EventArgs^  e) {
		 }
private: System::Void NNLS_radioButton_CheckedChanged(System::Object^  sender, System::EventArgs^  e) {
		 }
private: System::Void Isotope_comboBox2_SelectedIndexChanged(System::Object^  sender, System::EventArgs^  e) {
		 }
private: System::Void Output_browse_button_Click(System::Object^  sender, System::EventArgs^  e) 
		 {

			fbd->ShowDialog();				  
			Output_textBox->Text = fbd->SelectedPath;
			filename4 = Output_textBox->Text;
		 }
};

}

