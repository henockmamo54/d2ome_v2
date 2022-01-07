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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridView_peptide = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chart_peptide = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_exportPeptideChart = new System.Windows.Forms.Button();
            this.button_exportAllPeptideChart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_peptide)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_peptide)).BeginInit();
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
            this.dataGridView_peptide.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.dataGridView_peptide);
            this.groupBox1.Location = new System.Drawing.Point(12, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 576);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Peptides";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chart_peptide);
            this.groupBox2.Location = new System.Drawing.Point(451, 34);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(674, 314);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // chart_peptide
            // 
            this.chart_peptide.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.Name = "ChartArea1";
            this.chart_peptide.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart_peptide.Legends.Add(legend1);
            this.chart_peptide.Location = new System.Drawing.Point(6, 16);
            this.chart_peptide.Name = "chart_peptide";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series1.Legend = "Legend1";
            series1.MarkerColor = System.Drawing.Color.Black;
            series1.MarkerSize = 7;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Purple;
            series2.Legend = "Legend1";
            series2.MarkerSize = 7;
            series2.Name = "Series3";
            this.chart_peptide.Series.Add(series1);
            this.chart_peptide.Series.Add(series2);
            this.chart_peptide.Size = new System.Drawing.Size(662, 292);
            this.chart_peptide.TabIndex = 0;
            this.chart_peptide.Text = "chart1";
            // 
            // button_exportPeptideChart
            // 
            this.button_exportPeptideChart.Location = new System.Drawing.Point(1044, 354);
            this.button_exportPeptideChart.Name = "button_exportPeptideChart";
            this.button_exportPeptideChart.Size = new System.Drawing.Size(75, 23);
            this.button_exportPeptideChart.TabIndex = 8;
            this.button_exportPeptideChart.Text = "Export Graph";
            this.button_exportPeptideChart.UseVisualStyleBackColor = true;
            this.button_exportPeptideChart.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button_exportAllPeptideChart
            // 
            this.button_exportAllPeptideChart.Location = new System.Drawing.Point(344, 616);
            this.button_exportAllPeptideChart.Name = "button_exportAllPeptideChart";
            this.button_exportAllPeptideChart.Size = new System.Drawing.Size(75, 23);
            this.button_exportAllPeptideChart.TabIndex = 9;
            this.button_exportAllPeptideChart.Text = "Export All";
            this.button_exportAllPeptideChart.UseVisualStyleBackColor = true;
            this.button_exportAllPeptideChart.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1153, 653);
            this.Controls.Add(this.button_exportAllPeptideChart);
            this.Controls.Add(this.button_exportPeptideChart);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "d2ome_v2";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_peptide)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_peptide)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView_peptide;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_exportPeptideChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_peptide;
        private System.Windows.Forms.Button button_exportAllPeptideChart;
    }
}

