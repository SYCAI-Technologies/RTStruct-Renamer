using FellowOakDicom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using TextBox = System.Windows.Forms.TextBox;

namespace RTLabelRenamer
{
    public partial class Form2 : Form
    {
        DicomFile file;
        int indexRow;
        
        struct RTStruct
        {
            public string path { get; set; }
            public string studyInstanceUID { get; set; }
            public string patientID { get; set; }
            //public string birthDate { get; set; }
            public string patientName { get; set; }
            public List<string> labels { get; set; }
            public List<string> ct { get; set; }
        }
        List<RTStruct> lista = new List<RTStruct> { };
        RTStruct rTStruct;
        public Form2()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            //folderBrowserDialog1.SelectedPath = @"\\192.168.1.59\tooltest";
            //abrir el dialogo que permite elegir el archivo
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            //si un archivo se selecciona
            if (result == DialogResult.OK)
            {
                //ponemos el archivo seleccionado en la caja de texto
                string path = this.folderBrowserDialog1.SelectedPath;

                IEnumerable<string> filesArray = Directory.EnumerateFiles(path, "*.dcm", SearchOption.AllDirectories);



                //Dictionary<string, string> dict = new Dictionary<string, string>();
                //separamos los archivos entre los RTStruct y los CT

                List<string> rtstructArray = new List<string>();
                List<string>  ctArray = new List<string>();

                foreach (string archivo in filesArray)
                {
                    file = DicomFile.Open(archivo);

                    if (file.Dataset.GetString(DicomTag.Modality) == "RTSTRUCT")
                    {
                        //Debug.WriteLine(archivo);
                        rtstructArray.Add(archivo);
                    }
                    else
                    {
                        ctArray.Add(archivo);
                    }

                }

                foreach (string archivo in rtstructArray)
                {
                    file = DicomFile.Open(archivo);

                    RTStruct rt = new RTStruct
                    {
                        path = archivo,
                        patientID = file.Dataset.GetString(DicomTag.PatientID),
                        studyInstanceUID = file.Dataset.GetSingleValue<string>(DicomTag.StudyInstanceUID),
                        //birthDate = file.Dataset.GetSingleValue<string>(DicomTag.PatientBirthDate),
                        patientName = file.Dataset.GetSingleValue<string>(DicomTag.PatientName),
                        labels = new List<string>(),
                        ct = new List<string>()
                    };

                   

                    var roiSequence = file.Dataset.GetSequence(DicomTag.StructureSetROISequence);

                    foreach (var sequence in roiSequence)
                    {
                        var roiName = sequence.GetString(DicomTag.ROIName);
                        rt.labels.Add(roiName);
                    }
                    lista.Add(rt);
                    
                }

                foreach (string archivo in ctArray)
                {
                    file = DicomFile.Open(archivo);
                    string UIDEstudio = file.Dataset.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                    foreach (RTStruct rTStruct in lista)
                    {
                        if(rTStruct.studyInstanceUID == UIDEstudio)
                        {
                            rTStruct.ct.Add(archivo);
                        }
                    }
                }



                cargarDataView();


               /* foreach (RTStruct datos in lista)
                {
                    textBox1.AppendText("-------------------------------------------");
                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText(datos.patientID);
                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText("*******************************************");
                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText(datos.studyInstanceUID);
                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText("-------------------------------------------");
                    textBox1.AppendText(Environment.NewLine);

                    foreach(string label in datos.labels)
                    {
                        textBox1.AppendText(label);
                        textBox1.AppendText(Environment.NewLine);
                    }
               
                    
                    foreach (string cts in datos.ct)
                    {
                        textBox1.AppendText("+++++++++++++++++++++++++++++++++++++");
                        textBox1.AppendText(Environment.NewLine);
                        textBox1.AppendText(cts);
                        textBox1.AppendText(Environment.NewLine);
                    }

                }*/
            }
        }

        private void cargarDataView()
        {

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.MultiSelect = false;
            dataGridView1.RowHeadersVisible = false;


            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.Name = "PatientId";
            column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns.Add(column1);

            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.Name = "StudyId";
            column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns.Add(column2);

            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.Name = "patientName";
            column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns.Add(column3);

            DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn();
            column4.Name = "labels";
            column4.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns.Add(column4);



            //dataGridView1.DataSource = lista;


            for (int j = 0; j < lista.Count; j++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);


                row.Cells[0].Value = lista[j].patientID;
                row.Cells[1].Value = lista[j].studyInstanceUID;
                row.Cells[2].Value = lista[j].patientName;

                for (int i = 0; i < lista[j].labels.Count; i++)
                {
                    row.Cells[3].Value += lista[j].labels.ElementAt(i) + ",";

                }

                dataGridView1.Rows.Add(row);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            panel1.Controls.Clear();

            rTStruct = lista.ElementAt(indexRow);
            
            TextBox TBox;
            int vertPos = 20;
            int i = 1;
            foreach (string label in rTStruct.labels)
            {
                
                TBox = new TextBox();
                TBox.Location = new Point(85, vertPos);
                TBox.Size = new Size(150, 50);
                TBox.TabIndex = i;
                TBox.AccessibleName = label;
                TBox.Text = label;
                panel1.Controls.Add(TBox);
                i++;
                vertPos += 20;
                
                //Debug.WriteLine(label);
            }

            Button guardar = new Button();
            guardar.Location = new Point(85, vertPos);
            guardar.Size = new Size(150, 50);
            guardar.TabIndex = i;
            guardar.Text = "Guardar";

            panel1.Controls.Add(guardar);

            guardar.Click += guardar_Click;


            //Form3 form = new Form3();
            //form.Show();

        }
        void guardar_Click(object sender, EventArgs e)
        {
            List<string> editedLabels = new List<string>();

            lista.ElementAt(indexRow).labels.Clear();
            
            foreach (Control TBox in panel1.Controls)
            {
                int i = 0;
                if(TBox is TextBox)
                {
                    string editedLabel = TBox.Text;
                    editedLabels.Add(editedLabel);
                    lista.ElementAt(indexRow).labels.Add(editedLabel);
                    i++;
                }


            }

            //lista.ElementAt(indexRow) editedLabels;
            
            
            
            //Debug.WriteLine();

            cargarDataView();


        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                
                indexRow = dataGridView1.CurrentRow.Index;
                button2.Enabled = true;
                //Debug.WriteLine(indexRow);
            }
        }
    }

}
