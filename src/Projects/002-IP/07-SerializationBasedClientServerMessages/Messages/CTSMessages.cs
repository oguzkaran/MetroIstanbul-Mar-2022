using System;

namespace CSD
{
    [Serializable]
    public class CTSConnect
    {
        private string m_nick;

        public CTSConnect()
        { }

        public CTSConnect(string nick)
        {
            m_nick = nick;
        }

        public string Nick
        {
            get { return m_nick; }
            set { m_nick = value; }
        }
    }

    [Serializable]
    public class CTSSend
    {
        private string m_msg;

        public CTSSend()
        { }

        public CTSSend(string msg)
        {
            m_msg = msg;
        }

        public string Msg
        {
            get { return m_msg; }
            set { m_msg = value; }
        }
    }
}
