namespace v2
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label4_proteinRateConstantValue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_proteinNameSelector = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_Browsefolder = new System.Windows.Forms.Button();
            this.txt_source = new System.Windows.Forms.TextBox();
            this.groupBox3_proteinchart = new System.Windows.Forms.GroupBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_exportAllPeptideChart = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_exportPeptideChart = new System.Windows.Forms.Button();
            this.chart_peptide = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView_peptide = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3_proteinchart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_peptide)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_peptide)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1202, 708);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1043, 633);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Computation";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.label4_proteinRateConstantValue);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.comboBox_proteinNameSelector);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.btn_Browsefolder);
            this.tabPage2.Controls.Add(this.txt_source);
            this.tabPage2.Controls.Add(this.groupBox3_proteinchart);
            this.tabPage2.Controls.Add(this.button_exportAllPeptideChart);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1194, 682);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Visualization";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1043, 633);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "About Heavy water";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 29);
            this.label1.MaximumSize = new System.Drawing.Size(700, 500);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(689, 312);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label4_proteinRateConstantValue
            // 
            this.label4_proteinRateConstantValue.AutoSize = true;
            this.label4_proteinRateConstantValue.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4_proteinRateConstantValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4_proteinRateConstantValue.Location = new System.Drawing.Point(935, 24);
            this.label4_proteinRateConstantValue.Name = "label4_proteinRateConstantValue";
            this.label4_proteinRateConstantValue.Size = new System.Drawing.Size(0, 13);
            this.label4_proteinRateConstantValue.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(824, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Protein rate constant";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(473, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Protein";
            // 
            // comboBox_proteinNameSelector
            // 
            this.comboBox_proteinNameSelector.FormattingEnabled = true;
            this.comboBox_proteinNameSelector.Location = new System.Drawing.Point(519, 16);
            this.comboBox_proteinNameSelector.Name = "comboBox_proteinNameSelector";
            this.comboBox_proteinNameSelector.Size = new System.Drawing.Size(226, 21);
            this.comboBox_proteinNameSelector.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Source";
            // 
            // btn_Browsefolder
            // 
            this.btn_Browsefolder.Location = new System.Drawing.Point(386, 15);
            this.btn_Browsefolder.Name = "btn_Browsefolder";
            this.btn_Browsefolder.Size = new System.Drawing.Size(52, 23);
            this.btn_Browsefolder.TabIndex = 23;
            this.btn_Browsefolder.Text = "Browse";
            this.btn_Browsefolder.UseVisualStyleBackColor = true;
            // 
            // txt_source
            // 
            this.txt_source.Location = new System.Drawing.Point(78, 17);
            this.txt_source.Name = "txt_source";
            this.txt_source.Size = new System.Drawing.Size(303, 20);
            this.txt_source.TabIndex = 22;
            // 
            // groupBox3_proteinchart
            // 
            this.groupBox3_proteinchart.Controls.Add(this.chart1);
            this.groupBox3_proteinchart.Location = new System.Drawing.Point(470, 410);
            this.groupBox3_proteinchart.Name = "groupBox3_proteinchart";
            this.groupBox3_proteinchart.Size = new System.Drawing.Size(674, 258);
            this.groupBox3_proteinchart.TabIndex = 21;
            this.groupBox3_proteinchart.TabStop = false;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(6, 13);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Legend = "Legend1";
            series1.MarkerColor = System.Drawing.Color.MidnightBlue;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Diamond;
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Purple;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(662, 239);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // button_exportAllPeptideChart
            // 
            this.button_exportAllPeptideChart.Location = new System.Drawing.Point(363, 645);
            this.button_exportAllPeptideChart.Name = "button_exportAllPeptideChart";
            this.button_exportAllPeptideChart.Size = new System.Drawing.Size(75, 23);
            this.button_exportAllPeptideChart.TabIndex = 20;
            this.button_exportAllPeptideChart.Text = "Export All";
            this.button_exportAllPeptideChart.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_exportPeptideChart);
            this.groupBox2.Controls.Add(this.chart_peptide);
            this.groupBox2.Location = new System.Drawing.Point(470, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(674, 340);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            // 
            // button_exportPeptideChart
            // 
            this.button_exportPeptideChart.Location = new System.Drawing.Point(593, 309);
            this.button_exportPeptideChart.Name = "button_exportPeptideChart";
            this.button_exportPeptideChart.Size = new System.Drawing.Size(75, 23);
            this.button_exportPeptideChart.TabIndex = 9;
            this.button_exportPeptideChart.Text = "Export Graph";
            this.button_exportPeptideChart.UseVisualStyleBackColor = true;
            // 
            // chart_peptide
            // 
            this.chart_peptide.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea2.Name = "ChartArea1";
            this.chart_peptide.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart_peptide.Legends.Add(legend2);
            this.chart_peptide.Location = new System.Drawing.Point(6, 16);
            this.chart_peptide.Name = "chart_peptide";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series3.Legend = "Legend1";
            series3.MarkerColor = System.Drawing.Color.Black;
            series3.MarkerSize = 7;
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "Series1";
            series3.YValuesPerPoint = 2;
            series4.BorderWidth = 2;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Color = System.Drawing.Color.Purple;
            series4.Legend = "Legend1";
            series4.MarkerSize = 7;
            series4.Name = "Series3";
            this.chart_peptide.Series.Add(series3);
            this.chart_peptide.Series.Add(series4);
            this.chart_peptide.Size = new System.Drawing.Size(662, 316);
            this.chart_peptide.TabIndex = 0;
            this.chart_peptide.Text = "chart1";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.dataGridView_peptide);
            this.groupBox1.Location = new System.Drawing.Point(31, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 576);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Peptides";
            // 
            // dataGridView_peptide
            // 
            this.dataGridView_peptide.AllowUserToAddRows = false;
            this.dataGridView_peptide.AllowUserToDeleteRows = false;
            this.dataGridView_peptide.AllowUserToOrderColumns = true;
            this.dataGridView_peptide.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_peptide.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView_peptide.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_peptide.ColumnHeadersHeight = 46;
            this.dataGridView_peptide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_peptide.Location = new System.Drawing.Point(3, 16);
            this.dataGridView_peptide.Name = "dataGridView_peptide";
            this.dataGridView_peptide.ReadOnly = true;
            this.dataGridView_peptide.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_peptide.ShowEditingIcon = false;
            this.dataGridView_peptide.Size = new System.Drawing.Size(404, 557);
            this.dataGridView_peptide.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1226, 723);
            this.Controls.Add(this.tabControl1);
            this.Name = "Main";
            this.Text = "Main";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox3_proteinchart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_peptide)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_peptide)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4_proteinRateConstantValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_proteinNameSelector;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_Browsefolder;
        private System.Windows.Forms.TextBox txt_source;
        private System.Windows.Forms.GroupBox groupBox3_proteinchart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button button_exportAllPeptideChart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_exportPeptideChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_peptide;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView_peptide;
    }
}