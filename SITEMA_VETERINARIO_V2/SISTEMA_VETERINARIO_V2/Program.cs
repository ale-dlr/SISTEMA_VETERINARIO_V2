using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace SISTEMA_VETERINARIO_V2
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public partial class Form1 : Form
    {
        private static string connectionString = "Data Source=veterinaria.db";

        public Form1()
        {
            InicializarBaseDeDatos();
            ConfigurarMenu();
        }

        private void InicializarBaseDeDatos()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string sql = @"
                    CREATE TABLE IF NOT EXISTS Tutores (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Documento TEXT,
                        Telefono TEXT,
                        Direccion TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Pacientes (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        TutorId INTEGER,
                        Nombre TEXT NOT NULL,
                        Especie TEXT,
                        Raza TEXT,
                        Peso REAL,
                        Edad INTEGER,
                        FOREIGN KEY(TutorId) REFERENCES Tutores(Id)
                    );
                    CREATE TABLE IF NOT EXISTS Consultas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        PacienteId INTEGER,
                        Fecha TEXT,
                        Motivo TEXT,
                        Temperatura REAL,
                        DiagnosticoVeterinario TEXT,
                        MedicamentosRecetados TEXT,
                        FOREIGN KEY(PacienteId) REFERENCES Pacientes(Id)
                    );
                ";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ConfigurarMenu()
        {
            this.Text = "Sistema Veterinario v2";
            this.Size = new Size(380, 320);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lbl = new Label { Text = "CONTROL CLÍNICO VETERINARIO", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(65, 20), AutoSize = true };
            this.Controls.Add(lbl);

            Button btn1 = new Button { Text = "1. Registrar Tutor", Location = new Point(65, 65), Width = 230, Height = 35 };
            btn1.Click += (s, e) => new FormTutor().ShowDialog();
            this.Controls.Add(btn1);

            Button btn2 = new Button { Text = "2. Registrar Mascota", Location = new Point(65, 115), Width = 230, Height = 35 };
            btn2.Click += (s, e) => new FormMascota().ShowDialog();
            this.Controls.Add(btn2);

            Button btn3 = new Button { Text = "3. Registrar Consulta", Location = new Point(65, 165), Width = 230, Height = 35 };
            btn3.Click += (s, e) => new FormConsulta().ShowDialog();
            this.Controls.Add(btn3);

            Button btn4 = new Button { Text = "4. Ver Expediente Clínico", Location = new Point(65, 215), Width = 230, Height = 35 };
            btn4.Click += (s, e) => new FormExpediente().ShowDialog();
            this.Controls.Add(btn4);
        }
    }

    public class FormTutor : Form
    {
        private TextBox txtNombre, txtDoc, txtTel, txtDir;
        private static string connectionString = "Data Source=veterinaria.db";

        public FormTutor()
        {
            this.Text = "Registrar Tutor";
            this.Size = new Size(350, 260);
            this.StartPosition = FormStartPosition.CenterParent;

            Controls.Add(new Label { Text = "Nombre:", Location = new Point(20, 20), AutoSize = true });
            txtNombre = new TextBox { Location = new Point(110, 20), Width = 190 };
            Controls.Add(txtNombre);

            Controls.Add(new Label { Text = "Documento:", Location = new Point(20, 60), AutoSize = true });
            txtDoc = new TextBox { Location = new Point(110, 60), Width = 190 };
            Controls.Add(txtDoc);

            Controls.Add(new Label { Text = "Teléfono:", Location = new Point(20, 100), AutoSize = true });
            txtTel = new TextBox { Location = new Point(110, 100), Width = 190 };
            Controls.Add(txtTel);

            Controls.Add(new Label { Text = "Dirección:", Location = new Point(20, 140), AutoSize = true });
            txtDir = new TextBox { Location = new Point(110, 140), Width = 190 };
            Controls.Add(txtDir);

            Button btn = new Button { Text = "Guardar", Location = new Point(110, 180), Width = 100 };
            btn.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text)) { MessageBox.Show("Escriba el nombre."); return; }
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    string q = "INSERT INTO Tutores (Nombre, Documento, Telefono, Direccion) VALUES (@n, @d, @t, @dir)";
                    using (var cmd = new SqliteCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@n", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@d", txtDoc.Text);
                        cmd.Parameters.AddWithValue("@t", txtTel.Text);
                        cmd.Parameters.AddWithValue("@dir", txtDir.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("¡Tutor guardado con éxito!");
                this.Close();
            };
            Controls.Add(btn);
        }
    }

    public class FormMascota : Form
    {
        private ComboBox cmbTutores;
        private TextBox txtNombre, txtEspecie, txtRaza, txtPeso, txtEdad;
        private static string connectionString = "Data Source=veterinaria.db";

        public FormMascota()
        {
            this.Text = "Registrar Mascota";
            this.Size = new Size(350, 330);
            this.StartPosition = FormStartPosition.CenterParent;

            Controls.Add(new Label { Text = "Tutor:", Location = new Point(20, 20), AutoSize = true });
            cmbTutores = new ComboBox { Location = new Point(110, 20), Width = 190, DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cmbTutores);

            Controls.Add(new Label { Text = "Nombre:", Location = new Point(20, 60), AutoSize = true });
            txtNombre = new TextBox { Location = new Point(110, 60), Width = 190 };
            Controls.Add(txtNombre);

            Controls.Add(new Label { Text = "Especie:", Location = new Point(20, 100), AutoSize = true });
            txtEspecie = new TextBox { Location = new Point(110, 100), Width = 190 };
            Controls.Add(txtEspecie);

            Controls.Add(new Label { Text = "Raza:", Location = new Point(20, 140), AutoSize = true });
            txtRaza = new TextBox { Location = new Point(110, 140), Width = 190 };
            Controls.Add(txtRaza);

            Controls.Add(new Label { Text = "Peso (kg):", Location = new Point(20, 180), AutoSize = true });
            txtPeso = new TextBox { Location = new Point(110, 180), Width = 190 };
            Controls.Add(txtPeso);

            Controls.Add(new Label { Text = "Edad:", Location = new Point(20, 220), AutoSize = true });
            txtEdad = new TextBox { Location = new Point(110, 220), Width = 190 };
            Controls.Add(txtEdad);

            CargarTutores();

            Button btn = new Button { Text = "Guardar", Location = new Point(110, 255), Width = 100 };
            btn.Click += (s, e) =>
            {
                if (cmbTutores.SelectedItem == null) { MessageBox.Show("Seleccione un tutor."); return; }
                var kv = (KeyValuePair<int, string>)cmbTutores.SelectedItem;
                double.TryParse(txtPeso.Text, out double peso);
                int.TryParse(txtEdad.Text, out int edad);

                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    string q = "INSERT INTO Pacientes (TutorId, Nombre, Especie, Raza, Peso, Edad) VALUES (@tid, @n, @esp, @raza, @p, @ed)";
                    using (var cmd = new SqliteCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@tid", kv.Key);
                        cmd.Parameters.AddWithValue("@n", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@esp", txtEspecie.Text);
                        cmd.Parameters.AddWithValue("@raza", txtRaza.Text);
                        cmd.Parameters.AddWithValue("@p", peso);
                        cmd.Parameters.AddWithValue("@ed", edad);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("¡Mascota guardada con éxito!");
                this.Close();
            };
            Controls.Add(btn);
        }

        private void CargarTutores()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string q = "SELECT Id, Nombre FROM Tutores";
                using (var cmd = new SqliteCommand(q, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cmbTutores.Items.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            cmbTutores.DisplayMember = "Value";
            cmbTutores.ValueMember = "Key";
        }
    }

    public class FormConsulta : Form
    {
        private ComboBox cmbMascotas;
        private TextBox txtMotivo, txtTemp, txtDiag, txtMed;
        private static string connectionString = "Data Source=veterinaria.db";

        public FormConsulta()
        {
            this.Text = "Registrar Consulta";
            this.Size = new Size(370, 330);
            this.StartPosition = FormStartPosition.CenterParent;

            Controls.Add(new Label { Text = "Mascota:", Location = new Point(20, 20), AutoSize = true });
            cmbMascotas = new ComboBox { Location = new Point(120, 20), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cmbMascotas);

            Controls.Add(new Label { Text = "Motivo:", Location = new Point(20, 60), AutoSize = true });
            txtMotivo = new TextBox { Location = new Point(120, 60), Width = 200 };
            Controls.Add(txtMotivo);

            Controls.Add(new Label { Text = "Temperatura:", Location = new Point(20, 100), AutoSize = true });
            txtTemp = new TextBox { Location = new Point(120, 100), Width = 200 };
            Controls.Add(txtTemp);

            Controls.Add(new Label { Text = "Diagnóstico:", Location = new Point(20, 140), AutoSize = true });
            txtDiag = new TextBox { Location = new Point(120, 140), Width = 200, Height = 40, Multiline = true };
            Controls.Add(txtDiag);

            Controls.Add(new Label { Text = "Medicamentos:", Location = new Point(20, 190), AutoSize = true });
            txtMed = new TextBox { Location = new Point(120, 190), Width = 200, Height = 40, Multiline = true };
            Controls.Add(txtMed);

            CargarMascotas();

            Button btn = new Button { Text = "Guardar", Location = new Point(120, 245), Width = 100 };
            btn.Click += (s, e) =>
            {
                if (cmbMascotas.SelectedItem == null) { MessageBox.Show("Seleccione una mascota."); return; }
                var kv = (KeyValuePair<int, string>)cmbMascotas.SelectedItem;
                double.TryParse(txtTemp.Text, out double temp);

                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    string q = "INSERT INTO Consultas (PacienteId, Fecha, Motivo, Temperatura, DiagnosticoVeterinario, MedicamentosRecetados) VALUES (@pid, @f, @m, @t, @diag, @med)";
                    using (var cmd = new SqliteCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@pid", kv.Key);
                        cmd.Parameters.AddWithValue("@f", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@m", txtMotivo.Text);
                        cmd.Parameters.AddWithValue("@t", temp);
                        cmd.Parameters.AddWithValue("@diag", txtDiag.Text);
                        cmd.Parameters.AddWithValue("@med", txtMed.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("¡Consulta guardada con éxito!");
                this.Close();
            };
            Controls.Add(btn);
        }

        private void CargarMascotas()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string q = "SELECT Id, Nombre FROM Pacientes";
                using (var cmd = new SqliteCommand(q, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cmbMascotas.Items.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            cmbMascotas.DisplayMember = "Value";
            cmbMascotas.ValueMember = "Key";
        }
    }

    public class FormExpediente : Form
    {
        private ComboBox cmbMascotas;
        private TextBox txtRes;
        private static string connectionString = "Data Source=veterinaria.db";

        public FormExpediente()
        {
            this.Text = "Expediente Clínico";
            this.Size = new Size(480, 380);
            this.StartPosition = FormStartPosition.CenterParent;

            Controls.Add(new Label { Text = "Mascota:", Location = new Point(20, 20), AutoSize = true });
            cmbMascotas = new ComboBox();
            cmbMascotas.Location = new Point(90, 20);
            cmbMascotas.Width = 230;
            cmbMascotas.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cmbMascotas);

            Button btn = new Button { Text = "Ver", Location = new Point(330, 18), Width = 90, Height = 25 };
            btn.Click += (s, e) => Mostrar();
            Controls.Add(btn);

            txtRes = new TextBox { Location = new Point(20, 60), Width = 410, Height = 260, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };
            Controls.Add(txtRes);

            CargarMascotas();
        }

        private void CargarMascotas()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string q = "SELECT Id, Nombre FROM Pacientes";
                using (var cmd = new SqliteCommand(q, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cmbMascotas.Items.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            cmbMascotas.DisplayMember = "Value";
            cmbMascotas.ValueMember = "Key";
        }

        private void Mostrar()
        {
            if (cmbMascotas.SelectedItem == null) { MessageBox.Show("Seleccione una mascota."); return; }
            var kv = (KeyValuePair<int, string>)cmbMascotas.SelectedItem;
            string res = "";

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string q1 = "SELECT p.Nombre, p.Especie, p.Raza, p.Peso, p.Edad, t.Nombre, t.Telefono FROM Pacientes p INNER JOIN Tutores t ON p.TutorId = t.Id WHERE p.Id = @id";
                using (var cmd = new SqliteCommand(q1, conn))
                {
                    cmd.Parameters.AddWithValue("@id", kv.Key);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res += $"=== EXPEDIENTE ===\r\nPaciente: {reader.GetString(0)} ({reader.GetString(1)} - {reader.GetString(2)})\r\n";
                            res += $"Peso: {reader.GetDouble(3)} kg | Edad: {reader.GetInt32(4)} años\r\n";
                            res += $"Tutor: {reader.GetString(5)} (Tel: {reader.GetString(6)})\r\n-----------------------------------\r\n";
                        }
                    }
                }

                string q2 = "SELECT Fecha, Motivo, Temperatura, DiagnosticoVeterinario, MedicamentosRecetados FROM Consultas WHERE PacienteId = @id";
                using (var cmd = new SqliteCommand(q2, conn))
                {
                    cmd.Parameters.AddWithValue("@id", kv.Key);
                    using (var reader = cmd.ExecuteReader())
                    {
                        int n = 1;
                        while (reader.Read())
                        {
                            res += $"\n[Visita #{n++}] Fecha: {reader.GetString(0)}\r\n";
                            res += $"  - Motivo: {reader.GetString(1)} ({reader.GetDouble(2)} °C)\r\n";
                            res += $"  - Diagnóstico: {reader.GetString(3)}\r\n";
                            res += $"  - Receta: {reader.GetString(4)}\r\n";
                        }
                    }
                }
            }
            txtRes.Text = res;
        }
    }
}