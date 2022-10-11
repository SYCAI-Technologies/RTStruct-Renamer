using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FellowOakDicom;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        String fileName;
        DicomFile file;

        List<string> validLabels = new List<string> { "PANCREAS","LIVER", "LIVER CYST", "LIVER LESION", "RIGHT KIDNEY", "RIGHT KIDNEY LESION", "RIGHT KIDNEY CYST","LEFT KIDNEY","LEFT KIDNEY LESION","LEFT KIDNEY CYST","ADK","TS","TNEND","NPMI","IPMN","MCN","NQM","PSEUDOCYST","CAS","SCA","NSP","QS"};
        List<string> errorCount = new List<string> { };

        public Form1()
        {
            InitializeComponent();
        }

        private void toggleGuiVisibility(Boolean status)
        {
            label1.Visible = status;
            textBox1.Visible = status;
            label2.Visible = status;
            textBox2.Visible = status;
            label4.Visible = status;
            textBox3.Visible = status;
            button1.Enabled = status;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toggleGuiVisibility(false);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Choose RTStruct File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "dcm",
                Filter = "dcm files (*.dcm)|*.dcm | All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                fileName = openFileDialog1.FileName;

                file = DicomFile.Open(fileName);
                Console.WriteLine(fileName);
                
                try
                {
                    var roiSequence = file.Dataset.GetSequence(DicomTag.StructureSetROISequence);
                    label3.Text = file.Dataset.GetString(DicomTag.PatientID);
                    
                    var texts = new List<TextBox> { textBox1, textBox2, textBox3 };
                    int c = 0;
                    foreach (var sequence in roiSequence)
                    {
                        
                        var roiName = sequence.GetString(DicomTag.ROIName);
                        Console.WriteLine(roiName);
                        texts.ElementAt(c).Text = roiName;
                        c++;
                    }

                    toggleGuiVisibility(true);
                    Console.WriteLine("Sequence Length: " + roiSequence.Count());
                    if (roiSequence.Count() == 2)
                    {
                        label4.Visible = false;
                        textBox3.Visible = false;
                    }
                    if (roiSequence.Count() == 1)
                    {
                        label2.Visible = false;
                        textBox2.Visible = false;
                    }
                }
                catch (FellowOakDicom.DicomDataException)
                {
                    MessageBox.Show("Please make sure to select a valid RTStruct file.", "File is not valid",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //Save button
        private void button1_Click(object sender, EventArgs e)
        {
            var roiSequence = file.Dataset.GetSequence(DicomTag.StructureSetROISequence);

            var texts = new List<TextBox> { textBox1, textBox2, textBox3 };
            int c = 0;
            foreach (var sequence in roiSequence)
            {
                sequence.AddOrUpdate(DicomTag.ROIName, texts.ElementAt(c).Text);
                c++;
            }

            var roiObservation = file.Dataset.GetSequence(DicomTag.RTROIObservationsSequence);
            c = 0;
            foreach (var sequence in roiObservation)
            {
                sequence.AddOrUpdate(DicomTag.ROIObservationLabel, texts.ElementAt(c).Text);
                c++;
            }


            file.Save(fileName);
            MessageBox.Show("File saved successfully.", "OK",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //abrir el dialogo que permite elegir el archivo
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            //si un archivo se selecciona
            if (result == DialogResult.OK)
            {
                //ponemos el archivo seleccionado en la caja de texto
                this.textBox4.Text = this.folderBrowserDialog1.SelectedPath;

                IEnumerable<string> filesArray = Directory.EnumerateFiles(this.textBox4.Text, "*.dcm", SearchOption.AllDirectories);


                List<string> estudios = new List<string> { };

                foreach (String archivo in filesArray)
                {
                    Debug.WriteLine(archivo);
                    file = DicomFile.Open(archivo);

                    if(file.Dataset.GetString(DicomTag.Modality) == "RTSTRUCT"){
                        try
                        {
                            var roiSequence = file.Dataset.GetSequence(DicomTag.StructureSetROISequence);
                            var idPaciente = file.Dataset.GetString(DicomTag.PatientID);

                            textBox5.Text += idPaciente;
                            textBox5.AppendText(Environment.NewLine);
                            textBox5.Text += "-----------------";
                            textBox5.AppendText(Environment.NewLine);

                            foreach (var sequence in roiSequence)
                            {
                                var roiName = sequence.GetString(DicomTag.ROIName);

                                if (!validLabels.Any(roiName.Contains))
                                {
                                    errorCount.Add(idPaciente);
                                }

                                textBox5.Text += roiName + '\n';
                                textBox5.AppendText(Environment.NewLine);
                            }
                            textBox5.AppendText(Environment.NewLine);
                        }
                        catch (FellowOakDicom.DicomDataException)
                        {
                            MessageBox.Show("Please make sure to select a valid RTStruct file.", "File is not valid",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {

                        //DateTime fechaEstudio = file.Dataset.GetSingleValue<DateTime>(DicomTag.StudyDate);
                        //textBox5.Text += fechaEstudio.ToString();

                        string UIDEstudio = file.Dataset.GetSingleValue<string>(DicomTag.StudyInstanceUID);

                        

                        if (!estudios.Contains(UIDEstudio))
                        {
                            estudios.Add(UIDEstudio);
                            Directory.CreateDirectory(this.textBox4.Text+"\\"+UIDEstudio);
                        }
                        else
                        {
                            if (!File.Exists(this.textBox4.Text + "\\" + UIDEstudio + "\\" + Path.GetFileName(archivo)))
                            {
                                File.Copy(archivo, this.textBox4.Text + "\\" + UIDEstudio + "\\" + Path.GetFileName(archivo), true);
                            }
                            
                        }

                        //textBox5.Text += UIDEstudio;
                        //textBox5.AppendText(Environment.NewLine);
                        //textBox5.Text += "-----------------";
                        //textBox5.AppendText(Environment.NewLine);
                        
                    }
                }
                foreach (String error in errorCount)
                {
                    textBox5.Text += "***ERROR: patient:" + error + "***";
                }
            }
        }
    }
}
