using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Common.Share.RSM
{
    public class KeyValueServer
    {
        List<string> list = new List<string>();
        Citizen leader = new Citizen();
        public Bucket Get(string key)
        {
            var index = list.BinarySearch(key);
            var bucket = leader.Read(index);
            return bucket;//decode
        }
        public bool Set(string key, Bucket bucket)
        {
            var index = list.BinarySearch(key);
            return leader.Write(index, bucket);
        }
        public bool Delete(string key)
        {
            var index = list.BinarySearch(key);
            return leader.Write(index, null);
        }
        public string[] Keys()
        {
            return list.ToArray();
        }

    }
    public enum CitizenType
    {
        Citizen = 0,
        Candidate = 1,
        Leader = 2,
    }
    public class Citizen
    {
        /// <summary>
        /// 选举号
        /// </summary>
        public int ElectionNo { get; set; }
        /// <summary>
        /// 当前本地的选举号
        /// </summary>
        public int VotedNo { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }

        private CitizenType Type = CitizenType.Candidate;
        private Citizen[] Followers;
        private Citizen Leader;
        private DateTime lastActiveTime = DateTime.Now;
        private Bucket[] buckets;

        /// <summary>
        /// 开始一个选举
        /// </summary>
        /// <returns></returns>
        public bool StartElection()
        {
            //使用一个新的选举号
            this.ElectionNo++;
            if (SendMessage("Vote", this))
            {
                this.Type = CitizenType.Leader;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 发起一个写操作
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public bool Write(int index, Bucket bucket)
        {
            bucket.LeaderNo = this.ElectionNo;
            bucket.VersionNo++;
            if (SendMessage("Write", bucket))
            {
                //save data to local
                return true;
            }
            if (DateTime.Now - lastActiveTime >= TimeSpan.FromSeconds(30))
            {
                if (StartElection())
                {
                    return this.Write(index, bucket);
                }
            }
            this.Type = CitizenType.Citizen;
            return false;
        }
        public Bucket Read(int index)
        {
            if (!EnsureRecovery(index))
            {
                return null;
            }
            if (SendMessage("Read", index))
            {
                //get bucket from local disk
                return new Bucket();
            }
            else
            {
                this.Type = CitizenType.Citizen;
                return null;
            }
        }
        private bool SendMessage(string type, object data)
        {
            var cutDown = this.Followers.Length;
            List<Task> tasks = new List<Task>();
            foreach (var item in Followers)
            {
                tasks.Add(Task.Run(() =>
                {
                    WebClient client = new WebClient();
                    var url = item.ToString();
                    var result = client.DownloadStringTaskAsync(url).Result;
                    if (result == "OK")
                    {
                        cutDown--;
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray(), TimeSpan.FromSeconds(30));
            if (cutDown <= this.Followers.Length / 2)
            {
                return true;
            }
            return false;
        }

        private bool ReceivedMessage()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket协议

            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 9095);//初始化一个侦听局域网内部所有IP和指定端口

            EndPoint ep = (EndPoint)iep;

            socket.Bind(iep);//绑定这个实例

            while (true)
            {
                byte[] buffer = new byte[1024];//设置缓冲数据流

                socket.ReceiveFrom(buffer, ref ep);//接收数据,并确把数据设置到缓冲流里面
                Console.WriteLine(Encoding.Unicode.GetString(buffer).TrimEnd('\u0000') + " " + DateTime.Now.ToString());
            }
        }
        public bool EnsureRecovery(int index)
        {
            // get bucket from local disk;
            var bucket = new Bucket();
            if (this.ElectionNo == bucket.LeaderNo)
            {
                return true;
            }
            if (SendMessage("Read", index))
            {
                // get max leader no from majority
                var max_ver = 0;
                bucket.LeaderNo = max_ver;
                bucket.LeaderNo = this.ElectionNo;
                bucket.VersionNo = 0;
                //save bucket to local disk
                return true;
            }
            return false;
        }

        public bool DoVote(int election_no, Citizen citizen)
        {
            if (election_no >= this.VotedNo)
            {
                this.VotedNo = election_no;
                this.Type = CitizenType.Citizen;
                this.Leader = citizen;
                return true;
            }
            return false;
        }
        public bool DoWrite(Bucket bucket, Citizen citizen)
        {
            if (bucket.LeaderNo < this.VotedNo)
            {
                return false;
            }
            else
            {
                this.VotedNo = bucket.LeaderNo;
                this.Type = CitizenType.Citizen;
                this.Leader = citizen;
                //save bucket to local disk
                return true;
            }
        }
        public bool DoRead(int index, int election_no, Citizen citizen)
        {
            //get bucket from local

            if (election_no == this.VotedNo)
            {

                return false;
            }
            this.VotedNo = election_no;
            this.Type = CitizenType.Citizen;
            this.Leader = citizen;
            return true;
        }
    }
    public class Bucket
    {
        public int LeaderNo { get; set; }
        public int VersionNo { get; set; }

        public static bool operator ==(Bucket bucket1, Bucket bucket2)
        {
            return true;
        }

        public static bool operator !=(Bucket bucket1, Bucket bucket2)
        {
            return true;
        }

        public string Data { get; set; }
    }

}
