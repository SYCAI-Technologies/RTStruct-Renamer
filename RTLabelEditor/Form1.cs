using FellowOakDicom;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace RTLabelEditor
{
    public partial class Form1 : Form
    {
        //variable que nos dice si estamos en modo offline 
        bool offline = false;

        //variable que almacena el archivo dicom
        DicomFile file;

        //la fila seleccionada en el datagridview
        int indexRow;

        //la ruta donde se cargara el arbol
        string rootpath = @"\\192.168.1.59\tooltest\Hospitales\RX External";
        string path = @"\\192.168.1.59\tooltest\Hospitales";

        //la ruta donde se abrira la carpeta
        string ruta = "";

        //la ruta donde se subiran las carpetas
        string subirRuta = @"\\192.168.1.59\tooltest\Hospitales\00_PARA SUBIR";

        //La lista con las etiquetas correctas
        List<string> validLabels = new List<string> { "PANCREAS", "LIVER", "LIVER CYST", "LIVER LESION", "RIGHT KIDNEY", "RIGHT KIDNEY LESION", "RIGHT KIDNEY CYST", "LEFT KIDNEY", "LEFT KIDNEY LESION", "LEFT KIDNEY CYST", "ADK", "TS", "TNEND", "NPMI", "MCN", "NQM", "PSEUDOCYST", "CAS", "SCA", "NSP", "QS" };

        //lista de archivos a borrar si el usuario cancela la subida
        List<string> archivosParaBorrar = new List<string> { };
       
        //diccionario que contiene el id del estudio como clave y el objeto struct asociado
        Dictionary<string, RTStudy> lista = new Dictionary<string,RTStudy>();

        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker2.WorkerSupportsCancellation = true;
        }

        private void btn_abrir_carpeta_Click(object sender, EventArgs e)
        {
            //hacer visible el panel de carga
            panel2.Visible = true;
            labelTitulo.Text = "Cargando los archivos en el editor...";
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            
            //si ya tenemos una lista de estudios de un momento anterior borramos la lista
            lista = new Dictionary<string, RTStudy>();

            //cargamos todos los archivos seleccionados en un array
            //string[] filesArray = Directory.GetFiles(ruta, "*.dcm", SearchOption.AllDirectories);
            //IEnumerable<string> filesArray = Directory.EnumerateFiles(ruta, "*.dcm", SearchOption.AllDirectories);

            Application.UseWaitCursor = true;
            int tamano = Directory.EnumerateFiles(ruta, "*.dcm", SearchOption.AllDirectories).Count();
            double i = 1;
            foreach (string archivo in Directory.EnumerateFiles(ruta, "*.dcm", SearchOption.AllDirectories))
            {
                //cancelacion
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    double porc = i / tamano;
                    worker.ReportProgress((int)(porc * 100), archivo);


                    file = DicomFile.Open(archivo);
                    //cogemos el id de estudio de los archivos que sera la clave de nuestro diccionario
                    string UIDEstudio = file.Dataset.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                    //veo ke tipo de archivo es
                    if (file.Dataset.GetString(DicomTag.Modality) == "RTSTRUCT")
                    {
                        //compruebo si la key ya se ha añadido al diccionario
                        if (!lista.ContainsKey(UIDEstudio))
                        {
                            RTStudy rt = new RTStudy();

                            rt.path = archivo;
                            rt.patientID = file.Dataset.GetString(DicomTag.PatientID);
                            rt.studyDate = file.Dataset.GetSingleValue<string>(DicomTag.StudyDate);
                            rt.patientName = file.Dataset.GetSingleValue<string>(DicomTag.PatientName);
                            rt.labels = new List<string>();

                            var roiSequence = file.Dataset.GetSequence(DicomTag.StructureSetROISequence);

                            foreach (var sequence in roiSequence)
                            {
                                var roiName = sequence.GetString(DicomTag.ROIName);
                                rt.labels.Add(roiName);
                            }

                            lista.Add(UIDEstudio, rt);

                        }
                        else
                        {
                            RTStudy rt = lista.GetValueOrDefault(UIDEstudio);
                            rt.path = archivo;
                            rt.patientID = file.Dataset.GetString(DicomTag.PatientID);
                            rt.studyDate = file.Dataset.GetSingleValue<string>(DicomTag.StudyDate);
                            rt.patientName = file.Dataset.GetSingleValue<string>(DicomTag.PatientName);
                            rt.labels = new List<string>();

                            var roiSequence = file.Dataset.GetSequence(DicomTag.StructureSetROISequence);

                            foreach (var sequence in roiSequence)
                            {
                                var roiName = sequence.GetString(DicomTag.ROIName);
                                rt.labels.Add(roiName);
                            }
                        }
                    }
                    else
                    {
                        //comprobamos si la key ya se ha asignado
                        if (!lista.ContainsKey(UIDEstudio))
                        {
                            RTStudy rt = new RTStudy();
                            rt.ct = new List<string>();
                            rt.ct.Add(archivo);
                            lista.Add(UIDEstudio, rt);
                        }
                        else
                        {
                            RTStudy rt = lista.GetValueOrDefault(UIDEstudio);
                            rt.ct.Add(archivo);
                        }

                    }
                    i += 1;
                }
            }
            
        }

        private void cargarDataView()
        {
            
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();

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
            column1.MinimumWidth = 2;
            dataGridView1.Columns.Add(column1);

            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.Name = "StudyDate";
            column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            column2.MinimumWidth = 2;
            dataGridView1.Columns.Add(column2);

            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.Name = "patientName";
            column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            column3.MinimumWidth = 2;
            dataGridView1.Columns.Add(column3);

            DataGridViewTextBoxColumn column4 = new DataGridViewTextBoxColumn();
            column4.Name = "labels";
            column4.SortMode = DataGridViewColumnSortMode.NotSortable;
            column4.MinimumWidth = 2;
            dataGridView1.Columns.Add(column4);



            //dataGridView1.DataSource = lista;

            foreach(var item in lista)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = item.Value.patientID;
                row.Cells[1].Value = item.Value.studyDate;
                row.Cells[2].Value = item.Value.patientName;

                if(item.Value.labels != null)
                {
                    int i = 1;
                    foreach (var etiqueta in item.Value.labels)
                    {

                        if (i < item.Value.labels.Count())
                        {
                            row.Cells[3].Value += etiqueta + ",";

                        }
                        else
                        {
                            row.Cells[3].Value += etiqueta;
                        }

                        if (!validLabels.Contains(etiqueta))
                        {
                            row.Cells[0].Style.BackColor = Color.Tomato;
                            row.Cells[1].Style.BackColor = Color.Tomato;
                            row.Cells[2].Style.BackColor = Color.Tomato;
                            row.Cells[3].Style.BackColor = Color.Tomato;
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                        }

                        i++;
                    }
                }
                

                row.Cells[0].Style.Padding = new Padding(0, 0, 0, 0);
                dataGridView1.Rows.Add(row);
            }

            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //dataGridView1.AutoResizeColumns();

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            


        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text=e.UserState.ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            panel2.Visible = false;
            label1.Text = "";
            Application.UseWaitCursor = false;
            if (e.Cancelled == false)
            {
                cargarDataView();
                btn_subir_carpeta.Enabled = true;
                MessageBox.Show("operacion completada");
                progressBar1.Value = 0;
            }
            else
            {
                lista.Clear();
                MessageBox.Show("operacion cancelada");
                progressBar1.Value = 0;
            }
            
        }
        private TreeNode crearArbol(DirectoryInfo directoryInfo)
        {
            TreeNode treeNode = new TreeNode(directoryInfo.Name);
            foreach (var item in directoryInfo.GetDirectories())
            {
                treeNode.Nodes.Add(crearArbol(item));

            }
            foreach (var item in directoryInfo.GetFiles())
            {
                treeNode.Nodes.Add(new TreeNode(item.Name));
            }
            return treeNode;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            treeView1.Nodes.Clear();
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(rootpath);
                treeView1.Nodes.Add(crearArbol(directoryInfo));
            }
            catch (System.IO.IOException err)
            {
                offline = true;
                
                MessageBox.Show("No se puede acceder al servidor remoto, entrando en modo offline");
            }
            if (offline)
            {
               

                rootpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).FullName;
                DirectoryInfo directoryInfo = new DirectoryInfo(rootpath);
                treeView1.Nodes.Add(crearArbol(directoryInfo));
            }
            

            
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            FileAttributes attr;
            if (!offline)
            {
                rootpath = treeView1.SelectedNode.FullPath;

                
                //asegurarme que es un directorio

                ruta = path + "\\" + rootpath;
                attr = File.GetAttributes(ruta);

                //si lo que hay seleccionado no es un directorio vamos al directorio base
                if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    ruta = Directory.GetParent(ruta).FullName;
                }
            }
            else
            {
                rootpath = treeView1.SelectedNode.FullPath;
                ruta = path + "\\" + rootpath;
                attr = File.GetAttributes(ruta);

                //si lo que hay seleccionado no es un directorio vamos al directorio base
                if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    ruta = Directory.GetParent(ruta).FullName;
                }
            }
        }

        private void guardar_Click(object? sender, EventArgs e)
        {
            //guardamos las labels editadas
            List<string> editedLabels = new List<string>();
            lista.ElementAt(indexRow).Value.labels.Clear();

            foreach (Control TBox in panel1.Controls)
            {
                if (TBox is TextBox)
                {
                    if(TBox.Name== "idPaciente")
                    {
                        lista.ElementAt(indexRow).Value.patientID = TBox.Text;
                    }
                    else
                    {
                        string editedLabel = TBox.Text;
                        editedLabels.Add(editedLabel);
                        lista.ElementAt(indexRow).Value.labels.Add(editedLabel);
                    }
                           
                }
            }
            int indexRowBackup = indexRow; 
            //recargar el datagridview con los nuevos datos
            cargarDataView();
            //reseleccionar la ultima fila seleccionada
            dataGridView1.Rows[indexRowBackup].Selected = true;
            //cargar las labels de la fila seleccionada
            cargarLabels(indexRowBackup);
            //si la fila seleccionada esta muy abajo autoscroll abajo para mostrarla
            dataGridView1.FirstDisplayedScrollingRowIndex = indexRowBackup;

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                indexRow = dataGridView1.CurrentRow.Index;
            }

            cargarLabels(indexRow);
        }

        private void cargarLabels(int i)
        {
            panel1.Controls.Clear();

            TextBox TBox;
            int vertPos = 20;

            //patientid label
            Label labPacienteID = new Label();
            labPacienteID.Text = "PatientID";
            labPacienteID.Font = new Font(labPacienteID.Font,FontStyle.Bold);
            labPacienteID.Location = new Point(50, vertPos);
            vertPos += 25;
            panel1.Controls.Add(labPacienteID);

            //patient id textbox
            string idPaciente = lista.ElementAt(i).Value.patientID;
            TBox = new TextBox();
            TBox.Location = new Point(50, vertPos);
            TBox.Size = new Size(150, 50);
            TBox.Text = idPaciente;
            TBox.Name = "idPaciente";
            panel1.Controls.Add(TBox);
            vertPos += 30;

            //patientid label
            Label labEtiqueta = new Label();
            labEtiqueta.Text = "Labels";
            labEtiqueta.Font = new Font(labEtiqueta.Font, FontStyle.Bold);
            labEtiqueta.Location = new Point(50, vertPos);
            vertPos += 25;
            panel1.Controls.Add(labEtiqueta);

            if (lista.ElementAt(i).Value.labels != null)
            {
                //labels textbox
                foreach (string label in lista.ElementAt(i).Value.labels)
                {
                    TBox = new TextBox();
                    TBox.Location = new Point(50, vertPos);
                    TBox.Size = new Size(150, 50);
                    TBox.Text = label;

                    panel1.Controls.Add(TBox);

                    vertPos += 20;

                    if (!validLabels.Any(label.Equals))
                    {
                        TBox.BackColor = Color.Tomato;
                    }
                    else
                    {
                        TBox.BackColor = Color.LightGreen;
                    }
                }
            }
            

            Button guardar = new Button();
            guardar.Location = new Point(50, vertPos);
            guardar.Size = new Size(150, 50);

            guardar.Text = "Guardar";

            panel1.Controls.Add(guardar);

            guardar.Click += guardar_Click;
        }

        private void btn_subir_carpeta_Click(object sender, EventArgs e)
        {
            if (offline)
            {
                //abrir el dialogo que permite elegir el archivo
                DialogResult result = this.folderBrowserDialog1.ShowDialog();

                //si un archivo se selecciona
                if (result == DialogResult.OK)
                {
                    subirRuta = this.folderBrowserDialog1.SelectedPath;
                }
            }

            if (backgroundWorker2.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker2.RunWorkerAsync();
            }
            panel2.Visible = true;
            labelTitulo.Text = "Subiendo los archivos en destino...";
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            
            BackgroundWorker worker = sender as BackgroundWorker;

            Application.UseWaitCursor = true;
            int tamano = lista.Count();
            double i = 1;
            foreach (RTStudy estudio in lista.Values)
            {
                

                double porc = i / tamano;
                
                //creamos las carpetas
                Directory.CreateDirectory(subirRuta + "\\" + estudio.patientID);
                archivosParaBorrar.Add(subirRuta + "\\" + estudio.patientID);
                //accedemos a los archivos
                file = DicomFile.Open(estudio.path);
                var roiSequence = file.Dataset.GetSequence(DicomTag.StructureSetROISequence);

                //cambiamos las labels
                int labelIndex = 0;
                foreach (var sequence in roiSequence)
                {
                    sequence.AddOrUpdate(DicomTag.ROIName, estudio.labels.ElementAt(labelIndex));
                    labelIndex++;
                }
                var roiObservation = file.Dataset.GetSequence(DicomTag.RTROIObservationsSequence);
                labelIndex = 0;
                foreach (var sequence in roiObservation)
                {
                    sequence.AddOrUpdate(DicomTag.ROIObservationLabel, estudio.labels.ElementAt(labelIndex));
                    labelIndex++;
                }

                //cambiar la id del paciente en el RT
                file.Dataset.AddOrUpdate(DicomTag.PatientID, estudio.patientID);

                file.Save(subirRuta + "\\" + estudio.patientID + "\\" + Path.GetFileName(estudio.path));
                worker.ReportProgress((int)(porc * 100), subirRuta + "\\" + estudio.patientID + "\\" + Path.GetFileName(estudio.path));
                if (estudio.ct != null)
                {
                    foreach (string ct in estudio.ct)
                    {
                        //cancelacion
                        if (worker.CancellationPending == true)
                        {

                            e.Cancel = true;
                            break;
                        }
                        else
                        {
                            //cambiar la id del paciente en los CT
                            file = DicomFile.Open(ct);
                            file.Dataset.AddOrUpdate(DicomTag.PatientID, estudio.patientID);

                            //File.Copy(ct, subirRuta + "\\" + estudio.patientID + "\\" + Path.GetFileName(ct), true);
                            file.Save(subirRuta + "\\" + estudio.patientID + "\\" + Path.GetFileName(ct));
                            worker.ReportProgress((int)(porc * 100), subirRuta + "\\" + estudio.patientID + "\\" + Path.GetFileName(ct));
                        }
                    }
                }
                i += 1;
             
            }
            
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.UserState.ToString();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Text = "Cancelando...";
            Application.UseWaitCursor = false;
            panel2.Visible = false;
            progressBar1.Value = 0;
            if (e.Cancelled==false)
            {
                MessageBox.Show("Archivos subidos con exito");
            }
            else
            {
                //limpiamos los archivos que hemos copiado
                foreach (string archivo in archivosParaBorrar)
                {
                    if (Directory.Exists(archivo))
                    {
                        Directory.Delete(archivo, true);
                    }
                }

                MessageBox.Show("Operacion de subida cancelada");
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btn_abrir_local_Click(object sender, EventArgs e)
        {
            //abrir el dialogo que permite elegir el archivo
            DialogResult result = this.folderBrowserDialog1.ShowDialog();

            //si un archivo se selecciona
            if (result == DialogResult.OK)
            {
                //subirRuta = this.folderBrowserDialog1.SelectedPath;
                ruta = this.folderBrowserDialog1.SelectedPath;
                panel2.Visible = true;
                labelTitulo.Text = "Cargando los archivos en el editor...";
                if (backgroundWorker1.IsBusy != true)
                {
                    // Start the asynchronous operation.
                    backgroundWorker1.RunWorkerAsync();
                }

            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (backgroundWorker2.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker2.CancelAsync();
            }
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}