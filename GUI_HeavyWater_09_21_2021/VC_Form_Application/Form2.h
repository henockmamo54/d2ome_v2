#pragma once

namespace VC_Form_Application {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// Summary for Form2
	/// </summary>
	public ref class Form2 : public System::Windows::Forms::Form
	{
	public:
		Form2(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~Form2()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::RichTextBox^  About_richTextBox;
	private: System::Windows::Forms::Button^  OK_button;
	protected: 


	protected: 

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			System::ComponentModel::ComponentResourceManager^  resources = (gcnew System::ComponentModel::ComponentResourceManager(Form2::typeid));
			this->About_richTextBox = (gcnew System::Windows::Forms::RichTextBox());
			this->OK_button = (gcnew System::Windows::Forms::Button());
			this->SuspendLayout();
			// 
			// About_richTextBox
			// 
			this->About_richTextBox->BackColor = System::Drawing::SystemColors::HighlightText;
			this->About_richTextBox->ForeColor = System::Drawing::Color::Black;
			this->About_richTextBox->HideSelection = false;
			this->About_richTextBox->Location = System::Drawing::Point(78, 72);
			this->About_richTextBox->Name = L"About_richTextBox";
			this->About_richTextBox->ReadOnly = true;
			this->About_richTextBox->Size = System::Drawing::Size(494, 370);
			this->About_richTextBox->TabIndex = 0;
			this->About_richTextBox->Text = resources->GetString(L"About_richTextBox.Text");
			this->About_richTextBox->TextChanged += gcnew System::EventHandler(this, &Form2::richTextBox1_TextChanged);
			// 
			// OK_button
			// 
			this->OK_button->Location = System::Drawing::Point(255, 464);
			this->OK_button->Name = L"OK_button";
			this->OK_button->Size = System::Drawing::Size(157, 36);
			this->OK_button->TabIndex = 1;
			this->OK_button->Text = L"OK";
			this->OK_button->UseVisualStyleBackColor = true;
			this->OK_button->Click += gcnew System::EventHandler(this, &Form2::OK_button_Click);
			// 
			// Form2
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(660, 533);
			this->Controls->Add(this->OK_button);
			this->Controls->Add(this->About_richTextBox);
			this->Icon = (cli::safe_cast<System::Drawing::Icon^  >(resources->GetObject(L"$this.Icon")));
			this->Name = L"Form2";
			this->Text = L"About Heavy Water";
			this->Load += gcnew System::EventHandler(this, &Form2::Form2_Load);
			this->ResumeLayout(false);

		}
#pragma endregion
	private: System::Void richTextBox1_TextChanged(System::Object^  sender, System::EventArgs^  e) {
			 }
	private: System::Void Form2_Load(System::Object^  sender, System::EventArgs^  e) {
			 }
	private: System::Void OK_button_Click(System::Object^  sender, System::EventArgs^  e) 
			 {
				 this->Close();
			 }
	};
}
