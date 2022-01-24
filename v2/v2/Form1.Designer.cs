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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridView_peptide = new System.Windows.Forms.DataGridView();
            this.button_exportAllPeptideChart = new System.Windows.Forms.Button();
            this.txt_source = new System.Windows.Forms.TextBox();
            this.btn_Browsefolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_proteinNameSelector = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4_proteinRateConstantValue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5_Ic = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.button_exportProteinChart = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_exportPeptideChart = new System.Windows.Forms.Button();
            this.chart_peptide = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_peptide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_peptide)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_peptide
            // 
            this.dataGridView_peptide.AllowUserToAddRows = false;
            this.dataGridView_peptide.AllowUserToDeleteRows = false;
            this.dataGridView_peptide.AllowUserToOrderColumns = true;
            this.dataGridView_peptide.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView_peptide.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_peptide.ColumnHeadersHeight = 46;
            this.dataGridView_peptide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_peptide.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_peptide.Name = "dataGridView_peptide";
            this.dataGridView_peptide.ReadOnly = true;
            this.dataGridView_peptide.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_peptide.ShowEditingIcon = false;
            this.dataGridView_peptide.Size = new System.Drawing.Size(429, 825);
            this.dataGridView_peptide.TabIndex = 1;
            this.dataGridView_peptide.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_peptide_CellClick);
            this.dataGridView_peptide.SelectionChanged += new System.EventHandler(this.dataGridView_peptide_SelectionChanged);
            // 
            // button_exportAllPeptideChart
            // 
            this.button_exportAllPeptideChart.Location = new System.Drawing.Point(337, 12);
            this.button_exportAllPeptideChart.Name = "button_exportAllPeptideChart";
            this.button_exportAllPeptideChart.Size = new System.Drawing.Size(75, 23);
            this.button_exportAllPeptideChart.TabIndex = 9;
            this.button_exportAllPeptideChart.Text = "Export All";
            this.button_exportAllPeptideChart.UseVisualStyleBackColor = true;
            this.button_exportAllPeptideChart.Click += new System.EventHandler(this.button_exportAllPeptideChart_Click);
            // 
            // txt_source
            // 
            this.txt_source.Location = new System.Drawing.Point(88, 32);
            this.txt_source.Name = "txt_source";
            this.txt_source.Size = new System.Drawing.Size(303, 20);
            this.txt_source.TabIndex = 11;
            // 
            // btn_Browsefolder
            // 
            this.btn_Browsefolder.Location = new System.Drawing.Point(396, 30);
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
            this.label1.Location = new System.Drawing.Point(21, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Source";
            // 
            // comboBox_proteinNameSelector
            // 
            this.comboBox_proteinNameSelector.FormattingEnabled = true;
            this.comboBox_proteinNameSelector.Location = new System.Drawing.Point(529, 31);
            this.comboBox_proteinNameSelector.Name = "comboBox_proteinNameSelector";
            this.comboBox_proteinNameSelector.Size = new System.Drawing.Size(226, 21);
            this.comboBox_proteinNameSelector.TabIndex = 14;
            this.comboBox_proteinNameSelector.SelectedIndexChanged += new System.EventHandler(this.comboBox_proteinNameSelector_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(469, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Protein";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(794, 27);
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
            this.label4_proteinRateConstantValue.Location = new System.Drawing.Point(905, 27);
            this.label4_proteinRateConstantValue.Name = "label4_proteinRateConstantValue";
            this.label4_proteinRateConstantValue.Size = new System.Drawing.Size(0, 13);
            this.label4_proteinRateConstantValue.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(794, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Total ion current";
            // 
            // label5_Ic
            // 
            this.label5_Ic.AutoSize = true;
            this.label5_Ic.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label5_Ic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5_Ic.Location = new System.Drawing.Point(905, 50);
            this.label5_Ic.Name = "label5_Ic";
            this.label5_Ic.Size = new System.Drawing.Size(0, 13);
            this.label5_Ic.TabIndex = 19;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txt_source);
            this.splitContainer1.Panel1.Controls.Add(this.label5_Ic);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Browsefolder);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.label4_proteinRateConstantValue);
            this.splitContainer1.Panel1.Controls.Add(this.comboBox_proteinNameSelector);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1142, 981);
            this.splitContainer1.SplitterDistance = 93;
            this.splitContainer1.TabIndex = 20;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(1142, 884);
            this.splitContainer2.SplitterDistance = 429;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.dataGridView_peptide);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.button_exportAllPeptideChart);
            this.splitContainer3.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer3_Panel2_Paint);
            this.splitContainer3.Size = new System.Drawing.Size(429, 884);
            this.splitContainer3.SplitterDistance = 825;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.button_exportPeptideChart);
            this.splitContainer4.Panel1.Controls.Add(this.chart_peptide);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer4.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer4_Panel2_Paint);
            this.splitContainer4.Size = new System.Drawing.Size(709, 884);
            this.splitContainer4.SplitterDistance = 389;
            this.splitContainer4.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.button_exportProteinChart);
            this.splitContainer5.Panel1.Controls.Add(this.chart1);
            this.splitContainer5.Size = new System.Drawing.Size(709, 491);
            this.splitContainer5.SplitterDistance = 415;
            this.splitContainer5.TabIndex = 0;
            // 
            // button_exportProteinChart
            // 
            this.button_exportProteinChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_exportProteinChart.Location = new System.Drawing.Point(624, 382);
            this.button_exportProteinChart.Margin = new System.Windows.Forms.Padding(10);
            this.button_exportProteinChart.Name = "button_exportProteinChart";
            this.button_exportProteinChart.Size = new System.Drawing.Size(75, 23);
            this.button_exportProteinChart.TabIndex = 16;
            this.button_exportProteinChart.Text = "Export Graph";
            this.button_exportProteinChart.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea4.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea4);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend4.Name = "Legend1";
            this.chart1.Legends.Add(legend4);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(10);
            this.chart1.Name = "chart1";
            this.chart1.Padding = new System.Windows.Forms.Padding(10);
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series7.Legend = "Legend1";
            series7.MarkerColor = System.Drawing.Color.MidnightBlue;
            series7.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Diamond;
            series7.Name = "Series1";
            series7.YValuesPerPoint = 2;
            series8.BorderWidth = 2;
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series8.Color = System.Drawing.Color.Purple;
            series8.Legend = "Legend1";
            series8.Name = "Series2";
            this.chart1.Series.Add(series7);
            this.chart1.Series.Add(series8);
            this.chart1.Size = new System.Drawing.Size(709, 415);
            this.chart1.TabIndex = 15;
            this.chart1.Text = "chart1";
            // 
            // button_exportPeptideChart
            // 
            this.button_exportPeptideChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_exportPeptideChart.Location = new System.Drawing.Point(608, 362);
            this.button_exportPeptideChart.Name = "button_exportPeptideChart";
            this.button_exportPeptideChart.Size = new System.Drawing.Size(75, 23);
            this.button_exportPeptideChart.TabIndex = 13;
            this.button_exportPeptideChart.Text = "Export Graph";
            this.button_exportPeptideChart.UseVisualStyleBackColor = true;
            // 
            // chart_peptide
            // 
            this.chart_peptide.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea3.Name = "ChartArea1";
            this.chart_peptide.ChartAreas.Add(chartArea3);
            this.chart_peptide.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chart_peptide.Legends.Add(legend3);
            this.chart_peptide.Location = new System.Drawing.Point(0, 0);
            this.chart_peptide.Name = "chart_peptide";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series5.Legend = "Legend1";
            series5.MarkerColor = System.Drawing.Color.Black;
            series5.MarkerSize = 7;
            series5.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series5.Name = "Series1";
            series5.YValuesPerPoint = 2;
            series6.BorderWidth = 2;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series6.Color = System.Drawing.Color.Purple;
            series6.Legend = "Legend1";
            series6.Name = "Series3";
            this.chart_peptide.Series.Add(series5);
            this.chart_peptide.Series.Add(series6);
            this.chart_peptide.Size = new System.Drawing.Size(709, 389);
            this.chart_peptide.TabIndex = 12;
            this.chart_peptide.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1142, 981);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "d2ome: Visualization";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_peptide)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_peptide)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView_peptide;
        private System.Windows.Forms.Button button_exportAllPeptideChart;
        private System.Windows.Forms.TextBox txt_source;
        private System.Windows.Forms.Button btn_Browsefolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_proteinNameSelector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4_proteinRateConstantValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5_Ic;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Button button_exportProteinChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button button_exportPeptideChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_peptide;
    }
}

