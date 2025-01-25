namespace Client;

public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
    }

    private void btn_login_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbx_id.Text) || string.IsNullOrEmpty(tbx_nickname.Text))
        {
            MessageBox.Show("입력하세요");
            return;
        }

        await Singleton.Instance.LoginAsync();

        Singleton.Instance.Id = tbx_id.Text;
        Singleton.Instance.Nickname = tbx_nickname.Text;
                
        RoomList roomList = new RoomList();
        roomList.ShowDialog();
    }
}