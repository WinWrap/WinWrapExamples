using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Example
{
	public delegate void ReceivedDataDelegate(Connection conn, string data);

	/// <summary>
	/// Summary description for Connection.
	/// </summary>
	public class Connection
	{
		private class StateObject
		{
			public byte[] buffer;

			public StateObject(int bufsize)
			{
				buffer = new byte[bufsize];
			}
		}

		private Socket socket_;
		private int id_;
		private StringBuilder sbRecv_ = new StringBuilder();
        private object lock_ = new object();

		private static int idNext_;

		public Socket Socket
		{
			get { return socket_; }
		}

		public Connection(Socket socket)
		{
			id_ = idNext_;
			if (++idNext_ < 0)
				idNext_ = 0;

			socket_ = socket;
			Recv_Start();
		}

        public Connection(string address, int port) : this(CreateSocket(address, port))
        {
        }

        public string[] GetReceviedData(string separator)
        {
            lock (lock_)
            {
                string data = sbRecv_.ToString();
                sbRecv_.Clear();
                int x = data.LastIndexOf(separator);
                if (x == -1)
                    sbRecv_.Append(data);
                else
                {
                    sbRecv_.Append(data.Substring(x + 2));
                    data = data.Substring(0, x);
                }

                return data.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

		public void Close()
		{
			try
			{
				socket_.Close();
			}
			catch
			{
			}
		}

		public bool Connected
		{
			get { return socket_.Connected; }
		}

		public int Id
		{
			get { return id_; }
		}

		public void Recv_Start()
		{
			StateObject so = new StateObject(0x1000);
			socket_.BeginReceive(so.buffer, 0, so.buffer.Length, 0,
				new AsyncCallback(Recv_Callback), so);
		}

		public void Send(string data)
		{
			if (string.IsNullOrEmpty(data))
				return;
											
			int send = Encoding.UTF8.GetByteCount(data);
			StateObject so = new StateObject(send);
            Encoding.UTF8.GetBytes(data, 0, send, so.buffer, 0);
			socket_.BeginSend(so.buffer, 0, so.buffer.Length, SocketFlags.None,
				new AsyncCallback(Sent_Callback), so);
		}

        private static Socket CreateSocket(string address, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(address);
            IPEndPoint ep = new IPEndPoint(ip, port);
            socket.Connect(ep);
            return socket;
        }

		private void Recv_Callback(IAsyncResult ar)
		{
			try
			{
				StateObject so = (StateObject)ar.AsyncState;
				int read = socket_.EndReceive(ar);
                if (read > 0)
                {
                    string data = Encoding.UTF8.GetString(so.buffer, 0, read);
                    lock (lock_)
                        sbRecv_.Append(data);
                }

				Recv_Start();
			}
			catch
			{
				Close();
			}
		}

		private void Sent_Callback(IAsyncResult ar)
		{
			StateObject so = (StateObject)ar.AsyncState;
		}
	}

    public class Connections : IDisposable
    {
        private Socket listenSocket_;
        private List<Connection> conns_ = new List<Connection>();
        private object lock_ = new object();

        public bool Any
        {
            get
            {
                lock (lock_)
                    return conns_.Count > 0;
            }
        }

        public Connections(int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Blocking = false;
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
            socket.Bind(ep);
            socket.Listen(5);
            socket.BeginAccept(new AsyncCallback(Listen_Callback), socket);
            listenSocket_ = socket;
        }

        public void Dispose()
        {
            if (listenSocket_ != null)
            {
                lock (lock_)
                {
                    while (conns_.Count > 0)
                    {
                        Connection conn = conns_[0];
                        conn.Close();
                        conns_.Remove(conn);
                    }
                }

                listenSocket_.Close();
                listenSocket_ = null;
            }
        }

        public void ForEachConnection(Action<Connection> process)
        {
            lock (lock_)
                foreach (Connection conn in conns_)
                    process(conn);
        }

        private void Listen_Callback(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState;
            Socket s2 = s.EndAccept(ar);
            Connection conn = new Connection(s2);

            lock (lock_)
                conns_.Add(conn);

            listenSocket_.BeginAccept(new AsyncCallback(Listen_Callback), listenSocket_);
        }
    }
}
