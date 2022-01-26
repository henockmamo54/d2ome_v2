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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridView_peptide = new System.Windows.Forms.DataGridView();
            this.button_exportPeptideChart = new System.Windows.Forms.Button();
            this.chart_peptide = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_exportAllPeptideChart = new System.Windows.Forms.Button();
            this.button_exportProteinChart = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txt_source = new System.Windows.Forms.TextBox();
            this.btn_Browsefolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_proteinNameSelector = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4_proteinRateConstantValue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5_Ic = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar_exportall = new System.Windows.Forms.ProgressBar();
            this.button2_cancelled = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox3_proteinchart = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_peptide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_peptide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox3_proteinchart.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
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
            this.dataGridView_peptide.ColumnHeadersHeight = 46;
            this.dataGridView_peptide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_peptide.Location = new System.Drawing.Point(3, 16);
            this.dataGridView_peptide.Name = "dataGridView_peptide";
            this.dataGridView_peptide.ReadOnly = true;
            this.dataGridView_peptide.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_peptide.ShowEditingIcon = false;
            this.dataGridView_peptide.Size = new System.Drawing.Size(480, 609);
            this.dataGridView_peptide.TabIndex = 1;
            this.dataGridView_peptide.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_peptide_CellClick);
            this.dataGridView_peptide.SelectionChanged += new System.EventHandler(this.dataGridView_peptide_SelectionChanged);
            // 
            // button_exportPeptideChart
            // 
            this.button_exportPeptideChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_exportPeptideChart.Location = new System.Drawing.Point(611, 336);
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
            chartArea3.Name = "ChartArea1";
            this.chart_peptide.ChartAreas.Add(chartArea3);
            this.chart_peptide.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chart_peptide.Legends.Add(legend3);
            this.chart_peptide.Location = new System.Drawing.Point(3, 16);
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
            this.chart_peptide.Size = new System.Drawing.Size(686, 346);
            this.chart_peptide.TabIndex = 0;
            this.chart_peptide.Text = "chart1";
            // 
            // button_exportAllPeptideChart
            // 
            this.button_exportAllPeptideChart.Location = new System.Drawing.Point(134, 14);
            this.button_exportAllPeptideChart.Name = "button_exportAllPeptideChart";
            this.button_exportAllPeptideChart.Size = new System.Drawing.Size(133, 23);
            this.button_exportAllPeptideChart.TabIndex = 9;
            this.button_exportAllPeptideChart.Text = "Export all peptide";
            this.button_exportAllPeptideChart.UseVisualStyleBackColor = true;
            this.button_exportAllPeptideChart.Click += new System.EventHandler(this.button_exportAllPeptideChart_Click);
            // 
            // button_exportProteinChart
            // 
            this.button_exportProteinChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_exportProteinChart.Location = new System.Drawing.Point(611, 228);
            this.button_exportProteinChart.Name = "button_exportProteinChart";
            this.button_exportProteinChart.Size = new System.Drawing.Size(75, 23);
            this.button_exportProteinChart.TabIndex = 10;
            this.button_exportProteinChart.Text = "Export Graph";
            this.button_exportProteinChart.UseVisualStyleBackColor = true;
            this.button_exportProteinChart.Click += new System.EventHandler(this.button_exportProteinChart_Click);
            // 
            // chart1
            // 
            chartArea4.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea4);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend4.Name = "Legend1";
            this.chart1.Legends.Add(legend4);
            this.chart1.Location = new System.Drawing.Point(3, 16);
            this.chart1.Name = "chart1";
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
            this.chart1.Size = new System.Drawing.Size(686, 238);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // txt_source
            // 
            this.txt_source.Location = new System.Drawing.Point(71, 36);
            this.txt_source.Name = "txt_source";
            this.txt_source.Size = new System.Drawing.Size(303, 20);
            this.txt_source.TabIndex = 11;
            // 
            // btn_Browsefolder
            // 
            this.btn_Browsefolder.Location = new System.Drawing.Point(379, 34);
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
            this.label1.Location = new System.Drawing.Point(24, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Source";
            // 
            // comboBox_proteinNameSelector
            // 
            this.comboBox_proteinNameSelector.FormattingEnabled = true;
            this.comboBox_proteinNameSelector.Location = new System.Drawing.Point(512, 35);
            this.comboBox_proteinNameSelector.Name = "comboBox_proteinNameSelector";
            this.comboBox_proteinNameSelector.Size = new System.Drawing.Size(226, 21);
            this.comboBox_proteinNameSelector.TabIndex = 14;
            this.comboBox_proteinNameSelector.SelectedIndexChanged += new System.EventHandler(this.comboBox_proteinNameSelector_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(466, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Protein";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(817, 29);
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
            this.label4_proteinRateConstantValue.Location = new System.Drawing.Point(928, 29);
            this.label4_proteinRateConstantValue.Name = "label4_proteinRateConstantValue";
            this.label4_proteinRateConstantValue.Size = new System.Drawing.Size(0, 13);
            this.label4_proteinRateConstantValue.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(817, 52);
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
            this.label5_Ic.Location = new System.Drawing.Point(928, 52);
            this.label5_Ic.Name = "label5_Ic";
            this.label5_Ic.Size = new System.Drawing.Size(0, 13);
            this.label5_Ic.TabIndex = 19;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "Export all proteins";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar_exportall
            // 
            this.progressBar_exportall.Location = new System.Drawing.Point(24, 43);
            this.progressBar_exportall.Name = "progressBar_exportall";
            this.progressBar_exportall.Size = new System.Drawing.Size(380, 23);
            this.progressBar_exportall.TabIndex = 21;
            // 
            // button2_cancelled
            // 
            this.button2_cancelled.BackgroundImage = global::v2.Properties.Resources.x1;
            this.button2_cancelled.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button2_cancelled.Location = new System.Drawing.Point(405, 43);
            this.button2_cancelled.Name = "button2_cancelled";
            this.button2_cancelled.Size = new System.Drawing.Size(29, 23);
            this.button2_cancelled.TabIndex = 22;
            this.button2_cancelled.UseVisualStyleBackColor = true;
            this.button2_cancelled.Click += new System.EventHandler(this.button2_cancelled_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.38249F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.61751F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3_proteinchart, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox6, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(24, 125);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.47107F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.52893F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1190, 716);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox4.Controls.Add(this.button_exportPeptideChart);
            this.groupBox4.Controls.Add(this.chart_peptide);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(495, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(692, 365);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.dataGridView_peptide);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox3, 2);
            this.groupBox3.Size = new System.Drawing.Size(486, 628);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Peptide";
            // 
            // groupBox3_proteinchart
            // 
            this.groupBox3_proteinchart.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3_proteinchart.Controls.Add(this.button_exportProteinChart);
            this.groupBox3_proteinchart.Controls.Add(this.chart1);
            this.groupBox3_proteinchart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3_proteinchart.Location = new System.Drawing.Point(495, 374);
            this.groupBox3_proteinchart.Name = "groupBox3_proteinchart";
            this.groupBox3_proteinchart.Size = new System.Drawing.Size(692, 257);
            this.groupBox3_proteinchart.TabIndex = 0;
            this.groupBox3_proteinchart.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox6.Controls.Add(this.progressBar_exportall);
            this.groupBox6.Controls.Add(this.button_exportAllPeptideChart);
            this.groupBox6.Controls.Add(this.button2_cancelled);
            this.groupBox6.Controls.Add(this.button1);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(3, 637);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(486, 76);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.txt_source);
            this.groupBox7.Controls.Add(this.btn_Browsefolder);
            this.groupBox7.Controls.Add(this.comboBox_proteinNameSelector);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.label5_Ic);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.label4);
            this.groupBox7.Controls.Add(this.label4_proteinRateConstantValue);
            this.groupBox7.Location = new System.Drawing.Point(24, 23);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(1187, 81);
            this.groupBox7.TabIndex = 23;
            this.groupBox7.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1232, 858);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox7);
            this.MinimumSize = new System.Drawing.Size(1248, 897);
            this.Name = "Form1";
            this.Text = "d2ome: Visualization";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_peptide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_peptide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3_proteinchart.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView_peptide;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_peptide;
        private System.Windows.Forms.Button button_exportAllPeptideChart;
        private System.Windows.Forms.Button button_exportPeptideChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TextBox txt_source;
        private System.Windows.Forms.Button btn_Browsefolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_proteinNameSelector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4_proteinRateConstantValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5_Ic;
        private System.Windows.Forms.Button button_exportProteinChart;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar_exportall;
        private System.Windows.Forms.Button button2_cancelled;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox3_proteinchart;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
    }
}

