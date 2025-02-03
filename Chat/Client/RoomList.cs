using Core;

namespace Client;

public partial class RoomList : Form
{
    public RoomList()
    {
        InitializeComponent();
        Singleton.Instance.CreateRoomResponsed += CreateRoomResponsed;
        Singleton.Instance.RoomListResponsed += RoomListResponsed;

        FormClosing += (s, e) =>
        {
            Singleton.Instance.CreateRoomResponsed -= CreateRoomResponsed;
            Singleton.Instance.RoomListResponsed -= RoomListResponsed;
            Singleton.Instance.Socket.Shutdown(System.Net.Sockets.SocketShutdown.Send);
        };

        // 방목록(폼)이 활성화 될때마다 새로고침 해야함
        Activated += (s, e) =>
        {
            button1_Click(null, EventArgs.Empty);
        };
    }

    // 방만들기
    private async void btn_createRoom_Click(object sender, EventArgs e)
    {
        string roomName = tbx_roomName.Text;
        if (string.IsNullOrEmpty(roomName))
        {
            MessageBox.Show("입력하세요");
            return;
        }

        CreateRoomRequestPacket packet = new CreateRoomRequestPacket(roomName);
        await Singleton.Instance.Socket.SendAsync(packet.Serialize(), System.Net.Sockets.SocketFlags.None);
    }

    // 방입장하기
    private void btn_entrance_Click(object sender, EventArgs e)
    {
        if (listBox_roomList.SelectedItem == null)
            return;

        IAsyncResult ar = null;
        ar = BeginInvoke(() =>
        {
            ChatRoom chatRoom = new ChatRoom();
            chatRoom.Text = listBox_roomList.SelectedItem.ToString();
            chatRoom.ShowDialog();
            EndInvoke(ar);
        });
    }

    // 방 목록 새로고침
    private async void button1_Click(object sender, EventArgs e)
    {
        listBox_roomList.Items.Clear();
        RoomListRequestPacket packet = new RoomListRequestPacket();
        await Singleton.Instance.Socket.SendAsync(packet.Serialize(), System.Net.Sockets.SocketFlags.None);
    }

    private void CreateRoomResponsed(object? sender, EventArgs e)
    {
        CreateRoomResponsePacket packet = (CreateRoomResponsePacket)sender!;
        if (packet.Code == 200)
        {
            listBox_roomList.Items.Add(tbx_roomName.Text);

            IAsyncResult ar = null;
            ar = BeginInvoke(() =>
            {
                ChatRoom chatRoom = new ChatRoom();
                chatRoom.Text = tbx_roomName.Text;
                tbx_roomName.Text = null;
                chatRoom.ShowDialog();
                EndInvoke(ar);
            });
        }
    }

    private void RoomListResponsed(object? sender, EventArgs e)
    {
        RoomListResponsePacket packet = (RoomListResponsePacket)sender!;
        foreach (var item in packet.RoomNames)
        {
            listBox_roomList.Items.Add(item);
        }
    }
}
