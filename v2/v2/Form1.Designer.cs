namespace v2
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridView_peptide = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_exportPeptideChart = new System.Windows.Forms.Button();
            this.chart_peptide = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_exportAllPeptideChart = new System.Windows.Forms.Button();
            this.groupBox3_proteinchart = new System.Windows.Forms.GroupBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txt_source = new System.Windows.Forms.TextBox();
            this.btn_Browsefolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_proteinNameSelector = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4_proteinRateConstantValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_peptide)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_peptide)).BeginInit();
            this.groupBox3_proteinchart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_peptide
            // 
            this.dataGridView_peptide.AllowUserToAddRows = false;
            this.dataGridView_peptide.AllowUserToDeleteRows = false;
            this.dataGridView_peptide.AllowUserToOrderColumns = true;
            this.dataGridView_peptide.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_peptide.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView_peptide.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_peptide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_peptide.Location = new System.Drawing.Point(3, 16);
            this.dataGridView_peptide.Name = "dataGridView_peptide";
            this.dataGridView_peptide.ReadOnly = true;
            this.dataGridView_peptide.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_peptide.ShowEditingIcon = false;
            this.dataGridView_peptide.Size = new System.Drawing.Size(404, 557);
            this.dataGridView_peptide.TabIndex = 1;
            this.dataGridView_peptide.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_peptide_CellClick);
            this.dataGridView_peptide.SelectionChanged += new System.EventHandler(this.dataGridView_peptide_SelectionChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.dataGridView_peptide);
            this.groupBox1.Location = new System.Drawing.Point(12, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 576);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Peptides";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_exportPeptideChart);
            this.groupBox2.Controls.Add(this.chart_peptide);
            this.groupBox2.Location = new System.Drawing.Point(451, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(674, 340);
            this.groupBox2.TabIndex = 7;
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
            this.button_exportPeptideChart.Click += new System.EventHandler(this.button_exportPeptideChart_Click);
            // 
            // chart_peptide
            // 
            this.chart_peptide.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea5.Name = "ChartArea1";
            this.chart_peptide.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chart_peptide.Legends.Add(legend5);
            this.chart_peptide.Location = new System.Drawing.Point(6, 16);
            this.chart_peptide.Name = "chart_peptide";
            series9.ChartArea = "ChartArea1";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series9.Legend = "Legend1";
            series9.MarkerColor = System.Drawing.Color.Black;
            series9.MarkerSize = 7;
            series9.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series9.Name = "Series1";
            series9.YValuesPerPoint = 2;
            series10.BorderWidth = 2;
            series10.ChartArea = "ChartArea1";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series10.Color = System.Drawing.Color.Purple;
            series10.Legend = "Legend1";
            series10.MarkerSize = 7;
            series10.Name = "Series3";
            this.chart_peptide.Series.Add(series9);
            this.chart_peptide.Series.Add(series10);
            this.chart_peptide.Size = new System.Drawing.Size(662, 316);
            this.chart_peptide.TabIndex = 0;
            this.chart_peptide.Text = "chart1";
            // 
            // button_exportAllPeptideChart
            // 
            this.button_exportAllPeptideChart.Location = new System.Drawing.Point(344, 653);
            this.button_exportAllPeptideChart.Name = "button_exportAllPeptideChart";
            this.button_exportAllPeptideChart.Size = new System.Drawing.Size(75, 23);
            this.button_exportAllPeptideChart.TabIndex = 9;
            this.button_exportAllPeptideChart.Text = "Export All";
            this.button_exportAllPeptideChart.UseVisualStyleBackColor = true;
            this.button_exportAllPeptideChart.Click += new System.EventHandler(this.button_exportAllPeptideChart_Click);
            // 
            // groupBox3_proteinchart
            // 
            this.groupBox3_proteinchart.Controls.Add(this.chart1);
            this.groupBox3_proteinchart.Location = new System.Drawing.Point(451, 418);
            this.groupBox3_proteinchart.Name = "groupBox3_proteinchart";
            this.groupBox3_proteinchart.Size = new System.Drawing.Size(674, 258);
            this.groupBox3_proteinchart.TabIndex = 10;
            this.groupBox3_proteinchart.TabStop = false;
            // 
            // chart1
            // 
            chartArea6.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chart1.Legends.Add(legend6);
            this.chart1.Location = new System.Drawing.Point(6, 13);
            this.chart1.Name = "chart1";
            series11.ChartArea = "ChartArea1";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series11.Legend = "Legend1";
            series11.MarkerColor = System.Drawing.Color.MidnightBlue;
            series11.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Diamond;
            series11.Name = "Series1";
            series11.YValuesPerPoint = 2;
            series12.BorderWidth = 3;
            series12.ChartArea = "ChartArea1";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series12.Color = System.Drawing.Color.Purple;
            series12.Legend = "Legend1";
            series12.Name = "Series2";
            this.chart1.Series.Add(series11);
            this.chart1.Series.Add(series12);
            this.chart1.Size = new System.Drawing.Size(662, 239);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // txt_source
            // 
            this.txt_source.Location = new System.Drawing.Point(59, 25);
            this.txt_source.Name = "txt_source";
            this.txt_source.Size = new System.Drawing.Size(303, 20);
            this.txt_source.TabIndex = 11;
            // 
            // btn_Browsefolder
            // 
            this.btn_Browsefolder.Location = new System.Drawing.Point(367, 23);
            this.btn_Browsefolder.Name = "btn_Browsefolder";
            this.btn_Browsefolder.Size = new System.Drawing.Size(52, 23);
            this.btn_Browsefolder.TabIndex = 12;
            this.btn_Browsefolder.Text = "Browse";
            this.btn_Browsefolder.UseVisualStyleBackColor = true;
            this.btn_Browsefolder.Click += new System.EventHandler(this.btn_Browsefolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Source";
            // 
            // comboBox_proteinNameSelector
            // 
            this.comboBox_proteinNameSelector.FormattingEnabled = true;
            this.comboBox_proteinNameSelector.Location = new System.Drawing.Point(500, 24);
            this.comboBox_proteinNameSelector.Name = "comboBox_proteinNameSelector";
            this.comboBox_proteinNameSelector.Size = new System.Drawing.Size(226, 21);
            this.comboBox_proteinNameSelector.TabIndex = 14;
            this.comboBox_proteinNameSelector.SelectedIndexChanged += new System.EventHandler(this.comboBox_proteinNameSelector_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(454, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Protein";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(805, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Protein rate constant";
            // 
            // label4_proteinRateConstantValue
            // 
            this.label4_proteinRateConstantValue.AutoSize = true;
            this.label4_proteinRateConstantValue.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4_proteinRateConstantValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4_proteinRateConstantValue.Location = new System.Drawing.Point(916, 32);
            this.label4_proteinRateConstantValue.Name = "label4_proteinRateConstantValue";
            this.label4_proteinRateConstantValue.Size = new System.Drawing.Size(0, 13);
            this.label4_proteinRateConstantValue.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1153, 682);
            this.Controls.Add(this.label4_proteinRateConstantValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_proteinNameSelector);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Browsefolder);
            this.Controls.Add(this.txt_source);
            this.Controls.Add(this.groupBox3_proteinchart);
            this.Controls.Add(this.button_exportAllPeptideChart);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "d2ome_v2";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_peptide)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_peptide)).EndInit();
            this.groupBox3_proteinchart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView_peptide;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_peptide;
        private System.Windows.Forms.Button button_exportAllPeptideChart;
        private System.Windows.Forms.Button button_exportPeptideChart;
        private System.Windows.Forms.GroupBox groupBox3_proteinchart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TextBox txt_source;
        private System.Windows.Forms.Button btn_Browsefolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_proteinNameSelector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4_proteinRateConstantValue;
    }
}

