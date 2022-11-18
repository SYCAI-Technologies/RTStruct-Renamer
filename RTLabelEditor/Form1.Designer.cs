namespace RTLabelEditor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_abrir_carpeta = new System.Windows.Forms.Button();
            this.btn_subir_carpeta = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_abrir_local = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelTitulo = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(314, 554);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(320, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(889, 455);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1215, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 554);
            this.panel1.TabIndex = 2;
            // 
            // btn_abrir_carpeta
            // 
            this.btn_abrir_carpeta.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_abrir_carpeta.Location = new System.Drawing.Point(314, 508);
            this.btn_abrir_carpeta.Name = "btn_abrir_carpeta";
            this.btn_abrir_carpeta.Size = new System.Drawing.Size(901, 23);
            this.btn_abrir_carpeta.TabIndex = 3;
            this.btn_abrir_carpeta.Text = "Abrir Carpeta";
            this.btn_abrir_carpeta.UseVisualStyleBackColor = true;
            this.btn_abrir_carpeta.Click += new System.EventHandler(this.btn_abrir_carpeta_Click);
            // 
            // btn_subir_carpeta
            // 
            this.btn_subir_carpeta.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_subir_carpeta.Enabled = false;
            this.btn_subir_carpeta.Location = new System.Drawing.Point(314, 485);
            this.btn_subir_carpeta.Name = "btn_subir_carpeta";
            this.btn_subir_carpeta.Size = new System.Drawing.Size(901, 23);
            this.btn_subir_carpeta.TabIndex = 4;
            this.btn_subir_carpeta.Text = "Subir Carpeta";
            this.btn_subir_carpeta.UseVisualStyleBackColor = true;
            this.btn_subir_carpeta.Click += new System.EventHandler(this.btn_subir_carpeta_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(42, 83);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(889, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 5;
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 6;
            // 
            // btn_abrir_local
            // 
            this.btn_abrir_local.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_abrir_local.Location = new System.Drawing.Point(314, 531);
            this.btn_abrir_local.Name = "btn_abrir_local";
            this.btn_abrir_local.Size = new System.Drawing.Size(901, 23);
            this.btn_abrir_local.TabIndex = 7;
            this.btn_abrir_local.Text = "Abrir Carpeta Local";
            this.btn_abrir_local.UseVisualStyleBackColor = true;
            this.btn_abrir_local.Click += new System.EventHandler(this.btn_abrir_local_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.labelTitulo);
            this.panel2.Controls.Add(this.btnCancelar);
            this.panel2.Controls.Add(this.progressBar1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(278, 182);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(974, 203);
            this.panel2.TabIndex = 8;
            this.panel2.Visible = false;
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelTitulo.Location = new System.Drawing.Point(383, 15);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(93, 21);
            this.labelTitulo.TabIndex = 8;
            this.labelTitulo.Text = "labelTitulo";
            this.labelTitulo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(453, 145);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1446, 554);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btn_subir_carpeta);
            this.Controls.Add(this.btn_abrir_carpeta);
            this.Controls.Add(this.btn_abrir_local);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.Text = "RTStruct-Renamer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TreeView treeView1;
        private DataGridView dataGridView1;
        private Panel panel1;
        private Button btn_abrir_carpeta;
        private Button btn_subir_carpeta;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label label1;
        private Button btn_abrir_local;
        private Panel panel2;
        private Button btnCancelar;
        private Label labelTitulo;
    }
}