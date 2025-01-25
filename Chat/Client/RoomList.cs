namespace Client;

public partial class RoomList : Form
{
    public RoomList()
    {
        InitializeComponent();

        // 방목록(폼)이 활성화 될때마다 새로고침 해야함
        Activated += (s, e) =>
        {
            listBox_roomList.Items.Clear();
        };
    }

    // 방만들기
    private void btn_createRoom_Click(object sender, EventArgs e)
    {
        string roomName = tbx_roomName.Text;
        if (string.IsNullOrEmpty(roomName))
        {
            MessageBox.Show("입력하세요");
            return;
        }
        tbx_roomName.Text = null;
        listBox_roomList.Items.Add(roomName);

        ChatRoom chatRoom = new ChatRoom();
        chatRoom.Text = roomName;
        chatRoom.ShowDialog();
    }

    // 방입장하기
    private void btn_entrance_Click(object sender, EventArgs e)
    {
        if (listBox_roomList.SelectedItem == null)
            return;

        ChatRoom chatRoom = new ChatRoom();
        chatRoom.Text = listBox_roomList.SelectedItem.ToString();
        chatRoom.ShowDialog();
    }
}
