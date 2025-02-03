using Core;

namespace Client;

public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
        Singleton.Instance.LoginResponsed += LoginResponsed;
        FormClosing += (s, e) =>
        {
            Singleton.Instance.LoginResponsed -= LoginResponsed;
        };
    }

    private async void btn_login_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbx_id.Text) || string.IsNullOrEmpty(tbx_nickname.Text))
        {
            MessageBox.Show("입력하세요");
            return;
        }

        await Singleton.Instance.ConnectAsync();
        LoginRequestPacket packet = new LoginRequestPacket(tbx_id.Text, tbx_nickname.Text);
        await Singleton.Instance.Socket.SendAsync(packet.Serialize(), System.Net.Sockets.SocketFlags.None);

    }

    private void LoginResponsed(object? sender, EventArgs e)
    {
        LoginResponsePacket packet = (LoginResponsePacket)sender!;
        if (packet.Code == 200)
        {
            Singleton.Instance.Id = tbx_id.Text;
            Singleton.Instance.Nickname = tbx_nickname.Text;

            IAsyncResult ar = null;
            ar = BeginInvoke(() =>
            {
                RoomList roomList = new RoomList();
                roomList.ShowDialog();
                EndInvoke(ar);
            });
        }
        else
        {
            Singleton.Instance.Socket.Shutdown(System.Net.Sockets.SocketShutdown.Send);
        }
    }
}