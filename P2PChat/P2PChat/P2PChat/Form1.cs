using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;

namespace P2PChat
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer connectionTimer;
        private int remainingSeconds = 30;
        private TcpClient tcpClient;
        private Form progressForm;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            // �T�O���s�ƥ�j�w
            btnEnableTCP.Click += new EventHandler(btnEnableTCP_Click);
        }

        private void InitializeTimer()
        {
            connectionTimer = new System.Windows.Forms.Timer();
            connectionTimer.Interval = 1000; // 1��
            connectionTimer.Tick += ConnectionTimer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // �����J�ɪ���l��
            btnEnableTCP.Enabled = true;
            txtClientIP.Text = "";
            txtClientPORT.Text = "";
        }

        private void ShowProgressForm()
        {
            progressForm = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                Text = "�s����"
            };

            Label lblProgress = new Label
            {
                Text = $"���b�s�����A��...\n�Ѿl�ɶ��G{remainingSeconds}��",
                AutoSize = true,
                Location = new Point(20, 20)
            };

            progressForm.Controls.Add(lblProgress);
            progressForm.Show();
        }

        private void UpdateProgressForm()
        {
            if (progressForm != null && !progressForm.IsDisposed)
            {
                Label lblProgress = (Label)progressForm.Controls[0];
                lblProgress.Text = $"���b�s�����A��...\n�Ѿl�ɶ��G{remainingSeconds}��";
            }
        }

        private async void btnEnableTCP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClientIP.Text) || string.IsNullOrEmpty(txtClientPORT.Text))
            {
                MessageBox.Show("�п�JIP�a�}�MPort", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // �}�l�s���p��
                remainingSeconds = 30;
                connectionTimer.Start();
                btnEnableTCP.Enabled = false;
                ShowProgressForm();

                // ���ճs��
                tcpClient = new TcpClient();
                var connectTask = tcpClient.ConnectAsync(txtClientIP.Text, int.Parse(txtClientPORT.Text));

                // ���ݳs�������ζW��
                using (var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    try
                    {
                        await Task.WhenAny(connectTask, Task.Delay(30000, cts.Token));

                        if (!connectTask.IsCompleted)
                        {
                            throw new TimeoutException("�s���W��");
                        }

                        // �ˬd�s���O�_�u�����\
                        if (!tcpClient.Connected)
                        {
                            throw new SocketException((int)SocketError.ConnectionRefused);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        throw new TimeoutException("�s���W��");
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                        {
                            throw new Exception("���A�����ҰʩΩڵ��s��");
                        }
                        throw;
                    }
                }

                // �p�G���\�s��
                connectionTimer.Stop();
                if (progressForm != null && !progressForm.IsDisposed)
                {
                    progressForm.Close();
                }
                MessageBox.Show("�w�g���\�s��", "���\", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // �}�Ҳ�ѵ���
                ChatForm chatForm = new ChatForm();
                chatForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                connectionTimer.Stop();
                btnEnableTCP.Enabled = true;
                if (progressForm != null && !progressForm.IsDisposed)
                {
                    progressForm.Close();
                }
                MessageBox.Show($"�s�����ѡG{ex.Message}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConnectionTimer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            UpdateProgressForm();

            if (remainingSeconds <= 0)
            {
                connectionTimer.Stop();
                btnEnableTCP.Enabled = true;

                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient = null;
                }

                if (progressForm != null && !progressForm.IsDisposed)
                {
                    Label lblProgress = (Label)progressForm.Controls[0];
                    lblProgress.Text = "���A���L�^��";
                    lblProgress.ForeColor = Color.Red;
                }
            }
        }
    }
}
