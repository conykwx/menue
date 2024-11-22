using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

public class FileProgressForm : Form
{
    private ProgressBar progressBar;
    private Button btnOpenFile;
    private Label lblProgress;

    public FileProgressForm()
    {
        this.Text = "Читання файлу з ProgressBar";
        this.Size = new System.Drawing.Size(400, 150);

        progressBar = new ProgressBar { Location = new System.Drawing.Point(20, 50), Width = 350 };
        btnOpenFile = new Button { Text = "Відкрити файл", Location = new System.Drawing.Point(20, 20) };
        lblProgress = new Label { Text = "Прогрес: 0%", Location = new System.Drawing.Point(20, 80), AutoSize = true };

        btnOpenFile.Click += BtnOpenFile_Click;

        this.Controls.Add(progressBar);
        this.Controls.Add(btnOpenFile);
        this.Controls.Add(lblProgress);
    }

    private void BtnOpenFile_Click(object sender, EventArgs e)
    {
        using (OpenFileDialog ofd = new OpenFileDialog())
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofd.FileName;
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    long fileSize = fs.Length;
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    long totalRead = 0;

                    progressBar.Maximum = 100;

                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalRead += bytesRead;
                        int progress = (int)(totalRead * 100 / fileSize);
                        progressBar.Value = progress;
                        lblProgress.Text = $"Прогрес: {progress}%";
                        Application.DoEvents(); // Оновлює UI
                    }
                }

                MessageBox.Show("Файл прочитаний успішно!");
            }
        }
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new FileProgressForm());
    }
}
